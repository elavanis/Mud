using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Objects.Mob;
using Objects.Item.Interface;
using static Objects.Room.Room;
using Objects.Mob.Interface;
using Moq;
using Objects.Global;
using Objects.Global.Engine.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Zone.Interface;
using Objects.World.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Command.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Interface;
using Objects.LoadPercentage.Interface;
using System.Linq;
using Shared.FileIO.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using System.Reflection;

namespace ObjectsUnitTest.Room
{
    [TestClass]
    public class RoomUnitTest
    {
        Objects.Room.Room room;
        Mock<IFileIO> fileIO;
        Mock<IItem> item;
        Mock<ISerialization> serializer;
        Mock<ISettings> settings;

        [TestInitialize]
        public void Setup()
        {
            fileIO = new Mock<IFileIO>();
            item = new Mock<IItem>();
            serializer = new Mock<ISerialization>();
            settings = new Mock<ISettings>();

            settings.Setup(e => e.VaultDirectory).Returns("vault");
            serializer.Setup(e => e.Serialize(It.IsAny<List<IItem>>())).Returns("serializedList");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            room = new Objects.Room.Room();
            room.Zone = 1;
            room.Id = 2;
        }

        [TestMethod]
        public void Room_AddItemToRoom_ValutContentsWritten()
        {
            room.Attributes.Add(RoomAttribute.Vault);

            room.AddItemToRoom(item.Object);

            fileIO.Verify(e => e.WriteFile("vault\\1-2.vault", "serializedList"), Times.Once);
        }

        [TestMethod]
        public void Room_RemoveItemFromRoom_ValutContentsWritten()
        {
            FieldInfo fieldInfo = room.GetType().GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            List<IItem> items = (List<IItem>)fieldInfo.GetValue(room);
            items.Add(item.Object);
            items.Add(item.Object);

            room.Attributes.Add(RoomAttribute.Vault);

            room.RemoveItemFromRoom(item.Object);

            fileIO.Verify(e => e.WriteFile("vault\\1-2.vault", "serializedList"), Times.Once);
        }

        [TestMethod]
        public void Room_ToString()
        {
            string result = room.ToString();

            Assert.AreEqual("1-0", result);
        }

        [TestMethod]
        public void Room_CheckEnter_NoNpcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);
            Mock<INonPlayerCharacter> mob = new Mock<INonPlayerCharacter>();

            Assert.IsNull(room.CheckEnter(mob.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_NpcNotSmall()
        {
            Mock<INonPlayerCharacter> mob = new Mock<INonPlayerCharacter>();

            room.AddMobileObjectToRoom(new Mock<INonPlayerCharacter>().Object);

            Assert.IsNull(room.CheckEnter(mob.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_NpcSmall()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("The room is to small to fit another person in there.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            room.Attributes.Add(RoomAttribute.Small);
            room.AddMobileObjectToRoom(new Mock<INonPlayerCharacter>().Object);
            Mock<INonPlayerCharacter> mob = new Mock<INonPlayerCharacter>();

            IResult result = room.CheckEnter(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NoNpc()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Non player characters can not enter here.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            room.Attributes.Add(RoomAttribute.NoNPC);
            Mock<INonPlayerCharacter> mob = new Mock<INonPlayerCharacter>();

            IResult result = room.CheckEnter(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NoPcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);
            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();

            Assert.IsNull(room.CheckEnter(mob.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_PcNotSmall()
        {
            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();

            room.AddMobileObjectToRoom(new Mock<IPlayerCharacter>().Object);

            Assert.IsNull(room.CheckEnter(mob.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_PcSmall()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("The room is to small to fit another person in there.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            room.Attributes.Add(RoomAttribute.Small);
            room.AddMobileObjectToRoom(new Mock<IPlayerCharacter>().Object);
            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();

            IResult result = room.CheckEnter(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NotOwner()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("That property belongs to NotYou and you are not on the guest list.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            room.Owner = "NotYou";

            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });

            IResult result = room.CheckEnter(mob.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_Owner()
        {
            room.Owner = "NotYou";

            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "NotYou" });

            IResult result = room.CheckEnter(mob.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Room_CheckEnter_NotGuest()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("That property belongs to NotYou and you are not on the guest list.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            room.Owner = "NotYou";
            room.Guests.Add("AlsoNotYou");

            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });

            IResult result = room.CheckEnter(mob.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_Guest()
        {
            room.Owner = "NotYou";
            room.Guests.Add("PC");

            Mock<IPlayerCharacter> mob = new Mock<IPlayerCharacter>();
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });

            IResult result = room.CheckEnter(mob.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Room_CheckLeave_Pass()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.IsInCombat).Returns(false);

            Assert.IsNull(room.CheckLeave(mob.Object));
        }

        [TestMethod]
        public void Room_CheckLeave_NotEnoughStamina()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(10);
            room.MovementCost = 5;

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You need to rest before you attempt to leave.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_NeverEnoughStaminaNotFull()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(2);
            room.MovementCost = 5;

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You need to rest before you attempt to leave.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_NeverEnoughStaminaFull()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(1);
            room.MovementCost = 5;

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You need to rest before you attempt to leave.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Assert.IsNull(room.CheckLeave(mob.Object));
        }

        [TestMethod]
        public void Room_CheckLeaveDirection_Guard()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            Mock<IGuard> guard = new Mock<IGuard>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();

            tagWrapper.Setup(e => e.WrapInTag("You can not leave.", TagType.Info)).Returns("message");
            guard.Setup(e => e.GuardDirections).Returns(new HashSet<Direction>() { Direction.North });
            guard.Setup(e => e.BlockLeaveMessage).Returns("You can not leave.");
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guard.Object });
            mob.Setup(e => e.Room).Returns(room);
            room.AddMobileObjectToRoom(npc.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = room.CheckLeaveDirection(mob.Object, Direction.North);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_InCombat()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.IsInCombat).Returns(true);
            mob.Setup(e => e.Stamina).Returns(1);
            room.MovementCost = 1;

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You can not leave while your fighting for your life.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;


            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Room_Enter_Npc()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.Room).Returns(room);
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            room.Enter(npc.Object);

            Assert.AreEqual(1, room.NonPlayerCharacters.Count);
            Assert.AreSame(npc.Object, room.NonPlayerCharacters[0]);
            npc.Verify(e => e.Room, Times.Once);
            evnt.Verify(e => e.EnterRoom(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Room_Enter_Pc()
        {
            Mock<IPlayerCharacter> npc = new Mock<IPlayerCharacter>();
            npc.Setup(e => e.Room).Returns(room);
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            room.Enter(npc.Object);

            Assert.AreEqual(1, room.PlayerCharacters.Count);
            Assert.AreSame(npc.Object, room.PlayerCharacters[0]);
            npc.Verify(e => e.Room, Times.Once);
            evnt.Verify(e => e.EnterRoom(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_Npc()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            room.AddMobileObjectToRoom(npc.Object);

            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            room.Leave(npc.Object, Direction.East);

            Assert.AreEqual(0, room.PlayerCharacters.Count);
            evnt.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_Pc()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            room.AddMobileObjectToRoom(pc.Object);

            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            room.Leave(pc.Object, Direction.East);

            Assert.AreEqual(0, room.PlayerCharacters.Count);
            evnt.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
        }

        [TestMethod]
        public void Room_FinishLoad_LoadPercentNpc()
        {
            Mock<ILoadPercentage> loadableItems = new Mock<ILoadPercentage>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();

            loadableItems.Setup(e => e.Load).Returns(true);
            loadableItems.Setup(e => e.Object).Returns(npc.Object);

            room.LoadableItems.Add(loadableItems.Object);

            room.FinishLoad();

            List<INonPlayerCharacter> npcList = new List<INonPlayerCharacter>(room.NonPlayerCharacters);
            Assert.IsTrue(npcList.Contains(npc.Object));
        }

        [TestMethod]
        public void Room_FinishLoad_LoadPercentItem()
        {
            Mock<ILoadPercentage> loadableItems = new Mock<ILoadPercentage>();
            Mock<IItem> item = new Mock<IItem>();

            loadableItems.Setup(e => e.Load).Returns(true);
            loadableItems.Setup(e => e.Object).Returns(item.Object);

            room.LoadableItems.Add(loadableItems.Object);

            room.FinishLoad();

            Assert.IsTrue(room.Items.Contains(item.Object));
        }

        #region Weather
        #region Properties
        #region High
        #region Precipitation
        [TestMethod]
        public void Room_RoomPrecipitationHighBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationHighBegin", room.RoomPrecipitationHighBegin);
            zone.Verify(e => e.ZonePrecipitationHighBegin, Times.Once);
            Assert.AreEqual("ZonePrecipitationHighBegin", room.RoomPrecipitationHighBegin);
            zone.Verify(e => e.ZonePrecipitationHighBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationHighBegin_Set()
        {
            room.RoomPrecipitationHighBegin = "test";
            Assert.AreEqual("test", room.RoomPrecipitationHighBegin);
        }

        [TestMethod]
        public void Room_RoomPrecipitationHighEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationHighEnd", room.RoomPrecipitationHighEnd);
            zone.Verify(e => e.ZonePrecipitationHighEnd, Times.Once);
            Assert.AreEqual("ZonePrecipitationHighEnd", room.RoomPrecipitationHighEnd);
            zone.Verify(e => e.ZonePrecipitationHighEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationHighEnd_Set()
        {
            room.RoomPrecipitationHighEnd = "test";
            Assert.AreEqual("test", room.RoomPrecipitationHighEnd);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraHighBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationExtraHighBegin", room.RoomPrecipitationExtraHighBegin);
            zone.Verify(e => e.ZonePrecipitationExtraHighBegin, Times.Once);
            Assert.AreEqual("ZonePrecipitationExtraHighBegin", room.RoomPrecipitationExtraHighBegin);
            zone.Verify(e => e.ZonePrecipitationExtraHighBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraHighBegin_Set()
        {
            room.RoomPrecipitationExtraHighBegin = "test";
            Assert.AreEqual("test", room.RoomPrecipitationExtraHighBegin);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraHighEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationExtraHighEnd", room.RoomPrecipitationExtraHighEnd);
            zone.Verify(e => e.ZonePrecipitationExtraHighEnd, Times.Once);
            Assert.AreEqual("ZonePrecipitationExtraHighEnd", room.RoomPrecipitationExtraHighEnd);
            zone.Verify(e => e.ZonePrecipitationExtraHighEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraHighEnd_Set()
        {
            room.RoomPrecipitationExtraHighEnd = "test";
            Assert.AreEqual("test", room.RoomPrecipitationExtraHighEnd);
        }
        #endregion Precipitation

        #region WindSpeed
        [TestMethod]
        public void Room_RoomWindspeedHighBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedHighBegin", room.RoomWindSpeedHighBegin);
            zone.Verify(e => e.ZoneWindSpeedHighBegin, Times.Once);
            Assert.AreEqual("ZoneWindSpeedHighBegin", room.RoomWindSpeedHighBegin);
            zone.Verify(e => e.ZoneWindSpeedHighBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedHighBegin_Set()
        {
            room.RoomWindSpeedHighBegin = "test";
            Assert.AreEqual("test", room.RoomWindSpeedHighBegin);
        }

        [TestMethod]
        public void Room_RoomWindSpeedHighEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedHighEnd", room.RoomWindSpeedHighEnd);
            zone.Verify(e => e.ZoneWindSpeedHighEnd, Times.Once);
            Assert.AreEqual("ZoneWindSpeedHighEnd", room.RoomWindSpeedHighEnd);
            zone.Verify(e => e.ZoneWindSpeedHighEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedHighEnd_Set()
        {
            room.RoomWindSpeedHighEnd = "test";
            Assert.AreEqual("test", room.RoomWindSpeedHighEnd);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraHighBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedExtraHighBegin", room.RoomWindSpeedExtraHighBegin);
            zone.Verify(e => e.ZoneWindSpeedExtraHighBegin, Times.Once);
            Assert.AreEqual("ZoneWindSpeedExtraHighBegin", room.RoomWindSpeedExtraHighBegin);
            zone.Verify(e => e.ZoneWindSpeedExtraHighBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraHighBegin_Set()
        {
            room.RoomWindSpeedExtraHighBegin = "test";
            Assert.AreEqual("test", room.RoomWindSpeedExtraHighBegin);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraHighEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedExtraHighEnd", room.RoomWindSpeedExtraHighEnd);
            zone.Verify(e => e.ZoneWindSpeedExtraHighEnd, Times.Once);
            Assert.AreEqual("ZoneWindSpeedExtraHighEnd", room.RoomWindSpeedExtraHighEnd);
            zone.Verify(e => e.ZoneWindSpeedExtraHighEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraHighEnd_Set()
        {
            room.RoomWindSpeedExtraHighEnd = "test";
            Assert.AreEqual("test", room.RoomWindSpeedExtraHighEnd);
        }
        #endregion WindSpeed
        #endregion High

        #region Low
        #region Precipitation
        [TestMethod]
        public void Room_RoomPrecipitationLowBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationLowBegin", room.RoomPrecipitationLowBegin);
            zone.Verify(e => e.ZonePrecipitationLowBegin, Times.Once);
            Assert.AreEqual("ZonePrecipitationLowBegin", room.RoomPrecipitationLowBegin);
            zone.Verify(e => e.ZonePrecipitationLowBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationLowBegin_Set()
        {
            room.RoomPrecipitationLowBegin = "test";
            Assert.AreEqual("test", room.RoomPrecipitationLowBegin);
        }

        [TestMethod]
        public void Room_RoomPrecipitationLowEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationLowEnd", room.RoomPrecipitationLowEnd);
            zone.Verify(e => e.ZonePrecipitationLowEnd, Times.Once);
            Assert.AreEqual("ZonePrecipitationLowEnd", room.RoomPrecipitationLowEnd);
            zone.Verify(e => e.ZonePrecipitationLowEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationLowEnd_Set()
        {
            room.RoomPrecipitationLowEnd = "test";
            Assert.AreEqual("test", room.RoomPrecipitationLowEnd);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraLowBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationExtraLowBegin", room.RoomPrecipitationExtraLowBegin);
            zone.Verify(e => e.ZonePrecipitationExtraLowBegin, Times.Once);
            Assert.AreEqual("ZonePrecipitationExtraLowBegin", room.RoomPrecipitationExtraLowBegin);
            zone.Verify(e => e.ZonePrecipitationExtraLowBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraLowBegin_Set()
        {
            room.RoomPrecipitationExtraLowBegin = "test";
            Assert.AreEqual("test", room.RoomPrecipitationExtraLowBegin);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraLowEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZonePrecipitationExtraLowEnd", room.RoomPrecipitationExtraLowEnd);
            zone.Verify(e => e.ZonePrecipitationExtraLowEnd, Times.Once);
            Assert.AreEqual("ZonePrecipitationExtraLowEnd", room.RoomPrecipitationExtraLowEnd);
            zone.Verify(e => e.ZonePrecipitationExtraLowEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomPrecipitationExtraLowEnd_Set()
        {
            room.RoomPrecipitationExtraLowEnd = "test";
            Assert.AreEqual("test", room.RoomPrecipitationExtraLowEnd);
        }
        #endregion Precipitation

        #region WindSpeed
        [TestMethod]
        public void Room_RoomWindspeedLowBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedLowBegin", room.RoomWindSpeedLowBegin);
            zone.Verify(e => e.ZoneWindSpeedLowBegin, Times.Once);
            Assert.AreEqual("ZoneWindSpeedLowBegin", room.RoomWindSpeedLowBegin);
            zone.Verify(e => e.ZoneWindSpeedLowBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedLowBegin_Set()
        {
            room.RoomWindSpeedLowBegin = "test";
            Assert.AreEqual("test", room.RoomWindSpeedLowBegin);
        }

        [TestMethod]
        public void Room_RoomWindSpeedLowEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedLowEnd", room.RoomWindSpeedLowEnd);
            zone.Verify(e => e.ZoneWindSpeedLowEnd, Times.Once);
            Assert.AreEqual("ZoneWindSpeedLowEnd", room.RoomWindSpeedLowEnd);
            zone.Verify(e => e.ZoneWindSpeedLowEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedLowEnd_Set()
        {
            room.RoomWindSpeedLowEnd = "test";
            Assert.AreEqual("test", room.RoomWindSpeedLowEnd);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraLowBegin_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedExtraLowBegin", room.RoomWindSpeedExtraLowBegin);
            zone.Verify(e => e.ZoneWindSpeedExtraLowBegin, Times.Once);
            Assert.AreEqual("ZoneWindSpeedExtraLowBegin", room.RoomWindSpeedExtraLowBegin);
            zone.Verify(e => e.ZoneWindSpeedExtraLowBegin, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraLowBegin_Set()
        {
            room.RoomWindSpeedExtraLowBegin = "test";
            Assert.AreEqual("test", room.RoomWindSpeedExtraLowBegin);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraLowEnd_Get()
        {
            Mock<IZone> zone = SetupWeatherZone();

            Assert.AreEqual("ZoneWindSpeedExtraLowEnd", room.RoomWindSpeedExtraLowEnd);
            zone.Verify(e => e.ZoneWindSpeedExtraLowEnd, Times.Once);
            Assert.AreEqual("ZoneWindSpeedExtraLowEnd", room.RoomWindSpeedExtraLowEnd);
            zone.Verify(e => e.ZoneWindSpeedExtraLowEnd, Times.Once);
        }

        [TestMethod]
        public void Room_RoomWindSpeedExtraLowEnd_Set()
        {
            room.RoomWindSpeedExtraLowEnd = "test";
            Assert.AreEqual("test", room.RoomWindSpeedExtraLowEnd);
        }
        #endregion WindSpeed
        #endregion Low
        #endregion Properties

        #region Notification
        #region Precipitation
        #region High
        [TestMethod]
        public void Room_PrecipitationNotification_HighBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(74);

            Assert.AreEqual("ZonePrecipitationHighBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_HighEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(73);

            Assert.AreEqual("ZonePrecipitationHighEnd", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraHighBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(89);

            Assert.AreEqual("ZonePrecipitationExtraHighBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraHighEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(88);

            Assert.AreEqual("ZonePrecipitationExtraHighEnd", room.PrecipitationNotification);
        }
        #endregion High
        #region Low
        [TestMethod]
        public void Room_PrecipitationNotification_LowBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(25);

            Assert.AreEqual("ZonePrecipitationLowBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_LowEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(26);

            Assert.AreEqual("ZonePrecipitationLowEnd", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraLowBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(10);

            Assert.AreEqual("ZonePrecipitationExtraLowBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraLowEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.Precipitation).Returns(11);

            Assert.AreEqual("ZonePrecipitationExtraLowEnd", room.PrecipitationNotification);
        }
        #endregion Low
        #endregion Precipitation

        #region WindSpeed
        #region High
        [TestMethod]
        public void Room_WindSpeedNotification_HighBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(74);

            Assert.AreEqual("ZoneWindSpeedHighBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_HighEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(73);

            Assert.AreEqual("ZoneWindSpeedHighEnd", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraHighBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(89);

            Assert.AreEqual("ZoneWindSpeedExtraHighBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraHighEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(88);

            Assert.AreEqual("ZoneWindSpeedExtraHighEnd", room.WindSpeedNotification);
        }
        #endregion High
        #region Low
        [TestMethod]
        public void Room_WindSpeedNotification_LowBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(25);

            Assert.AreEqual("ZoneWindSpeedLowBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_LowEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(26);

            Assert.AreEqual("ZoneWindSpeedLowEnd", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraLowBegin()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(10);

            Assert.AreEqual("ZoneWindSpeedExtraLowBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraLowEnd()
        {
            Mock<IWorld> world = SetupWeatherWorld();
            world.Setup(e => e.WindSpeed).Returns(11);

            Assert.AreEqual("ZoneWindSpeedExtraLowEnd", room.WindSpeedNotification);
        }
        #endregion Low
        #endregion WindSpeed
        #endregion Notification

        private Mock<IZone> SetupWeatherZone()
        {
            Tuple<Mock<IZone>, Mock<IWorld>> tuple = SetupWeather();
            return tuple.Item1;
        }

        private Mock<IWorld> SetupWeatherWorld()
        {
            Tuple<Mock<IZone>, Mock<IWorld>> tuple = SetupWeather();
            return tuple.Item2;
        }

        private Tuple<Mock<IZone>, Mock<IWorld>> SetupWeather()
        {
            Mock<IZone> zone = new Mock<IZone>();
            zone.Setup(e => e.ZoneWindSpeedExtraLowEnd).Returns("ZoneWindSpeedExtraLowEnd");
            zone.Setup(e => e.ZoneWindSpeedExtraLowBegin).Returns("ZoneWindSpeedExtraLowBegin");
            zone.Setup(e => e.ZoneWindSpeedLowEnd).Returns("ZoneWindSpeedLowEnd");
            zone.Setup(e => e.ZoneWindSpeedLowBegin).Returns("ZoneWindSpeedLowBegin");
            zone.Setup(e => e.ZonePrecipitationExtraLowEnd).Returns("ZonePrecipitationExtraLowEnd");
            zone.Setup(e => e.ZonePrecipitationExtraLowBegin).Returns("ZonePrecipitationExtraLowBegin");
            zone.Setup(e => e.ZonePrecipitationLowEnd).Returns("ZonePrecipitationLowEnd");
            zone.Setup(e => e.ZonePrecipitationLowBegin).Returns("ZonePrecipitationLowBegin");
            zone.Setup(e => e.ZoneWindSpeedExtraHighEnd).Returns("ZoneWindSpeedExtraHighEnd");
            zone.Setup(e => e.ZoneWindSpeedExtraHighBegin).Returns("ZoneWindSpeedExtraHighBegin");
            zone.Setup(e => e.ZoneWindSpeedHighEnd).Returns("ZoneWindSpeedHighEnd");
            zone.Setup(e => e.ZoneWindSpeedHighBegin).Returns("ZoneWindSpeedHighBegin");
            zone.Setup(e => e.ZonePrecipitationExtraHighEnd).Returns("ZonePrecipitationExtraHighEnd");
            zone.Setup(e => e.ZonePrecipitationExtraHighBegin).Returns("ZonePrecipitationExtraHighBegin");
            zone.Setup(e => e.ZonePrecipitationHighEnd).Returns("ZonePrecipitationHighEnd");
            zone.Setup(e => e.ZonePrecipitationHighBegin).Returns("ZonePrecipitationHighBegin");
            Dictionary<int, IZone> dictionary = new Dictionary<int, IZone>();
            dictionary.Add(1, zone.Object);
            Mock<IWorld> world = new Mock<IWorld>();
            world.Setup(e => e.Zones).Returns(dictionary);
            world.Setup(e => e.HighBegin).Returns(74);
            world.Setup(e => e.ExtraHighBegin).Returns(89);
            world.Setup(e => e.HighEnd).Returns(73);
            world.Setup(e => e.ExtraHighEnd).Returns(88);
            world.Setup(e => e.LowBegin).Returns(25);
            world.Setup(e => e.LowEnd).Returns(26);
            world.Setup(e => e.ExtraLowBegin).Returns(10);
            world.Setup(e => e.ExtraLowEnd).Returns(11);
            GlobalReference.GlobalValues.World = world.Object;

            return new Tuple<Mock<IZone>, Mock<IWorld>>(zone, world);
        }
        #endregion Weather
    }
}
