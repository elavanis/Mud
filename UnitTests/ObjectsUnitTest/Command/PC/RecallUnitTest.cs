using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Room.Interface;
using Objects.Room;
using static Objects.Room.Room;
using Objects.Zone.Interface;
using Objects.World.Interface;
using Objects.Item.Items;
using Objects.Item.Interface;
using Objects.Interface;
using Objects.Item.Items.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class RecallUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Recall();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
        }

        [TestMethod]
        public void Recall_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Recall {Set}", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Recall"));
        }

        [TestMethod]
        public void Recall_PerformCommand_NoParameter()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("No recall point defined.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_RoomNoRecallAttribute()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();

            mob.Setup(e => e.RecallPoint).Returns(roomId.Object);
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>() { RoomAttribute.NoRecall });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You try to recall but your body is held in place.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_PlayerCharacter()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IBaseObjectId> recallPoint = new Mock<IBaseObjectId>();
            Mock<IRoom> altRoom = new Mock<IRoom>();
            Mock<IZone> zone = new Mock<IZone>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Dictionary<int, IZone> zoneDictioanry = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> roomDictioanry = new Dictionary<int, IRoom>();
            List<IPlayerCharacter> pcInRoom = new List<IPlayerCharacter>();
            List<IPlayerCharacter> pcTargetRoom = new List<IPlayerCharacter>();

            pc.Setup(e => e.RecallPoint).Returns(roomId.Object);
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            room.Setup(e => e.PlayerCharacters).Returns(pcInRoom);
            world.Setup(e => e.Zones).Returns(zoneDictioanry);
            zone.Setup(e => e.Rooms).Returns(roomDictioanry);
            altRoom.Setup(e => e.PlayerCharacters).Returns(pcTargetRoom);
            roomDictioanry.Add(0, altRoom.Object);
            zoneDictioanry.Add(0, zone.Object);
            pcInRoom.Add(pc.Object);

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Your body begins to shimmer and become translucent.\r\nThe surroundings begin to fade to black and then new scenery appears before you.\r\nSlowly your body becomes solid again and you can see the recall crystal in front of you.", result.ResultMessage);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            altRoom.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            pc.VerifySet(e => e.Room = altRoom.Object);
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void Recall_PerformCommand_NonPlayerCharacter()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IBaseObjectId> recallPoint = new Mock<IBaseObjectId>();
            Mock<IRoom> altRoom = new Mock<IRoom>();
            Mock<IZone> zone = new Mock<IZone>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Dictionary<int, IZone> zoneDictioanry = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> roomDictioanry = new Dictionary<int, IRoom>();
            List<INonPlayerCharacter> npcInRoom = new List<INonPlayerCharacter>();
            List<INonPlayerCharacter> npcTargetRoom = new List<INonPlayerCharacter>();

            npc.Setup(e => e.RecallPoint).Returns(roomId.Object);
            npc.Setup(e => e.Room).Returns(room.Object);
            npc.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            room.Setup(e => e.NonPlayerCharacters).Returns(npcInRoom);
            world.Setup(e => e.Zones).Returns(zoneDictioanry);
            zone.Setup(e => e.Rooms).Returns(roomDictioanry);
            altRoom.Setup(e => e.NonPlayerCharacters).Returns(npcTargetRoom);
            roomDictioanry.Add(0, altRoom.Object);
            zoneDictioanry.Add(0, zone.Object);
            npcInRoom.Add(npc.Object);

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(npc.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Your body begins to shimmer and become translucent.\r\nThe surroundings begin to fade to black and then new scenery appears before you.\r\nSlowly your body becomes solid again and you can see the recall crystal in front of you.", result.ResultMessage);
            room.Verify(e => e.RemoveMobileObjectFromRoom(npc.Object));
            altRoom.Verify(e => e.AddMobileObjectToRoom(npc.Object));
            npc.VerifySet(e => e.Room = altRoom.Object);
            npc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void Recall_PerformCommand_InvalidRecalPoint()
        {
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IBaseObjectId> recallPoint = new Mock<IBaseObjectId>();

            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            mob.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            mob.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.World = null; //was getting issues when run with other tests that this was populated

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Invalid recall point defined.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_SetRecall()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            IRecallBeacon recall = new RecallBeacon();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            parameter.Setup(e => e.ParameterValue).Returns("set");
            room.Setup(e => e.Items).Returns(new List<IItem>() { recall });
            mob.Setup(e => e.Room).Returns(room.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Recall point set.", result.ResultMessage);
            mob.VerifySet(e => e.RecallPoint = It.IsAny<IBaseObjectId>());
        }

        [TestMethod]
        public void Recall_PerformCommand_SetRecallNoBeacon()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IItem> item = new Mock<IItem>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            parameter.Setup(e => e.ParameterValue).Returns("set");
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no recall beacon here.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_InvalidParameter()
        {
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IItem> item = new Mock<IItem>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            parameter.Setup(e => e.ParameterValue).Returns("bob");
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("No recall point defined.", result.ResultMessage);
        }
    }
}
