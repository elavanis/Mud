using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using Objects.Item.Items.BulletinBoard.Interface;
using Objects.Mob.Interface;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Objects.Item.Item;

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

        [TestInitialize]
        public void Setup()
        {
            fileIO = new Mock<IFileIO>();
            mob = new Mock<IMobileObject>();
            settings = new Mock<ISettings>();
            seriliazation = new Mock<ISerialization>();

            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "mob" });
            settings.Setup(e => e.BulletinBoardDirectory).Returns("bb");
            seriliazation.Setup(e => e.Serialize(It.IsAny<List<IMessage>>())).Returns("serializedMessages");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Serialization = seriliazation.Object;

            bulletinBoard = new Objects.Item.Items.BulletinBoard.BulletinBoard();
            bulletinBoard.Zone = 1;
            bulletinBoard.Id = 2;
            PropertyInfo propertyInfo = bulletinBoard.GetType().GetProperty("messages", BindingFlags.Instance | BindingFlags.NonPublic);
            messages = (List<IMessage>)propertyInfo.GetValue(bulletinBoard);
        }

        [TestMethod]
        public void BulletinBoard_WriteUnitTests()
        {
            Assert.AreEqual(-1, 0);
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

        //[TestMethod]
        //public void BulletinBoard_Remove()
        //{
        //    bulletinBoard.Post(mob.Object, "subject", "text");

        //    fileIO.Verify(e => e.WriteFile("bb\\1-2.BulletinBoard", "serializedMessages"), Times.Once);
        //}
    }
}
