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
        Mock<IRoom> room;
        Mock<IBaseObjectId> roomId;
        Mock<IPlayerCharacter> pc;
        Mock<INonPlayerCharacter> npc;
        Mock<IBaseObjectId> recallPoint;
        Mock<IRoom> altRoom;
        Mock<IZone> zone;
        Mock<IWorld> world;
        Mock<IParameter> parameter;
        Mock<IItem> item;
        Mock<IRecallBeacon> recallBeacon;
        HashSet<RoomAttribute> roomAttributes;
        List<IPlayerCharacter> pcInRoom;
        List<INonPlayerCharacter> npcInRoom;
        List<IPlayerCharacter> pcTargetRoom;
        List<INonPlayerCharacter> npcTargetRoom;


        Dictionary<int, IZone> zoneDictioanry;
        Dictionary<int, IRoom> roomDictioanry;



        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            room = new Mock<IRoom>();
            roomId = new Mock<IBaseObjectId>();
            pc = new Mock<IPlayerCharacter>();
            npc = new Mock<INonPlayerCharacter>();
            recallPoint = new Mock<IBaseObjectId>();
            altRoom = new Mock<IRoom>();
            zone = new Mock<IZone>();
            world = new Mock<IWorld>();
            parameter = new Mock<IParameter>();
            item = new Mock<IItem>();
            roomAttributes = new HashSet<RoomAttribute>();
            pcInRoom = new List<IPlayerCharacter>();
            npcInRoom = new List<INonPlayerCharacter>();
            recallBeacon = new Mock<IRecallBeacon>();
            zoneDictioanry = new Dictionary<int, IZone>();
            roomDictioanry = new Dictionary<int, IRoom>();
            pcTargetRoom = new List<IPlayerCharacter>();
            npcTargetRoom = new List<INonPlayerCharacter>();


            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            room.Setup(e => e.Attributes).Returns(roomAttributes);
            room.Setup(e => e.PlayerCharacters).Returns(pcInRoom);
            room.Setup(e => e.NonPlayerCharacters).Returns(npcInRoom);
            room.Setup(e => e.ZoneId).Returns(1);
            room.Setup(e => e.Id).Returns(2);
            mob.Setup(e => e.RecallPoint).Returns(roomId.Object);
            mob.Setup(e => e.Room).Returns(room.Object);
            world.Setup(e => e.Zones).Returns(zoneDictioanry);
            pc.Setup(e => e.RecallPoint).Returns(roomId.Object);
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            npc.Setup(e => e.RecallPoint).Returns(roomId.Object);
            npc.Setup(e => e.Room).Returns(room.Object);
            npc.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            mob.Setup(e => e.RecallPoint).Returns(recallPoint.Object);
            zone.Setup(e => e.Rooms).Returns(roomDictioanry);
            altRoom.Setup(e => e.PlayerCharacters).Returns(pcTargetRoom);
            recallPoint.Setup(e => e.Zone).Returns(1);
            recallPoint.Setup(e => e.Id).Returns(2);
            parameter.Setup(e => e.ParameterValue).Returns("set");


            pcInRoom.Add(pc.Object);
            npcInRoom.Add(npc.Object);
            roomDictioanry.Add(2, altRoom.Object);
            zoneDictioanry.Add(1, zone.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;

            command = new Recall();
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
            mob.Setup(e => e.RecallPoint).Returns<IBaseObject>(null);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("No recall point defined.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_RoomNoRecallAttribute()
        {
            roomAttributes.Add(RoomAttribute.NoRecall);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You try to recall but your body is held in place.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_PlayerCharacter()
        {
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
            recallPoint.Setup(e => e.Id).Returns(-1);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Invalid recall point defined.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_SetRecall()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { recallBeacon.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Recall point set.", result.ResultMessage);
            mob.VerifySet(e => e.RecallPoint = It.Is<IBaseObjectId>(f => f.Id == 2 && f.Zone == 1));
        }

        [TestMethod]
        public void Recall_PerformCommand_SetRecallNoBeacon()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no recall beacon here.", result.ResultMessage);
        }

        [TestMethod]
        public void Recall_PerformCommand_InvalidParameter()
        {
            mob.Setup(e => e.RecallPoint).Returns<IBaseObject>(null);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            parameter.Setup(e => e.ParameterValue).Returns("bob");
            room.Setup(e => e.Items).Returns(new List<IItem>() { recallBeacon.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("No recall point defined.", result.ResultMessage);
        }
    }
}
