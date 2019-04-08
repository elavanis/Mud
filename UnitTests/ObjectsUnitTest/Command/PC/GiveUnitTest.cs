using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class GiveUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<INonPlayerCharacter> performer;
        Mock<INonPlayerCharacter> receiver;
        Mock<IItem> item;
        Mock<ICommand> mockCommand;
        Mock<IParameter> parameter1;
        Mock<IParameter> parameter2;
        Mock<IRoom> room;
        Mock<IReceiver> receiverPersonality;
        Mock<IFindObjects> findObjects;
        List<IItem> performerItems;
        List<IItem> receiverItems;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<INonPlayerCharacter>();
            receiver = new Mock<INonPlayerCharacter>();
            item = new Mock<IItem>();
            mockCommand = new Mock<ICommand>();
            parameter1 = new Mock<IParameter>();
            parameter2 = new Mock<IParameter>();
            room = new Mock<IRoom>();
            receiverPersonality = new Mock<IReceiver>();
            findObjects = new Mock<IFindObjects>();
            performerItems = new List<IItem>();
            receiverItems = new List<IItem>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter1.Object, parameter2.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { performer.Object, receiver.Object });
            performer.Setup(e => e.Room).Returns(room.Object);
            performer.Setup(e => e.Items).Returns(performerItems);
            receiver.Setup(e => e.Personalities).Returns(new List<IPersonality>() { receiverPersonality.Object });
            receiver.Setup(e => e.KeyWords).Returns(new List<string>() { "npc" });
            receiver.Setup(e => e.Items).Returns(receiverItems);
            receiver.Setup(e => e.SentenceDescription).Returns("receiver sentence");
            parameter1.Setup(e => e.ParameterValue).Returns("item");
            parameter2.Setup(e => e.ParameterValue).Returns("npc");
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            item.Setup(e => e.SentenceDescription).Returns("item sentence");
            findObjects.Setup(e => e.FindHeldItemsOnMob(performer.Object, parameter1.Object.ParameterValue)).Returns(new List<IItem>() { item.Object });
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, parameter2.Object.ParameterValue)).Returns(new List<INonPlayerCharacter>() { receiver.Object });

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            receiverPersonality.Setup(e => e.ReceivedItem(performer.Object, receiver.Object, item.Object)).Returns(new Result("message", false));

            command = new Give();
        }

        [TestMethod]
        public void Give_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Give [Item Name] [Person]", result.ResultMessage);
        }

        [TestMethod]
        public void Give_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Give"));
        }

        [TestMethod]
        public void Give_PerformCommand_NoParams()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.AreEqual("Give [Item Name] [Person]", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Give_PerformCommand_1Params()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter1.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.AreEqual("Give [Item Name] [Person]", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Give_PerformCommand_NoItem()
        {
            findObjects.Setup(e => e.FindHeldItemsOnMob(performer.Object, parameter1.Object.ParameterValue)).Returns(new List<IItem>());

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.AreEqual("You do not seem to be carrying item.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Give_PerformCommand_NoPerson()
        {
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, parameter2.Object.ParameterValue)).Returns(new List<INonPlayerCharacter>());

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.AreEqual("You could not find npc.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Give_PerformCommand_GiveItem()
        {
            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            Assert.AreEqual("message", result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
            receiverPersonality.Verify(e => e.ReceivedItem(performer.Object, receiver.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Give_PerformCommand_GiveItemNoPersonality()
        {
            receiver.Setup(e => e.Personalities).Returns(new List<IPersonality>());

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            Assert.AreEqual("You give receiver sentence item sentence.", result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
            receiverPersonality.Verify(e => e.ReceivedItem(performer.Object, receiver.Object, item.Object), Times.Never);
        }
    }
}
