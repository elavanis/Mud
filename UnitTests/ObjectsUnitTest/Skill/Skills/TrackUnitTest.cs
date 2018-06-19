using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Skill.Skills;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Objects.Mob.MobileObject;
using static Objects.Room.Room;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills
{
    [TestClass]
    public class TrackUnitTest
    {
        Track track;
        Mock<IMobileObject> performer;
        Mock<ICommand> command;
        List<IParameter> parameters;
        Mock<ITagWrapper> tagWrapper;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<IRoom> room3;
        Mock<IFindObjects> findObjects;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IZone> zone;
        Mock<IZone> zone2;
        Dictionary<int, IRoom> rooms;
        Dictionary<int, IRoom> rooms2;
        Dictionary<int, IZone> zones;
        Mock<IWorld> world;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            track = new Track();
            performer = new Mock<IMobileObject>();
            command = new Mock<ICommand>();
            parameters = new List<IParameter>();
            Mock<IParameter> param = new Mock<IParameter>();
            parameters.Add(param.Object);
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            room3 = new Mock<IRoom>();
            findObjects = new Mock<IFindObjects>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            zone = new Mock<IZone>();
            zone2 = new Mock<IZone>();
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IExit> exit2 = new Mock<IExit>();
            rooms = new Dictionary<int, IRoom>();
            rooms2 = new Dictionary<int, IRoom>();
            zones = new Dictionary<int, IZone>();
            world = new Mock<IWorld>();

            command.Setup(e => e.CommandName).Returns("Perform");
            command.Setup(e => e.Parameters).Returns(parameters);
            param.Setup(e => e.ParameterValue).Returns("Track");
            performer.Setup(e => e.Position).Returns(CharacterPosition.Stand);
            performer.Setup(e => e.Room).Returns(room.Object);
            exit.Setup(e => e.Room).Returns(2);
            exit.Setup(e => e.Zone).Returns(1);
            exit2.Setup(e => e.Room).Returns(3);
            exit2.Setup(e => e.Zone).Returns(2);
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.West).Returns(exit2.Object);
            room2.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            room3.Setup(e => e.Zone).Returns(2);
            room3.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            zone.Setup(e => e.Rooms).Returns(rooms);
            zone2.Setup(e => e.Rooms).Returns(rooms2);
            world.Setup(e => e.Zones).Returns(zones);
            zones.Add(1, zone.Object);
            zones.Add(2, zone2.Object);
            rooms.Add(1, room.Object);
            rooms.Add(2, room2.Object);
            rooms2.Add(3, room3.Object);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.World = world.Object;
        }

        [TestMethod]
        public void Track_TeachMessage()
        {
            string expected = "A good tracker needs to be able to search for their target using any means possible.";
            Assert.AreEqual(expected, track.TeachMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_Asleep()
        {
            performer.Setup(e => e.Position).Returns(CharacterPosition.Sleep);
            command.Setup(e => e.Parameters).Returns(parameters);
            tagWrapper.Setup(e => e.WrapInTag("You can not track while asleep.", TagType.Info)).Returns("expected message");

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_NotEnoughParameters()
        {
            command.Setup(e => e.Parameters).Returns(parameters);
            tagWrapper.Setup(e => e.WrapInTag("What are you trying to track?", TagType.Info)).Returns("expected message");

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_NpcInRoom()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            param.Setup(e => e.ParameterValue).Returns("target");
            parameters.Add(param.Object);

            tagWrapper.Setup(e => e.WrapInTag("You look up and see the target in front of you.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_PcInRoom()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            param.Setup(e => e.ParameterValue).Returns("target");
            parameters.Add(param.Object);

            tagWrapper.Setup(e => e.WrapInTag("You look up and see the target in front of you.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>() { pc.Object });

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_NpcInNextRoom()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            param.Setup(e => e.ParameterValue).Returns("target");
            parameters.Add(param.Object);

            tagWrapper.Setup(e => e.WrapInTag("You pickup the trail of a target to the East.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room2.Object, "target")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_NpcNotFound()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            param.Setup(e => e.ParameterValue).Returns("target");
            parameters.Add(param.Object);

            tagWrapper.Setup(e => e.WrapInTag("You were unable to pick up a trail to a target.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room2.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room2.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room3.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room3.Object, "target")).Returns(new List<IPlayerCharacter>());

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_NoTrackRoom()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            param.Setup(e => e.ParameterValue).Returns("target");
            parameters.Add(param.Object);

            tagWrapper.Setup(e => e.WrapInTag("You were unable to pick up a trail to a target.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>());
            findObjects.Setup(e => e.FindNpcInRoom(room2.Object, "target")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room2.Setup(e => e.Attributes).Returns(new List<RoomAttribute>() { RoomAttribute.NoTrack });
            findObjects.Setup(e => e.FindNpcInRoom(room3.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room3.Object, "target")).Returns(new List<IPlayerCharacter>());

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
        }

        [TestMethod]
        public void Track_ProcessSkill_DontCheckRoomTwice()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            Mock<IExit> exit = new Mock<IExit>();

            param.Setup(e => e.ParameterValue).Returns("target");
            exit.Setup(e => e.Room).Returns(2);
            exit.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.West).Returns(exit.Object);

            parameters.Add(param.Object);


            tagWrapper.Setup(e => e.WrapInTag("You were unable to pick up a trail to a target.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room2.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room2.Object, "target")).Returns(new List<IPlayerCharacter>());

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
            room2.Verify(e => e.Attributes, Times.Once);
        }

        [TestMethod]
        public void Track_ProcessSkill_AnotherFindNpc()
        {
            Mock<IParameter> param = new Mock<IParameter>();
            Mock<IExit> exit = new Mock<IExit>();

            param.Setup(e => e.ParameterValue).Returns("target");
            exit.Setup(e => e.Room).Returns(3);
            exit.Setup(e => e.Zone).Returns(1);
            room2.Setup(e => e.East).Returns(exit.Object);
            rooms.Add(3, room3.Object);
            room3.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());

            tagWrapper.Setup(e => e.WrapInTag("You pickup the trail of a target to the West.", TagType.Info)).Returns("expected message");
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room2.Object, "target")).Returns(new List<INonPlayerCharacter>());
            findObjects.Setup(e => e.FindPcInRoom(room2.Object, "target")).Returns(new List<IPlayerCharacter>());

            findObjects.Setup(e => e.FindNpcInRoom(room3.Object, "target")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            parameters.Add(param.Object);

            IResult result = track.ProcessSkill(performer.Object, command.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("expected message", result.ResultMessage);
            room2.Verify(e => e.Attributes, Times.Once);
        }
    }
}
