using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.GameDateTime.Interface;
using Moq;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using Objects.Item.Items.Interface;
using Objects.Global.Random.Interface;
using Objects.Personality.Interface;
using Objects.GameDateTime.Interface;

namespace ObjectsUnitTest.Zone
{
    [TestClass]
    public class ZoneUnitTest
    {
        Objects.Zone.Zone zone;
        Mock<IRoom> room;
        Mock<IItem> item;
        Mock<INonPlayerCharacter> npc;
        Mock<IMobileObject> otherMob;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;
        Mock<IRoom> room1;
        Mock<IRoom> room2;
        Mock<IExit> exit1;
        Mock<IExit> exit2;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            zone = new Objects.Zone.Zone();
            room = new Mock<IRoom>();
            item = new Mock<IItem>();
            npc = new Mock<INonPlayerCharacter>();
            otherMob = new Mock<IMobileObject>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();
            room1 = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            exit1 = new Mock<IExit>();
            exit2 = new Mock<IExit>();

            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.OtherMobs).Returns(new List<IMobileObject>() { otherMob.Object });
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.AddDays(1)).Returns(gameDateTime.Object);
            room1.Setup(e => e.East).Returns(exit1.Object);
            room1.Setup(e => e.Id).Returns(1);
            room2.Setup(e => e.West).Returns(exit2.Object);
            room2.Setup(e => e.Id).Returns(2);
            exit1.Setup(e => e.Zone).Returns(1);
            exit1.Setup(e => e.Room).Returns(2);
            exit2.Setup(e => e.Zone).Returns(1);
            exit2.Setup(e => e.Room).Returns(1);

            zone.Rooms.Add(1, room.Object);
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
        }


        [TestMethod]
        public void Zone_FinishLoad_NoZoneSync()
        {
            zone.FinishLoad();

            Assert.IsNotNull(zone.ResetTime);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
            otherMob.Verify(e => e.FinishLoad(-1), Times.Once);
            item.Verify(e => e.FinishLoad(-1), Times.Once);
            room.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void Zone_FinishLoad_RecursiveLoad()
        {
            Mock<IItem> item1 = new Mock<IItem>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IItem> item3 = new Mock<IItem>();
            Mock<IContainer> container1 = item1.As<IContainer>();
            Mock<IContainer> container2 = item2.As<IContainer>();
            Mock<IContainer> container3 = item3.As<IContainer>();
            Mock<IGameDateTime> gameDateTime = new Mock<IGameDateTime>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            container3.Setup(e => e.Items).Returns(new List<IItem>());
            container2.Setup(e => e.Items).Returns(new List<IItem>() { item3.Object });
            container1.Setup(e => e.Items).Returns(new List<IItem>() { item2.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { item1.Object });

            zone.FinishLoad();

            item1.Verify(e => e.FinishLoad(-1), Times.Once);
            item2.Verify(e => e.FinishLoad(-1), Times.Once);
            item3.Verify(e => e.FinishLoad(-1), Times.Once);
            item1.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
            item2.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
            item3.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
        }

        [TestMethod]
        public void Zone_FinishLoad_ZoneSyncOption()
        {
            Mock<IGameDateTime> gameDateTime = new Mock<IGameDateTime>();
            Mock<IRandom> random = new Mock<IRandom>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            zone.ZoneObjectSyncOptions = 1;
            random.Setup(e => e.Next(1)).Returns(0);

            GlobalReference.GlobalValues.Random = random.Object;

            zone.FinishLoad(0);

            room.Verify(e => e.FinishLoad(0), Times.Once);
        }

        [TestMethod]
        public void Zone_ToCsFile()
        {
            string expected =
@"using GenerateZones;
using GenerateZones.Zones;
using MiscShared;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;

namespace GeneratedZones
{
    public class GeneratedZone : BaseZone, IZoneCode
    {
        public GeneratedZone() : base(1)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GeneratedZone);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.LookDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.ShortDescription = ""Underground cavern"";

            return room;
        }
        private IRoom GenerateRoom2()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.LookDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.ShortDescription = ""Underground cavern"";

            return room;
        }
        #endregion Rooms

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
        }
    }
}";


            zone.Rooms.Clear();
            zone.Rooms.Add(1, room1.Object);
            zone.Rooms.Add(2, room2.Object);

            string result = zone.ToCsFile(1);

            Assert.AreEqual(result, expected);
        }
    }
}