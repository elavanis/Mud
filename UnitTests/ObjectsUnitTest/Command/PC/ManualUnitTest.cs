using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.Commands.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class ManualUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IPlayerCharacter>();
            mockCommand = new Mock<ICommand>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            command = new Manual();
        }

        [TestMethod]
        public void Manual_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Do you really need to Man the Manual?", result.ResultMessage);
        }

        [TestMethod]
        public void Manual_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("Man"));
            Assert.IsTrue(result.Contains("Manual"));
        }

        [TestMethod]
        public void Manual_PerformCommand_AllCommands()
        {
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            SortedDictionary<string, IMobileObjectCommand> godCommands = new SortedDictionary<string, IMobileObjectCommand>();
            godCommands.Add("god", null);
            commandList.Setup(e => e.GodCommands).Returns(godCommands);
            SortedDictionary<string, IMobileObjectCommand> pcCommands = new SortedDictionary<string, IMobileObjectCommand>();
            pcCommands.Add("pc", null);
            commandList.Setup(e => e.PcCommands).Returns(pcCommands);
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            mob.Setup(e => e.God).Returns(true);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("In Game Commands\r\nGod Commands\r\ngod\r\n\r\nMortal Commands\r\npc", result.ResultMessage);
        }

        [TestMethod]
        public void Manual_PerformCommand_SpecificCommandGod()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> godCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> godCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> pcCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> pcCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IResult> godResult = new Mock<IResult>();
            Mock<IResult> pcResult = new Mock<IResult>();
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            param.Setup(e => e.ParameterValue).Returns("god");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { param.Object });
            godCommand.Setup(e => e.Instructions).Returns(godResult.Object);
            godCommands.Add("GOD", godCommand.Object);
            commandList.Setup(e => e.GodCommandsLookup).Returns(godCommands);
            pcCommand.Setup(e => e.Instructions).Returns(pcResult.Object);
            pcCommands.Add("PC", pcCommand.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(pcCommands);
            mob.Setup(e => e.God).Returns(true);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(godResult.Object, result);
        }

        [TestMethod]
        public void Manual_PerformCommand_SpecificCommandPc()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> godCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> godCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> pcCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> pcCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IResult> godResult = new Mock<IResult>();
            Mock<IResult> pcResult = new Mock<IResult>();
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            param.Setup(e => e.ParameterValue).Returns("pc");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { param.Object });
            godCommand.Setup(e => e.Instructions).Returns(godResult.Object);
            godCommands.Add("GOD", godCommand.Object);
            commandList.Setup(e => e.GodCommandsLookup).Returns(godCommands);
            pcCommand.Setup(e => e.Instructions).Returns(pcResult.Object);
            pcCommands.Add("PC", pcCommand.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(pcCommands);
            mob.Setup(e => e.God).Returns(true);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(pcResult.Object, result);
        }

        [TestMethod]
        public void Manual_PerformCommand_SpecificCommandNotFound()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> godCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> godCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> pcCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> pcCommands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IResult> godResult = new Mock<IResult>();
            Mock<IResult> pcResult = new Mock<IResult>();
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            param.Setup(e => e.ParameterValue).Returns("dog");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { param.Object });
            godCommand.Setup(e => e.Instructions).Returns(godResult.Object);
            godCommands.Add("GOD", godCommand.Object);
            commandList.Setup(e => e.GodCommandsLookup).Returns(godCommands);
            pcCommand.Setup(e => e.Instructions).Returns(pcResult.Object);
            pcCommands.Add("PC", pcCommand.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(pcCommands);
            mob.Setup(e => e.God).Returns(true);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreSame("Unable to find that command.", result.ResultMessage);
        }
    }
}
