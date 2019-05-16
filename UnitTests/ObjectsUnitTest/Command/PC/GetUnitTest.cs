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
        Mock<ICommand> mockCommand;
        Mock<IMobileObject> mob;
        Mock<IRoom> room;
        Mock<IItem> item;
        Mock<IItem> itemMoney;
        Mock<IMoney> money;
        Mock<IItem> itemContainer;
        Mock<IContainer> container;
        Mock<IParameter> parm1;
        Mock<IParameter> parm2;
        Mock<IFindObjects> findObjects;
        Mock<IItem> item2;
        Mock<IItem> item3;


        List<IItem> mobItems;
        List<IItem> roomItems;
        List<IItem> containerItems;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            engine = new Mock<IEngine>();
            mockEvent = new Mock<IEvent>();
            notify = new Mock<INotify>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            mockCommand = new Mock<ICommand>();
            mob = new Mock<IMobileObject>();
            room = new Mock<IRoom>();
            item = new Mock<IItem>();
            itemMoney = new Mock<IItem>();
            money = itemMoney.As<IMoney>();
            itemContainer = new Mock<IItem>();
            container = itemContainer.As<IContainer>();
            parm1 = new Mock<IParameter>();
            parm2 = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            item2 = new Mock<IItem>();
            item3 = new Mock<IItem>();

            mobItems = new List<IItem>();
            roomItems = new List<IItem>() { item.Object };
            containerItems = new List<IItem>() { item.Object };
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            engine.Setup(e => e.Event).Returns(mockEvent.Object);
            moneyToCoins.Setup(e => e.FormatedAsCoins(It.IsAny<ulong>())).Returns((ulong x) => (x.ToString()));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object });
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(mobItems);
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            itemMoney.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });
            itemMoney.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            parm1.Setup(e => e.ParameterValue).Returns("item");
            parm2.Setup(e => e.ParameterValue).Returns("container");
            item2.Setup(e => e.SentenceDescription).Returns("SentenceDescription2");
            item2.Setup(e => e.KeyWords).Returns(new List<string>() { "item2" });
            item2.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns(item.Object);
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item2", 0)).Returns(item2.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns(itemContainer.Object);
            container.Setup(e => e.Items).Returns(containerItems);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("coins");
            item3.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item3.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            item3.Setup(e => e.KeyWords).Returns(new List<string>() { "item3" });

            room.Setup(e => e.Items).Returns(roomItems);
            money.Setup(e => e.Value).Returns(10);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
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
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to get?", result.ResultMessage);
        }

        [TestMethod]
        public void Get_PerformCommand_OneParameterGetItem()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You pickup the SentenceDescription.", result.ResultMessage);
            Assert.IsTrue(mobItems.Contains(item.Object));
            room.Verify(e => e.RemoveItemFromRoom(item.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_GetAll()
        {
            roomItems.Add(item2.Object);
            parm1.Setup(e => e.ParameterValue).Returns("all");

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
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to get SentenceDescription.", result.ResultMessage);
            Assert.AreEqual(0, mobItems.Count);
            Assert.IsTrue(roomItems.Contains(item.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_OneParameterNotFound()
        {
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "item", 0)).Returns<IItem>(null);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find item.", result.ResultMessage);
            Assert.AreEqual(0, mobItems.Count);
            Assert.IsTrue(roomItems.Contains(item.Object));
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterGetItem_NoGet()
        {
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to get SentenceDescription.", result.ResultMessage);
            Assert.AreEqual(1, containerItems.Count);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterGetItem()
        {
            containerItems.Clear();
            containerItems.Add(itemMoney.Object);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You get the coins from the container.", result.ResultMessage);
            Assert.AreEqual(0, containerItems.Count);
            mob.VerifySet(e => e.Money = 10);
        }

        [TestMethod]
        public void Get_PerformCommand_GetAllFromContainer()
        {
            containerItems.Clear();
            containerItems.Add(itemMoney.Object);
            containerItems.Add(item3.Object);
            parm1.Setup(e => e.ParameterValue).Returns("all");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
            Assert.AreEqual(0, containerItems.Count);
            mob.VerifySet(e => e.Money = 10);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterCanNotFindItemInContainer()
        {
            roomItems.Add(itemContainer.Object);
            containerItems.Clear();
            containerItems.Add(item2.Object);
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find item item in container container.", result.ResultMessage);
        }

        [TestMethod]
        public void Get_PerformCommand_TwoParameterContainerNotFound()
        {
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.NoGet });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm1.Object, parm2.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, false, false, true)).Returns<IItem>(null);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find container container.", result.ResultMessage);
        }

        [TestMethod]
        public void Get_PerformCommand_OnlyAllowGetCorpserThatYouKilled()
        {
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void Get_PerformCommand_OnlyAllowLootCorpserThatYouKilled()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
