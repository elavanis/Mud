using System;
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
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Equip [Item Name]", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Equip();
        }

        [TestMethod]
        public void Equip_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
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

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Wield <Nothing>\r\nHead <Nothing>\r\nNeck <Nothing>\r\nArms <Nothing>\r\nHand <Nothing>\r\nFinger ShortDescription\r\nBody <Nothing>\r\nWaist <Nothing>\r\nLegs <Nothing>\r\nFeet <Nothing>\r\nHeld <Nothing>", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotFound()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You were unable to find parm.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotEquipment()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You can not equip the SentenceDescription.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemNotWorn()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You can not equip the SentenceDescription.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_AnotherItemAlreadyEquiped()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You already have AlreadyEquipedItemSentenceDescription in the Head position.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Equip_PerformCommand_ItemEquiped()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You equipped the SentenceDescription.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
            Assert.AreEqual(0, items.Count);
            mob.Verify(e => e.AddEquipment(item.Object), Times.Once);
        }
    }
}
