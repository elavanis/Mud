using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class TitleUnitTest
    {
        Title command;
        Mock<ITagWrapper> tagWrapper;
        Mock<ICommand> mockCommand;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mockCommand = new Mock<ICommand>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            pc.Setup(e => e.AvailableTitles).Returns(new HashSet<string>() { "title1", "title2" });
            parameter.Setup(e => e.ParameterValue).Returns("1");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Title();
        }

        [TestMethod]
        public void Title_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Title {Message Id}", result.ResultMessage);
        }

        [TestMethod]
        public void Title_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Title"));
        }

        [TestMethod]
        public void Title_NPC()
        {
            IResult result = command.PerformCommand(npc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Only players can set a title.", result.ResultMessage);
        }

        [TestMethod]
        public void Title_PC_NoParams()
        {
            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("0 \t title\r\n1 \t title", result.ResultMessage);
        }

        [TestMethod]
        public void Title_PC_SetTitle()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Title updated to title2", result.ResultMessage);
        }

        [TestMethod]
        public void Title_PC_NoTitleFound()
        {
            parameter.Setup(e => e.ParameterValue).Returns("3");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("No title found at that position.", result.ResultMessage);
        }

        [TestMethod]
        public void Title_PC_NotANumber()
        {
            parameter.Setup(e => e.ParameterValue).Returns("x");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Title {Message Id}", result.ResultMessage);
        }

        [TestMethod]
        public void Title_NoTitles()
        {
            pc.Setup(e => e.AvailableTitles).Returns(new HashSet<string>());

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Sorry, you have not earned any titles yet.", result.ResultMessage);
        }
    }
}
