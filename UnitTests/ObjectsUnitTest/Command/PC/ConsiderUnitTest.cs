using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Global.LevelDifference.Interface;
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
    public class ConsiderUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob1;
        Mock<IMobileObject> mob2;
        Mock<ICommand> mockCommand;
        Mock<IParameter> parm1;
        Mock<IFindObjects> findObjects;
        Mock<IEvaluateLevelDifference> evaluateLevel;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob1 = new Mock<IMobileObject>();
            mob2 = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            parm1 = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            evaluateLevel = new Mock<IEvaluateLevelDifference>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            parm1.Setup(e => e.ParameterValue).Returns("mob");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob1.Object, "mob", 0, false, false, true, true, false)).Returns(mob2.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            command = new Consider();
        }

        [TestMethod]
        public void Consider_Instructions()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Consider [Mob]", result.ResultMessage);
        }


        [TestMethod]
        public void Consider_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Consider"));
        }

        [TestMethod]
        public void Consider_NoParams()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob1.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Who would you like to consider attacking?", result.ResultMessage);
        }

        [TestMethod]
        public void Consider_ConsiderMob()
        {
            IResult result = command.PerformCommand(mob1.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Who would you like to consider attacking?", result.ResultMessage);
        }


        [TestMethod]
        public void Consider_WriteUnitTests()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
