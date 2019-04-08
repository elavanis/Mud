using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.GameDateTime.Interface;
using Objects.GameDateTime.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class TimeUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Time();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
        }

        [TestMethod]
        public void Time_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Time", result.ResultMessage);
        }

        [TestMethod]
        public void Time_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Time"));
        }

        [TestMethod]
        public void Tell_PerformCommand_NoParameter()
        {
            Mock<IInGameDateTime> inGameDateTime = new Mock<IInGameDateTime>();
            Mock<IGameDateTime> gameDateTime = new Mock<IGameDateTime>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.ToString()).Returns("time");

            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("time", result.ResultMessage);
        }
    }
}
