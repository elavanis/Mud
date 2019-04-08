using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Command.PC;
using Objects.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class FollowUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Follow();
        }

        [TestMethod]
        public void Follow_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Follow {Target}", result.ResultMessage);
        }

        [TestMethod]
        public void Follow_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Follow"));
        }

        [TestMethod]
        public void Follow_PerformCommand_NoParamsNotFollowing()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual(true, result.AllowAnotherCommand);
            Assert.AreEqual("You are not following anyone.", result.ResultMessage);
        }

        [TestMethod]
        public void Follow_PerformCommand_NoParams()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IMobileObject> target = new Mock<IMobileObject>();
            mob.Setup(e => e.FollowTarget).Returns(target.Object);
            target.Setup(e => e.SentenceDescription).Returns("SentenceDescription");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual(false, result.AllowAnotherCommand);
            Assert.AreEqual("You stop following SentenceDescription.", result.ResultMessage);
        }

        [TestMethod]
        public void Follow_PerformCommand_MobNotInRoom()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("mob");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IMobileObject> target = new Mock<IMobileObject>();
            mob.Setup(e => e.FollowTarget).Returns(target.Object);
            target.Setup(e => e.SentenceDescription).Returns("SentenceDescription");

            Mock<IFindObjects> findThings = new Mock<IFindObjects>();
            findThings.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "mob", 0, false, false, true, true, false)).Returns<IBaseObject>(null);
            GlobalReference.GlobalValues.FindObjects = findThings.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual(true, result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find mob.", result.ResultMessage);
        }

        [TestMethod]
        public void Follow_PerformCommand_MobInRoom()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("mob");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IMobileObject> target = new Mock<IMobileObject>();
            mob.Setup(e => e.FollowTarget).Returns(target.Object);
            target.Setup(e => e.SentenceDescription).Returns("SentenceDescription");

            Mock<IFindObjects> findThings = new Mock<IFindObjects>();
            findThings.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "mob", 0, false, false, true, true, true)).Returns(target.Object);
            GlobalReference.GlobalValues.FindObjects = findThings.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual(false, result.AllowAnotherCommand);
            Assert.AreEqual("You start to follow SentenceDescription.", result.ResultMessage);
        }

        [TestMethod]
        public void Follow_PerformCommand_FollowSelf()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("mob");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IMobileObject> target = new Mock<IMobileObject>();
            mob.Setup(e => e.FollowTarget).Returns(target.Object);
            target.Setup(e => e.SentenceDescription).Returns("SentenceDescription");

            Mock<IFindObjects> findThings = new Mock<IFindObjects>();
            findThings.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "mob", 0, false, false, true, true, true)).Returns(mob.Object);
            GlobalReference.GlobalValues.FindObjects = findThings.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual(true, result.AllowAnotherCommand);
            Assert.AreEqual("You probably shouldn't follow yourself.  People might think your strange.", result.ResultMessage);
        }
    }
}
