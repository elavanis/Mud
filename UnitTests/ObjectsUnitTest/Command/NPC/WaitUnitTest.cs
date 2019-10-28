using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.NPC;
using Objects.Global;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.NPC
{
    [TestClass]
    public class WaitUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            mockCommand = new Mock<ICommand>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Wait();
        }

        [TestMethod]
        public void Wait_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Wait", result.ResultMessage);
        }

        [TestMethod]
        public void Wait_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Wait"));
        }

        [TestMethod]
        public void WaitUnitTest_Pc()
        {
            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.AreEqual(null, result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void WaitUnitTest_Npc()
        {
            IResult result = command.PerformCommand(npc.Object, mockCommand.Object);

            Assert.AreEqual(null, result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
        }
    }
}