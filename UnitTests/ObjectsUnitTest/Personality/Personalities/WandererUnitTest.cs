using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality.Personalities;
using Objects.Mob.Interface;
using Moq;
using Objects.Global.Random.Interface;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Command.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;
using Objects.World.Interface;
using Objects.Interface;
using Objects.Personality.Personalities.Interface;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class WandererUnitTest
    {
        IWanderer wander;
        Mock<INonPlayerCharacter> npc;
        Mock<IRandom> random;
        Mock<INonPlayerCharacter> followTarget;
        Mock<IRoom> room;
        Mock<IRoom> north;
        Mock<IRoom> east;
        Mock<IRoom> south;
        Mock<IRoom> west;
        Mock<IRoom> up;
        Mock<IRoom> down;
        Mock<IExit> exitNorth;
        Mock<IExit> exitEast;
        Mock<IExit> exitSouth;
        Mock<IExit> exitWest;
        Mock<IExit> exitUp;
        Mock<IExit> exitDown;
        Mock<IWorld> world;
        Mock<IZone> zone;

        Dictionary<int, IZone> zones;
        Dictionary<int, IRoom> rooms;

        [TestInitialize]
        public void Setup()
        {
            wander = new Wanderer();
            npc = new Mock<INonPlayerCharacter>();
            random = new Mock<IRandom>();
            followTarget = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            north = new Mock<IRoom>();
            east = new Mock<IRoom>();
            south = new Mock<IRoom>();
            west = new Mock<IRoom>();
            up = new Mock<IRoom>();
            down = new Mock<IRoom>();
            exitNorth = new Mock<IExit>();
            exitEast = new Mock<IExit>();
            exitSouth = new Mock<IExit>();
            exitWest = new Mock<IExit>();
            exitUp = new Mock<IExit>();
            exitDown = new Mock<IExit>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();

            zones = new Dictionary<int, IZone>();
            rooms = new Dictionary<int, IRoom>();

            random.Setup(e => e.PercentDiceRoll(1)).Returns(true);
            npc.Setup(e => e.Room).Returns(room.Object);
            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);
            exitNorth.Setup(e => e.Room).Returns(1);
            exitEast.Setup(e => e.Room).Returns(2);
            exitSouth.Setup(e => e.Room).Returns(3);
            exitWest.Setup(e => e.Room).Returns(4);
            exitUp.Setup(e => e.Room).Returns(5);
            exitDown.Setup(e => e.Room).Returns(6);
            exitNorth.Setup(e => e.Zone).Returns(0);
            exitEast.Setup(e => e.Zone).Returns(0);
            exitSouth.Setup(e => e.Zone).Returns(0);
            exitWest.Setup(e => e.Zone).Returns(0);
            exitUp.Setup(e => e.Zone).Returns(0);
            exitDown.Setup(e => e.Zone).Returns(0);

            zones.Add(0, zone.Object);
            rooms.Add(1, north.Object);
            rooms.Add(2, east.Object);
            rooms.Add(3, south.Object);
            rooms.Add(4, west.Object);
            rooms.Add(5, up.Object);
            rooms.Add(6, down.Object);





            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.World = world.Object;

        }


        [TestMethod]
        public void Wanderer_Constructor()
        {
            Wanderer wanderer = new Wanderer(100);

            Assert.AreEqual(100, wanderer.MovementPercent);
        }

        [TestMethod]
        public void Wanderer_Process_CommandNotNull()
        {
            string command = "command";

            Assert.AreSame(command, wander.Process(npc.Object, command));
        }

        [TestMethod]
        public void Wanderer_Process_DoesNotTriggerMovement()
        {
            wander.MovementPercent = 2;

            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_FollowDeadMob()
        {
            npc.Setup(e => e.FollowTarget).Returns(followTarget.Object);
            followTarget.Setup(e => e.Health).Returns(-1);

            Assert.IsNull(wander.Process(npc.Object, null));
            npc.VerifySet(e => e.FollowTarget = null, Times.Once);
        }

        [TestMethod]
        public void Wanderer_Process_FollowAliveMob()
        {
            npc.Setup(e => e.FollowTarget).Returns(followTarget.Object);
            followTarget.Setup(e => e.Health).Returns(1);

            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_CanNotLeave()
        {
            room.Setup(e => e.CheckLeave(npc.Object)).Returns(new Mock<IResult>().Object);
            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_NoValidExits()
        {
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_North()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);

            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("N", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_East()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);
            room.Setup(e => e.East).Returns(exitEast.Object);

            random.Setup(e => e.Next(2)).Returns(1);
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("E", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_South()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);
            room.Setup(e => e.East).Returns(exitEast.Object);
            room.Setup(e => e.South).Returns(exitSouth.Object);

            random.Setup(e => e.Next(3)).Returns(2);
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("S", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_West()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);
            room.Setup(e => e.East).Returns(exitEast.Object);
            room.Setup(e => e.South).Returns(exitSouth.Object);
            room.Setup(e => e.West).Returns(exitWest.Object);

            random.Setup(e => e.Next(4)).Returns(3);
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("W", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_Up()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);
            room.Setup(e => e.East).Returns(exitEast.Object);
            room.Setup(e => e.South).Returns(exitSouth.Object);
            room.Setup(e => e.West).Returns(exitWest.Object);
            room.Setup(e => e.Up).Returns(exitUp.Object);

            random.Setup(e => e.Next(5)).Returns(4);
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("U", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_Down()
        {
            room.Setup(e => e.North).Returns(exitNorth.Object);
            room.Setup(e => e.East).Returns(exitEast.Object);
            room.Setup(e => e.South).Returns(exitSouth.Object);
            room.Setup(e => e.West).Returns(exitWest.Object);
            room.Setup(e => e.Up).Returns(exitUp.Object);
            room.Setup(e => e.Down).Returns(exitDown.Object);

            random.Setup(e => e.Next(6)).Returns(5);
            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("D", wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_CanNotEnter()
        {
            north.Setup(e => e.CheckEnter(npc.Object)).Returns(new Mock<IResult>().Object);
            room.Setup(e => e.North).Returns(exitNorth.Object);

            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_NavigableRoomsDoesNotContainsRoom()
        {
            Mock<IBaseObjectId> roomdId = new Mock<IBaseObjectId>();
            roomdId.Setup(e => e.Id).Returns(1);
            roomdId.Setup(e => e.Zone).Returns(0);
            wander.NavigableRooms.Add(roomdId.Object);

            room.Setup(e => e.North).Returns(exitNorth.Object);

            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.IsNull(wander.Process(npc.Object, null));
        }

        [TestMethod]
        public void Wanderer_Process_NavigableRoomsContainsRoom()
        {
            Mock<IBaseObjectId> roomdId = new Mock<IBaseObjectId>();
            roomdId.Setup(e => e.Id).Returns(0);
            roomdId.Setup(e => e.Zone).Returns(0);
            wander.NavigableRooms.Add(roomdId.Object);

            room.Setup(e => e.North).Returns(exitNorth.Object);

            room.Setup(e => e.CheckLeave(npc.Object)).Returns<IResult>(null);
            Assert.AreEqual("N", wander.Process(npc.Object, null));
        }
    }
}
