using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.God;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class PossessUnitTest
    {

        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<ICommand> commandMock;
        Mock<IMobileObject> mob;


        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            commandMock = new Mock<ICommand>();

            tagWrapper.Setup(e => e.WrapInTag("Possess {Mob Keyword}", TagType.Info)).Returns("possess");
            tagWrapper.Setup(e => e.WrapInTag("You release control of mob.", TagType.Info)).Returns("release");
            mob.Setup(e => e.SentenceDescription).Returns("mob");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Possess();
        }

        [TestMethod]
        public void Possess_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("possess", result.ResultMessage);
        }

        [TestMethod]
        public void Possess_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Possess"));
        }

        [TestMethod]
        public void Possess_PerformCommand_NoParameterNoPossession()
        {
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>());
            //mob.Setup(e => e.PossedMob).Returns(null);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were not possessing anyone.", result.ResultMessage);
        }

        [TestMethod]
        public void Possess_PerformCommand_NoParameterPossessing()
        {
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.PossedMob).Returns(mob.Object);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("release", result.ResultMessage);
            mob.VerifySet(e => e.PossedMob = null);
            mob.VerifySet(e => e.PossingMob = null);
        }

    }
}
