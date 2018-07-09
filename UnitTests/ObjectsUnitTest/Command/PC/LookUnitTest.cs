using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Shared.TagWrapper.Interface;
using Moq;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.Commands.Interface;
using Objects.Room.Interface;
using Objects.Global.CanMobDoSomething.Interface;
using static Objects.Mob.MobileObject;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class LookUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> mob;
        Mock<ICommand> mockCommand;

        Mock<IRoom> room;
        Mock<ICanMobDoSomething> canDoSomething;
        Mock<IEquipment> item;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("(L)ook {Object To Look At}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IPlayerCharacter>();
            mockCommand = new Mock<ICommand>();

            room = new Mock<IRoom>();
            canDoSomething = new Mock<ICanMobDoSomething>();
            item = new Mock<IEquipment>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Debug).Returns(true);
            item.Setup(e => e.ShortDescription).Returns("ItemShortDescription");
            npc.Setup(e => e.ShortDescription).Returns("NpcShortDescription");
            pc.Setup(e => e.ShortDescription).Returns("PcShortDescription");
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { mob.Object, pc.Object });
            room.Setup(e => e.ShortDescription).Returns("RoomShortDescription");
            room.Setup(e => e.LookDescription).Returns("RoomLongDescription");
            canDoSomething.Setup(e => e.SeeDueToLight(mob.Object)).Returns(true);
            canDoSomething.Setup(e => e.SeeObject(mob.Object, item.Object)).Returns(true);
            canDoSomething.Setup(e => e.SeeObject(mob.Object, npc.Object)).Returns(true);
            canDoSomething.Setup(e => e.SeeObject(mob.Object, pc.Object)).Returns(true);

            GlobalReference.GlobalValues.CanMobDoSomething = canDoSomething.Object;

            command = new Look();
        }

        [TestMethod]
        public void Look_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("L"));
            Assert.IsTrue(result.Contains("Look"));
        }

        [TestMethod]
        public void Look_PerformCommand_Sleep()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Sleep);
            tagWrapper.Setup(e => e.WrapInTag("You can not look while asleep.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_Dark()
        {
            canDoSomething.Setup(e => e.SeeDueToLight(mob.Object)).Returns(false);
            tagWrapper.Setup(e => e.WrapInTag("You can not see here because it is to dark.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookAtRoom()
        {
            Mock<IExit> exit = new Mock<IExit>();

            tagWrapper.Setup(e => e.WrapInTag("[Zone0 - Room0]\r\n[RoomShortDescription]\r\nRoomLongDescription\r\nExits:N E S W U D", TagType.Room)).Returns("Room");
            tagWrapper.Setup(e => e.WrapInTag("ItemShortDescription", TagType.Item)).Returns("Item");
            tagWrapper.Setup(e => e.WrapInTag("NpcShortDescription", TagType.NonPlayerCharacter)).Returns("NonPlayerCharacter");
            tagWrapper.Setup(e => e.WrapInTag("PcShortDescription", TagType.PlayerCharacter)).Returns("PlayerCharacter");
            tagWrapper.Setup(e => e.WrapInTag("Room\r\nItem\r\nNonPlayerCharacter\r\nPlayerCharacter", TagType.Info)).Returns("message");

            room.Setup(e => e.Up).Returns(exit.Object);
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.South).Returns(exit.Object);
            room.Setup(e => e.West).Returns(exit.Object);
            room.Setup(e => e.Down).Returns(exit.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("Room\r\nItem\r\nNonPlayerCharacter\r\nPlayerCharacter", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookPC()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("PcLongDescription", TagType.PlayerCharacter)).Returns("PcLongDescription");
            tagWrapper.Setup(e => e.WrapInTag("ItemShortDescription", TagType.Item)).Returns("ItemShortDescription");
            tagWrapper.Setup(e => e.WrapInTag("PcLongDescription\r\nItemShortDescription", TagType.Info)).Returns("message");

            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("pc");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "pc", 0, true, true, true, true, true)).Returns(pc.Object);
            pc.Setup(e => e.LookDescription).Returns("PcLongDescription");
            pc.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { item.Object });

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookNPC()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("NpcLongDescription", TagType.NonPlayerCharacter)).Returns("NpcLongDescription");
            tagWrapper.Setup(e => e.WrapInTag("ItemShortDescription", TagType.Item)).Returns("ItemShortDescription");
            tagWrapper.Setup(e => e.WrapInTag("NpcLongDescription\r\nItemShortDescription", TagType.Info)).Returns("message");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("npc");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "npc", 0, true, true, true, true, true)).Returns(npc.Object);
            npc.Setup(e => e.LookDescription).Returns("NpcLongDescription");
            npc.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { item.Object });

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookItem()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("LongDescription", TagType.Item)).Returns("LongDescription");
            tagWrapper.Setup(e => e.WrapInTag("LongDescription", TagType.Info)).Returns("message");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, true, true, true)).Returns(item.Object);
            item.Setup(e => e.LookDescription).Returns("LongDescription");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookContainer()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IContainer> container = new Mock<IContainer>();
            Mock<IItem> containerItem = container.As<IItem>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("ContainerLongDescription\r\nItemShortDescription", TagType.Item)).Returns("message");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("container");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            container.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            containerItem.Setup(e => e.LookDescription).Returns("ContainerLongDescription");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, true, true, true)).Returns(containerItem.Object);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_LookContainerEmpty()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IContainer> container = new Mock<IContainer>();
            Mock<IItem> containerItem = container.As<IItem>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("LongDescription\r\n<Empty>", TagType.Item)).Returns("message");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("container");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            container.Setup(e => e.Items).Returns(new List<IItem>());
            containerItem.Setup(e => e.LookDescription).Returns("LongDescription");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "container", 0, true, true, true, true, true)).Returns(containerItem.Object);
            item.Setup(e => e.LookDescription).Returns("LongDescription");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Look_PerformCommand_NothingFound()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            tagWrapper.Setup(e => e.WrapInTag("Unable to find anything that matches that description.", TagType.Info)).Returns("message");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            parameter.Setup(e => e.ParameterValue).Returns("nothing");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "nothing", 0, true, true, true, true, false)).Returns<IBaseObject>(null);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
