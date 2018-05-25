using System;
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

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Tell [Player Name] [Message]", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Tell();
            notify = new Mock<INotify>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void Tell_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
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
            tagWrapper.Setup(e => e.WrapInTag("Who would you like to tell what?", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_OneParameter()
        {
            Mock<IParameter> parm1 = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag("What would you like to tell them?", TagType.Info)).Returns("message");
            parm1.Setup(e => e.ParameterValue).Returns("pc");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_TwoParameterPcNotFound()
        {
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IParameter> parm2 = new Mock<IParameter>();
            Mock<IParameter> parm3 = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag("Unable to find pc to tell them.", TagType.Info)).Returns("message");
            pc.Setup(e => e.KeyWords).Returns(new List<string>());
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>() { pc.Object });
            parm1.Setup(e => e.ParameterValue).Returns("pc");
            parm2.Setup(e => e.ParameterValue).Returns("hi");
            parm3.Setup(e => e.ParameterValue).Returns("there");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object, parm3.Object });

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Tell_PerformCommand_TwoParameterPcFound()
        {
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IParameter> parm2 = new Mock<IParameter>();
            Mock<IParameter> parm3 = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag("keyword tells you -- hi there", TagType.Communication)).Returns("message");
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>() { pc.Object });
            parm1.Setup(e => e.ParameterValue).Returns("pc");
            parm2.Setup(e => e.ParameterValue).Returns("hi");
            parm3.Setup(e => e.ParameterValue).Returns("there");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object, parm3.Object });

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
        }
    }
}

