using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class PutUnitTest
    {
        Put command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<ICommand> mockCommand;
        Mock<IParameter> itemParameter;
        Mock<IParameter> containerParameter;
        Mock<IFindObjects> findObjects;
        Mock<IItem> item;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            itemParameter = new Mock<IParameter>();
            containerParameter = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            itemParameter.Setup(e => e.ParameterValue).Returns("parmValue");
            containerParameter.Setup(e => e.ParameterValue).Returns("parmValue2");
            findObjects.Setup(e => e.FindHeldItemsOnMob(performer.Object, itemParameter.Object.ParameterValue)).Returns(new List<IItem>() { item.Object });
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, containerParameter.Object.ParameterValue)).Returns(new List<IItem>() { item.Object });

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            command = new Put();
        }

        [TestMethod]
        public void Write_PutTest()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Put_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Put [Item Name] [Container]", result.ResultMessage);
        }

        [TestMethod]
        public void Put_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Put"));
        }

        [TestMethod]
        public void Put_PerformCommand_NoParameters()
        {
            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Put [Item Name] [Container]", result.ResultMessage);
        }

        [TestMethod]
        public void Put_PerformCommand_OneParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { itemParameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Put [Item Name] [Container]", result.ResultMessage);
        }

        [TestMethod]
        public void Put_PerformCommand_TwoParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { itemParameter.Object, containerParameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Put [Item Name] [Container]", result.ResultMessage);
        }
    }
}
