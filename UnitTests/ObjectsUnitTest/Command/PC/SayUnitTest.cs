using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SayUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<INotify> notify;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            command = new Say();
        }

        [TestMethod]
        public void Say_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Say [Message]", result.ResultMessage);
        }

        [TestMethod]
        public void Say_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Say"));
        }

        [TestMethod]
        public void Say_PerformCommand_NoParameters()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to say?", result.ResultMessage);
        }

        [TestMethod]
        public void Say_PerformCommand_Say()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<ICanMobDoSomething> canDoSomething = new Mock<ICanMobDoSomething>();

            parameter.Setup(e => e.ParameterValue).Returns("this is a test message");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { new Mock<IPlayerCharacter>().Object });
            canDoSomething.Setup(e => e.Hear(mob.Object, npc.Object)).Returns(true);
            mob.Setup(e => e.SentenceDescription).Returns("SentenceDescription");

            GlobalReference.GlobalValues.CanMobDoSomething = canDoSomething.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
            notify.Verify(e => e.Room(mob.Object, null, mob.Object.Room, It.IsAny<ITranslationMessage>(), new List<IMobileObject>() { mob.Object }, false, true), Times.Once);
        }
    }
}
