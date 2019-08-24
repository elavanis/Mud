using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.World.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class TellUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<INotify> notify;
        Mock<IFindObjects> findObjects;
        Mock<IRoom> room;
        Mock<IWorld> world;
        Mock<IPlayerCharacter> pc;
        Mock<IParameter> parm1;
        Mock<IParameter> parm2;
        Mock<IParameter> parm3;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Tell();
            notify = new Mock<INotify>();
            findObjects = new Mock<IFindObjects>();
            room = new Mock<IRoom>();
            world = new Mock<IWorld>();
            pc = new Mock<IPlayerCharacter>();
            parm1 = new Mock<IParameter>();
            parm2 = new Mock<IParameter>();
            parm3 = new Mock<IParameter>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "pc")).Returns(new List<INonPlayerCharacter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            pc.Setup(e => e.KeyWords).Returns(new List<string>());
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>() { pc.Object });
            parm1.Setup(e => e.ParameterValue).Returns("pc");
            parm2.Setup(e => e.ParameterValue).Returns("hi");
            parm3.Setup(e => e.ParameterValue).Returns("there");

            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.World = world.Object;
        }

        [TestMethod]
        public void Tell_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Tell [Player Name] [Message]", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Tell"));
        }

        [TestMethod]
        public void Tell_PerformCommand_NoParameter()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Who would you like to tell what?", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_OneParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to tell them?", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_TwoParameterPcNotFound()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object, parm3.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find pc to tell them.", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_TwoParameterPcFound()
        {
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object, parm3.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
        }
    }
}