using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Mob.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Room.Interface;
using Objects.Item.Interface;
using static Objects.Item.Item;
using Objects.Item.Items.Interface;
using Objects.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Notify.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class GetUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IEngine> engine;
        Mock<IEvent> mockEvent;
        Mock<INotify> notify;
        Mock<IMoneyToCoins> moneyToCoins;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            engine = new Mock<IEngine>();
            mockEvent = new Mock<IEvent>();
            notify = new Mock<INotify>();
            moneyToCoins = new Mock<IMoneyToCoins>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            engine.Setup(e => e.Event).Returns(mockEvent.Object);
            moneyToCoins.Setup(e => e.FormatedAsCoins(It.IsAny<ulong>())).Returns((ulong x) => (x.ToString()));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            command = new Get();
        }

        [TestMethod]
        public void Get_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Get [Item Name] {Container}", result.ResultMessage);
        }

        [TestMethod]
        public void Get_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Get"));
        }

        [TestMethod]
        public void Get_PerformCommand_NoParameters()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to get?", result.ResultMessage);
        }

        [TestMethod]
        public void Get_PerformCommand_OneParameterGetItem()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            List<IItem> roomItems = new List<IItem>() { item.Object };
            List<IItem> mobItems = new List<IItem>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            parm1.Setup(e => e.ParameterValue).Returns("item");
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns(item.Object);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Items).Returns(roomItems);
            mob.Setup(e => e.Items).Returns(mobItems);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You pickup the SentenceDescription.", result.ResultMessage);
            Assert.IsTrue(mobItems.Contains(item.Object));
            room.Verify(e => e.RemoveItemFromRoom(item.Object));
        }


        [TestMethod]
        public void Get_PerformCommand_GetAll()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            List<IItem> roomItems = new List<IItem>() { item.Object, item2.Object };
            List<IItem> mobItems = new List<IItem>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item2.Setup(e => e.SentenceDescription).Returns("SentenceDescription2");
            item2.Setup(e => e.KeyWords).Returns(new List<string>() { "item2" });
            item2.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            parm1.Setup(e => e.ParameterValue).Returns("all");
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns(item.Object);
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item2", 0)).Returns(item2.Object);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Items).Returns(roomItems);
            mob.Setup(e => e.Items).Returns(mobItems);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
            Assert.IsTrue(mobItems.Contains(item.Object));
            room.Verify(e => e.RemoveItemFromRoom(item.Object));
            room.Verify(e => e.RemoveItemFromRoom(item2.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_OneParameterNoGet()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            List<IItem> roomItems = new List<IItem>() { item.Object };
            List<IItem> mobItems = new List<IItem>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            parm1.Setup(e => e.ParameterValue).Returns("item");
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns(item.Object);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Items).Returns(roomItems);
            mob.Setup(e => e.Items).Returns(mobItems);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to get SentenceDescription.", result.ResultMessage);
            Assert.AreEqual(0, mobItems.Count);
            Assert.IsTrue(roomItems.Contains(item.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_OneParameterNotFound()
        {
            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            parm1.Setup(e => e.ParameterValue).Returns("item");
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns<IItem>(null);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);

            List<IItem> roomItems = new List<IItem>() { item.Object };
            room.Setup(e => e.Items).Returns(roomItems);
            List<IItem> mobItems = new List<IItem>();
            mob.Setup(e => e.Items).Returns(mobItems);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find item.", result.ResultMessage);
            Assert.AreEqual(0, mobItems.Count);
            Assert.IsTrue(roomItems.Contains(item.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterGetItem_NoGet()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IMoney> money = item.As<IMoney>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IContainer> container = item2.As<IContainer>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IParameter> parm2 = new Mock<IParameter>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            List<IItem> containerItems = new List<IItem>() { item.Object };
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            money.Setup(e => e.Value).Returns(10);
            parm1.Setup(e => e.ParameterValue).Returns("item");
            parm2.Setup(e => e.ParameterValue).Returns("container");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns(item2.Object);
            container.Setup(e => e.Items).Returns(containerItems);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("coins");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to get SentenceDescription.", result.ResultMessage);
            Assert.AreEqual(1, containerItems.Count);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterGetItem()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IMoney> money = item.As<IMoney>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IContainer> container = item2.As<IContainer>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IParameter> parm2 = new Mock<IParameter>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            List<IItem> containerItems = new List<IItem>() { item.Object };
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            money.Setup(e => e.Value).Returns(10);
            parm1.Setup(e => e.ParameterValue).Returns("item");
            parm2.Setup(e => e.ParameterValue).Returns("container");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns(item2.Object);
            container.Setup(e => e.Items).Returns(containerItems);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("coins");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You get the coins from the container.", result.ResultMessage);
            Assert.AreEqual(0, containerItems.Count);
            mob.VerifySet(e => e.Money = 10);
        }

        [TestMethod]
        public void Get_PerformCommand_GetAllFromContainer()
        {
            Mock<IItem> item = new Mock<IItem>();
            Mock<IMoney> money = item.As<IMoney>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IContainer> container = item2.As<IContainer>();
            Mock<IItem> item3 = new Mock<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            Mock<IParameter> parm2 = new Mock<IParameter>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            List<IItem> containerItems = new List<IItem>() { item.Object, item3.Object };
            List<IItem> mobItems = new List<IItem>();

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            item3.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item3.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item3.Setup(e => e.KeyWords).Returns(new List<string>() { "item3" });
            money.Setup(e => e.Value).Returns(10);
            parm1.Setup(e => e.ParameterValue).Returns("all");
            parm2.Setup(e => e.ParameterValue).Returns("container");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns(item2.Object);
            container.Setup(e => e.Items).Returns(containerItems);
            mob.Setup(e => e.Items).Returns(mobItems);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
            Assert.AreEqual(0, containerItems.Count);
            mob.VerifySet(e => e.Money = 10);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterCanNotFindItemInContainer()
        {
            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            Mock<IMoney> money = item.As<IMoney>();
            money.Setup(e => e.Value).Returns(10);
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IContainer> container = item2.As<IContainer>();


            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            parm1.Setup(e => e.ParameterValue).Returns("item");
            Mock<IParameter> parm2 = new Mock<IParameter>();
            parm2.Setup(e => e.ParameterValue).Returns("container");

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns(item2.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            List<IItem> containerItems = new List<IItem>();
            container.Setup(e => e.Items).Returns(containerItems);


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find item item in container container.", result.ResultMessage);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterContainerNotFound()
        {
            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            Mock<IMoney> money = item.As<IMoney>();
            money.Setup(e => e.Value).Returns(10);
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IContainer> container = item2.As<IContainer>();


            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parm1 = new Mock<IParameter>();
            parm1.Setup(e => e.ParameterValue).Returns("item");
            Mock<IParameter> parm2 = new Mock<IParameter>();
            parm2.Setup(e => e.ParameterValue).Returns("container");

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, false)).Returns<IItem>(null);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            List<IItem> containerItems = new List<IItem>();
            container.Setup(e => e.Items).Returns(containerItems);


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find container container.", result.ResultMessage);
        }
    }
}
