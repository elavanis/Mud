using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class EmoteUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> pc;
        Mock<INotify> notify;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            pc = new Mock<IPlayerCharacter>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            pc.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            command = new Emote();
        }

        [TestMethod]
        public void Emote_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Emote \"[your emote message]\"", result.ResultMessage);
        }

        [TestMethod]
        public void Emote_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Emote"));
        }

        [TestMethod]
        public void Emote_PerformCommand_NoParameter()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to emote?", result.ResultMessage);
        }

        [TestMethod]
        public void Emote_PerformCommand_Parameter()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("Bow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
            notify.Verify(e => e.Room(pc.Object, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }


    }
}
