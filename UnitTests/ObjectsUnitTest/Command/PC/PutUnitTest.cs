using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
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
        Mock<IContainer> container;
        Mock<IItem> itemContainer;
        Mock<IEngine> engine;
        Mock<IEvent> eventEngine;
        List<IItem> containerItems;
        List<IItem> performerItems;

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
            container = new Mock<IContainer>();
            itemContainer = container.As<IItem>();
            engine = new Mock<IEngine>();
            eventEngine = new Mock<IEvent>();
            containerItems = new List<IItem>();
            performerItems = new List<IItem>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            itemParameter.Setup(e => e.ParameterValue).Returns("parmValue");
            containerParameter.Setup(e => e.ParameterValue).Returns("parmValue2");
            findObjects.Setup(e => e.FindHeldItemsOnMob(performer.Object, It.IsAny<string>())).Returns(new List<IItem>());
            findObjects.Setup(e => e.FindHeldItemsOnMob(performer.Object, itemParameter.Object.ParameterValue)).Returns(new List<IItem>() { item.Object });
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, It.IsAny<string>())).Returns(new List<IItem>());
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, containerParameter.Object.ParameterValue)).Returns(new List<IItem>() { itemContainer.Object });
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, itemParameter.Object.ParameterValue)).Returns(new List<IItem>() { item.Object });
            performer.Setup(e => e.Room).Returns(room.Object);
            performer.Setup(e => e.Items).Returns(performerItems);
            engine.Setup(e => e.Event).Returns(eventEngine.Object);
            container.Setup(e => e.Items).Returns(containerItems);
            performerItems.Add(item.Object);
            item.Setup(e => e.SentenceDescription).Returns("item");
            itemContainer.Setup(e => e.SentenceDescription).Returns("container");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            command = new Put();
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
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You put item in container.", result.ResultMessage);
            Assert.IsFalse(performerItems.Contains(item.Object));
            Assert.IsTrue(containerItems.Contains(item.Object));
        }

        [TestMethod]
        public void Put_PerformCommand_ItemNotFound()
        {
            itemParameter.Setup(e => e.ParameterValue).Returns("not found");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { itemParameter.Object, containerParameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not seem to be carrying not found.", result.ResultMessage);
            Assert.IsTrue(performerItems.Contains(item.Object));
            Assert.IsFalse(containerItems.Contains(item.Object));
        }

        [TestMethod]
        public void Put_PerformCommand_ContainerNotFound()
        {
            containerParameter.Setup(e => e.ParameterValue).Returns("not found");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { itemParameter.Object, containerParameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find not found.", result.ResultMessage);
            Assert.IsTrue(performerItems.Contains(item.Object));
            Assert.IsFalse(containerItems.Contains(item.Object));
        }

        [TestMethod]
        public void Put_PerformCommand_NotAContainer()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { itemParameter.Object, itemParameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not put things in item.", result.ResultMessage);
            Assert.IsTrue(performerItems.Contains(item.Object));
            Assert.IsFalse(containerItems.Contains(item.Object));
        }
    }
}
