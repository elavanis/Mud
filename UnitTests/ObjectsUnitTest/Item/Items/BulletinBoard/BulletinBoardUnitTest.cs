using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using Objects.Item.Items.BulletinBoard.Interface;
using Objects.Mob.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Objects.Item.Item;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items.BulletinBoard
{
    [TestClass]
    public class BulletinBoardUnitTest
    {
        Objects.Item.Items.BulletinBoard.BulletinBoard bulletinBoard;
        Mock<IFileIO> fileIO;
        Mock<IMobileObject> mob;
        Mock<ISettings> settings;
        Mock<ISerialization> seriliazation;
        List<IMessage> messages;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMessage> message;
        Mock<IDefaultValues> defaultValues;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            fileIO = new Mock<IFileIO>();
            mob = new Mock<IMobileObject>();
            settings = new Mock<ISettings>();
            seriliazation = new Mock<ISerialization>();
            tagWrapper = new Mock<ITagWrapper>();
            message = new Mock<IMessage>();
            messages = new List<IMessage>();
            defaultValues = new Mock<IDefaultValues>();

            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "mob" });
            settings.Setup(e => e.BulletinBoardDirectory).Returns("bb");
            seriliazation.Setup(e => e.Serialize(It.IsAny<List<IMessage>>())).Returns("serializedMessages");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            message.Setup(e => e.Poster).Returns("mob");
            message.Setup(e => e.Read(mob.Object)).Returns("written message");
            fileIO.Setup(e => e.ReadAllText("bb\\1-2.BulletinBoard")).Returns("fileContents");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Serialization = seriliazation.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            bulletinBoard = new Objects.Item.Items.BulletinBoard.BulletinBoard("examine", "look", "sentence", "short");
            bulletinBoard.Zone = 1;
            bulletinBoard.Id = 2;
            PropertyInfo propertyInfo = bulletinBoard.GetType().GetProperty("messages", BindingFlags.Instance | BindingFlags.NonPublic);
            messages = (List<IMessage>)propertyInfo.GetValue(bulletinBoard);
            messages.Add(message.Object);
        }

        [TestMethod]
        public void BulletinBoard_Constructor()
        {
            Assert.IsTrue(bulletinBoard.Attributes.Contains(ItemAttribute.NoGet));
        }

        [TestMethod]
        public void BulletinBoard_Post()
        {
            bulletinBoard.Post(mob.Object, "subject", "text");

            fileIO.Verify(e => e.WriteFile("bb\\1-2.BulletinBoard", "serializedMessages"), Times.Once);
        }

        [TestMethod]
        public void BulletinBoard_Remove_NoItems()
        {
            messages.Clear();

            IResult result = bulletinBoard.Remove(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no message at 1.", result.ResultMessage);
            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod]
        public void BulletinBoard_Remove_NotOurItem()
        {
            message.Setup(e => e.Poster).Returns("notUs");

            IResult result = bulletinBoard.Remove(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not remove someone else's post.", result.ResultMessage);
            Assert.AreEqual(1, messages.Count);
            Assert.IsTrue(messages.Contains(message.Object));
        }

        [TestMethod]
        public void BulletinBoard_Remove_GodModeOn()
        {
            message.Setup(e => e.Poster).Returns("notUs");
            mob.Setup(e => e.God).Returns(true);

            IResult result = bulletinBoard.Remove(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You removed the post on the bulletin board.", result.ResultMessage);
            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod]
        public void BulletinBoard_Remove_Ours()
        {
            message.Setup(e => e.Poster).Returns("mob");

            IResult result = bulletinBoard.Remove(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You removed your post on the bulletin board.", result.ResultMessage);
            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod]
        public void BulletinBoard_Read_NotFound()
        {
            messages.Clear();

            IResult result = bulletinBoard.Read(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find message 1.", result.ResultMessage);
        }

        [TestMethod]
        public void BulletinBoard_Read_MessageFound()
        {
            IResult result = bulletinBoard.Read(mob.Object, 1);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("written message", result.ResultMessage);
        }

        [TestMethod]
        public void BulletinBoard_CalculateCost()
        {
            ulong cost = bulletinBoard.CalculateCost(mob.Object);

            Assert.AreEqual(10u, cost);
        }

        [TestMethod]
        public void BulletinBoard_CalculateCost_MaxPrice()
        {
            for (int i = 0; i < 25; i++)
            {
                messages.Add(message.Object);
            }

            ulong cost = bulletinBoard.CalculateCost(mob.Object);

            Assert.AreEqual(ulong.MaxValue, cost);
        }

        [TestMethod]
        public void BulletinBoard_FinishLoad()
        {
            Mock<IMessage> message1 = new Mock<IMessage>();
            List<IMessage> localMessages = new List<IMessage>();
            Mock<IMessage> message2 = new Mock<IMessage>();

            messages.Clear();
            seriliazation.Setup(e => e.Deserialize<List<IMessage>>("fileContents")).Returns(localMessages);
            message1.Setup(e => e.PostedDate).Returns(new Objects.GameDateTime.GameDateTime(DateTime.Now));
            message2.Setup(e => e.PostedDate).Returns(new Objects.GameDateTime.GameDateTime(DateTime.Now.AddDays(-360)));
            localMessages.Add(message1.Object);
            localMessages.Add(message2.Object);

            bulletinBoard.FinishLoad();

            Assert.AreEqual(1, messages.Count);
            Assert.IsTrue(messages.Contains(message1.Object));
            Assert.IsFalse(messages.Contains(message2.Object));
        }
    }
}
