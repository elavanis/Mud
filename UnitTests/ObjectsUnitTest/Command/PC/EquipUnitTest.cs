using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using System.Collections.Generic;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;
using Objects.Mob.Interface;
using Objects.Item.Items.Interface;
using static Objects.Item.Items.Equipment;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class EquipUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Equip();
        }

        [TestMethod]
        public void Equip_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Equip [Item Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Equip"));
        }

        [TestMethod]
        public void Equip_PerformCommand_NoParameter()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.ShortDescription).Returns("ShortDescription");
            equipment.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Finger);

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equipment.Object, equipment.Object });
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Wield <Nothing>\r\nHead <Nothing>\r\nNeck <Nothing>\r\nArms <Nothing>\r\nHand <Nothing>\r\nFinger ShortDescription\r\nBody <Nothing>\r\nWaist <Nothing>\r\nLegs <Nothing>\r\nFeet <Nothing>\r\nHeld <Nothing>", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotFound()
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

            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns<IItem>(null);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find parm.", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotEquipment()
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
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not equip the SentenceDescription.", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotWorn()
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

            Mock<IEquipment> item = new Mock<IEquipment>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.NotWorn);
            items.Add(item.Object);
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns(item.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;


            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not equip the SentenceDescription.", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_AnotherItemAlreadyEquiped()
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

            Mock<IEquipment> item = new Mock<IEquipment>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Head);
            items.Add(item.Object);
            Mock<IEquipment> alreadyEquipedItem = new Mock<IEquipment>();
            alreadyEquipedItem.Setup(e => e.SentenceDescription).Returns("AlreadyEquipedItemSentenceDescription");
            alreadyEquipedItem.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Head);
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { alreadyEquipedItem.Object });
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns(item.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You already have AlreadyEquipedItemSentenceDescription in the Head position.", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemEquiped()
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

            Mock<IEquipment> item = new Mock<IEquipment>();
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Head);
            items.Add(item.Object);
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "parm", 0)).Returns(item.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You equipped the SentenceDescription.", result.ResultMessage);
            Assert.AreEqual(0, items.Count);
            mob.Verify(e => e.AddEquipment(item.Object), Times.Once);
        }
    }
}
