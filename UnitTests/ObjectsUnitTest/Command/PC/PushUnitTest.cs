using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Interface;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Objects.Mob.MobileObject;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]

    public class PushUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IFindObjects> findObjects;
        Mock<IMobileObject> mobileObject;
        Mock<IBaseObject> baseObject;
        Mock<ICommand> mockCommand;
        Mock<IParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            findObjects = new Mock<IFindObjects>();
            mobileObject = new Mock<IMobileObject>();
            baseObject = new Mock<IBaseObject>();
            mockCommand = new Mock<ICommand>();
            parameter = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mobileObject.Object, "item", 0, true, true, false, false, true)).Returns(baseObject.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mobileObject.Object, "notFound", 0, true, true, false, false, true)).Returns((IBaseObject)null);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            command = new Push();
        }

        [TestMethod]
        public void Push_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Push {Item Keyword}", result.ResultMessage);
        }

        [TestMethod]
        public void Push_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Push"));
        }

        [TestMethod]
        public void Push_PerformCommand_NoParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mobileObject.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Push {Item Keyword}", result.ResultMessage);
        }

        [TestMethod]
        public void Push_PerformCommand_NothingFound()
        {
            parameter.Setup(e => e.ParameterValue).Returns("notFound");

            IResult result = command.PerformCommand(mobileObject.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You could not find the notFound to push it.", result.ResultMessage);
        }

        [TestMethod]
        public void Push_PerformCommand_TurnObect()
        {
            parameter.Setup(e => e.ParameterValue).Returns("item");

            command.PerformCommand(mobileObject.Object, mockCommand.Object);
            baseObject.Verify(e => e.Turn(mobileObject.Object, mockCommand.Object), Times.Once);
        }
    }
}
