using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using Objects.Command.Interface;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class DropUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            evnt = new Mock<IEvent>();
            engine = new Mock<IEngine>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            command = new Drop();
        }

        [TestMethod]
        public void Drop_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Drop [Item Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Drop_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Drop"));
        }

        [TestMethod]
        public void Drop_PerformCommand_NoParameter()
        {
            Mock<IMobileObject> mock = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());


            IResult result = command.PerformCommand(mock.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to drop?", result.ResultMessage);
        }

        [TestMethod]
        public void Drop_PerformCommand_ItemNotFound()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("parm");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IItem> item = new Mock<IItem>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns<IItem>(null);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find parm.", result.ResultMessage);
        }

        [TestMethod]
        public void Drop_PerformCommand_ItemFound()
        {
            List<IItem> items = new List<IItem>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(items);
            Mock<IRoom> room = new Mock<IRoom>();
            mob.Setup(e => e.Room).Returns(room.Object);
            List<IItem> roomItems = new List<IItem>();
            room.Setup(e => e.Items).Returns(roomItems);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("parm");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            items.Add(item.Object);
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns(item.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You dropped SentenceDescription.", result.ResultMessage);
        }
    }
}
