using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
using Objects.Personality.Interface;
using Objects.LoadPercentage.Interface;
using System.Linq;
using Shared.FileIO.Interface;
using Objects.Global.Settings.Interface;
using System.Reflection;
using Objects.Global.Serialization.Interface;
using Objects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Global.StringManuplation.Interface;

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
        Mock<ITagWrapper> tagWrapper;
        Mock<ILoadableItems> loadableItem;
        Mock<ILoadPercentage> loadPercentage;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IMobileObject> mob;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<IEvent> evnt;
        Mock<IEngine> engine;
        Mock<IGuard> guard;
        Mock<INotify> notify;
        Mock<IStringManipulator> stringManipulator;

        List<INonPlayerCharacter> lNpc;
        List<IPlayerCharacter> lPc;
        List<IMobileObject> lOtherMob;
        List<IItem> lItems;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            fileIO = new Mock<IFileIO>();
            item = new Mock<IItem>();
            serializer = new Mock<ISerialization>();
            settings = new Mock<ISettings>();
            tagWrapper = new Mock<ITagWrapper>();
            loadableItem = new Mock<ILoadableItems>();
            loadPercentage = new Mock<ILoadPercentage>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            mob = new Mock<IMobileObject>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            evnt = new Mock<IEvent>();
            engine = new Mock<IEngine>();
            guard = new Mock<IGuard>();
            notify = new Mock<INotify>();
            stringManipulator = new Mock<IStringManipulator>();

            settings.Setup(e => e.VaultDirectory).Returns("vault");
            serializer.Setup(e => e.Serialize(It.IsAny<List<IItem>>())).Returns("serializedList");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            loadPercentage.Setup(e => e.Load).Returns(true);
            loadPercentage.Setup(e => e.Object).Returns(item.Object);
            npc.Setup(e => e.Stamina).Returns(10);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guard.Object });
            npc.Setup(e => e.SentenceDescription).Returns("npc");
            pc.Setup(e => e.Stamina).Returns(10);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });
            pc.Setup(e => e.SentenceDescription).Returns("pc");
            mob.Setup(e => e.Stamina).Returns(10);
            mob.Setup(e => e.SentenceDescription).Returns("mob");
            engine.Setup(e => e.Event).Returns(evnt.Object);
            guard.Setup(e => e.GuardDirections).Returns(new HashSet<Direction>() { Direction.North });
            guard.Setup(e => e.BlockLeaveMessage).Returns("You can not leave.");
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("npc")).Returns("Npc");
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("pc")).Returns("Pc");
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("mob")).Returns("Mob");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            room = new Objects.Room.Room("examineDescription", "lookDescription", "shortDescription");
            room.Zone = 1;
            room.Id = 2;
            room.MovementCost = 1;


            FieldInfo fieldInfo = room.GetType().GetField("_nonPlayerCharacters", BindingFlags.Instance | BindingFlags.NonPublic);
            lNpc = (List<INonPlayerCharacter>)fieldInfo.GetValue(room);
            fieldInfo = room.GetType().GetField("_playerCharacters", BindingFlags.Instance | BindingFlags.NonPublic);
            lPc = (List<IPlayerCharacter>)fieldInfo.GetValue(room);
            fieldInfo = room.GetType().GetField("_otherMobs", BindingFlags.Instance | BindingFlags.NonPublic);
            lOtherMob = (List<IMobileObject>)fieldInfo.GetValue(room);
            fieldInfo = room.GetType().GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            lItems = (List<IItem>)fieldInfo.GetValue(room);

            SetupWeather();
            npc.Setup(e => e.Room).Returns(room);
            pc.Setup(e => e.Room).Returns(room);
            mob.Setup(e => e.Room).Returns(room);
        }

        [TestMethod]
        public void Room_Constructor()
        {
            Assert.AreEqual("examineDescription", room.ExamineDescription);
            Assert.AreEqual("lookDescription", room.LookDescription);
            Assert.AreEqual("shortDescription", room.ShortDescription);
        }

        [TestMethod]
        public void Room_AddItemToRoom_ValutContentsWritten()
        {
            room.Attributes.Add(RoomAttribute.Vault);

            room.AddItemToRoom(item.Object);

            fileIO.Verify(e => e.WriteFile("vault\\1-2.vault", "serializedList"), Times.Once);
        }

        [TestMethod]
        public void Room_AddMobToRoom_Npc()
        {
            room.AddMobileObjectToRoom(npc.Object);

            Assert.AreEqual(1, lNpc.Count());
            Assert.AreEqual(0, lPc.Count());
            Assert.AreEqual(0, lOtherMob.Count());
            Assert.IsTrue(room.NonPlayerCharacters.Contains(npc.Object));
        }

        [TestMethod]
        public void Room_AddMobToRoom_Pc()
        {
            room.AddMobileObjectToRoom(pc.Object);

            Assert.AreEqual(0, lNpc.Count());
            Assert.AreEqual(1, lPc.Count());
            Assert.AreEqual(0, lOtherMob.Count());
            Assert.IsTrue(room.PlayerCharacters.Contains(pc.Object));
        }

        [TestMethod]
        public void Room_AddMobToRoom_OtherMob()
        {
            room.AddMobileObjectToRoom(mob.Object);

            Assert.AreEqual(0, lNpc.Count());
            Assert.AreEqual(0, lPc.Count());
            Assert.AreEqual(1, lOtherMob.Count());
            Assert.IsTrue(room.OtherMobs.Contains(mob.Object));
        }

        [TestMethod]
        public void Room_RemoveMobToRoom_Npc()
        {
            lNpc.Add(npc.Object);

            room.RemoveMobileObjectFromRoom(npc.Object);

            Assert.AreEqual(0, lNpc.Count());
            Assert.AreEqual(0, lPc.Count());
            Assert.AreEqual(0, lOtherMob.Count());
        }

        [TestMethod]
        public void Room_RemoveMobToRoom_Pc()
        {
            lPc.Add(pc.Object);

            room.RemoveMobileObjectFromRoom(pc.Object);

            Assert.AreEqual(0, lNpc.Count());
            Assert.AreEqual(0, lPc.Count());
            Assert.AreEqual(0, lOtherMob.Count());
        }

        [TestMethod]
        public void Room_RemoveMobToRoom_OtherMob()
        {
            lOtherMob.Add(mob.Object);

            room.RemoveMobileObjectFromRoom(mob.Object);

            Assert.AreEqual(0, lNpc.Count());
            Assert.AreEqual(0, lPc.Count());
            Assert.AreEqual(0, lOtherMob.Count());
        }

        [TestMethod]
        public void Room_RemoveItemFromRoom_ValutContentsWritten()
        {
            lItems.Add(item.Object);
            lItems.Add(item.Object);

            room.Attributes.Add(RoomAttribute.Vault);

            room.RemoveItemFromRoom(item.Object);

            fileIO.Verify(e => e.WriteFile("vault\\1-2.vault", "serializedList"), Times.Once);
        }

        [TestMethod]
        public void Room_ToString()
        {
            string result = room.ToString();

            Assert.AreEqual("1-2", result);
        }

        [TestMethod]
        public void Room_CheckEnter_NoNpcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);

            Assert.IsNull(room.CheckEnter(npc.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_NpcNotSmall()
        {
            lNpc.Add(new Mock<INonPlayerCharacter>().Object);

            Assert.IsNull(room.CheckEnter(npc.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_NpcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);
            lNpc.Add(new Mock<INonPlayerCharacter>().Object);

            IResult result = room.CheckEnter(npc.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("The room is to small to fit another person in there.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NoNpc()
        {
            room.Attributes.Add(RoomAttribute.NoNPC);

            IResult result = room.CheckEnter(npc.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Non player characters can not enter here.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NoPcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);

            Assert.IsNull(room.CheckEnter(pc.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_PcNotSmall()
        {
            lPc.Add(new Mock<IPlayerCharacter>().Object);

            Assert.IsNull(room.CheckEnter(pc.Object));
        }

        [TestMethod]
        public void Room_CheckEnter_PcSmall()
        {
            room.Attributes.Add(RoomAttribute.Small);
            lPc.Add(new Mock<IPlayerCharacter>().Object);

            IResult result = room.CheckEnter(pc.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("The room is to small to fit another person in there.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_NotOwner()
        {
            room.Owner = "NotYou";

            IResult result = room.CheckEnter(pc.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("That property belongs to NotYou and you are not on the guest list.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_Owner()
        {
            room.Owner = "NotYou";
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "NotYou" });

            IResult result = room.CheckEnter(pc.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Room_CheckEnter_NotGuest()
        {
            room.Owner = "NotYou";
            room.Guests.Add("AlsoNotYou");

            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });

            IResult result = room.CheckEnter(pc.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("That property belongs to NotYou and you are not on the guest list.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckEnter_Guest()
        {
            room.Owner = "NotYou";
            room.Guests.Add("PC");

            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });

            IResult result = room.CheckEnter(pc.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Room_CheckLeave_Pass()
        {
            mob.Setup(e => e.IsInCombat).Returns(false);

            Assert.IsNull(room.CheckLeave(mob.Object));
        }

        [TestMethod]
        public void Room_CheckLeave_NotEnoughStamina()
        {
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(10);
            room.MovementCost = 5;

            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need to rest before you attempt to leave.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_NeverEnoughStaminaNotFull()
        {
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(2);
            room.MovementCost = 5;

            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need to rest before you attempt to leave.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_NeverEnoughStaminaFull()
        {
            mob.Setup(e => e.IsInCombat).Returns(false);
            mob.Setup(e => e.Stamina).Returns(1);
            mob.Setup(e => e.MaxStamina).Returns(1);
            room.MovementCost = 5;

            Assert.IsNull(room.CheckLeave(mob.Object));
        }

        [TestMethod]
        public void Room_CheckLeaveDirection_Guard()
        {
            lNpc.Add(npc.Object);

            IResult result = room.CheckLeaveDirection(mob.Object, Direction.North);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not leave.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_CheckLeave_InCombat()
        {
            mob.Setup(e => e.IsInCombat).Returns(true);
            mob.Setup(e => e.Stamina).Returns(1);
            room.MovementCost = 1;

            IResult result = room.CheckLeave(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not leave while your fighting for your life.", result.ResultMessage);
        }

        [TestMethod]
        public void Room_Enter_Npc()
        {
            room.Enter(npc.Object);

            Assert.AreEqual(1, room.NonPlayerCharacters.Count);
            Assert.AreSame(npc.Object, room.NonPlayerCharacters[0]);
            npc.Verify(e => e.Room, Times.Once);
            evnt.Verify(e => e.EnterRoom(npc.Object), Times.Once);
            notify.Verify(e => e.Room(npc.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Npc enters the room."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Enter_Pc()
        {
            room.Enter(pc.Object);

            Assert.AreEqual(1, room.PlayerCharacters.Count);
            Assert.AreSame(pc.Object, room.PlayerCharacters[0]);
            pc.Verify(e => e.Room, Times.Once);
            evnt.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            notify.Verify(e => e.Room(pc.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Pc enters the room."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Enter_OtherMob()
        {
            room.Enter(mob.Object);

            Assert.AreEqual(1, room.OtherMobs.Count);
            Assert.AreSame(mob.Object, room.OtherMobs[0]);
            mob.Verify(e => e.Room, Times.Once);
            evnt.Verify(e => e.EnterRoom(mob.Object), Times.Once);
            notify.Verify(e => e.Room(mob.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Mob enters the room."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_Npc()
        {
            lNpc.Add(npc.Object);

            room.Leave(npc.Object, Direction.East, false);

            Assert.AreEqual(0, room.PlayerCharacters.Count);
            evnt.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
            npc.VerifySet(e => e.Stamina = 9, Times.Once);
            notify.Verify(e => e.Room(npc.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Npc leaves East."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_Pc()
        {
            lPc.Add(pc.Object);

            room.Leave(pc.Object, Direction.East, false);

            Assert.AreEqual(0, room.PlayerCharacters.Count);
            evnt.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            pc.VerifySet(e => e.Stamina = 9, Times.Once);
            notify.Verify(e => e.Room(pc.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Pc leaves East."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_OtherMob()
        {
            lOtherMob.Add(mob.Object);

            room.Leave(mob.Object, Direction.East, false);

            Assert.AreEqual(0, room.OtherMobs.Count);
            evnt.Verify(e => e.LeaveRoom(mob.Object, Direction.East), Times.Once);
            mob.VerifySet(e => e.Stamina = 9, Times.Once);
            notify.Verify(e => e.Room(mob.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Mob leaves East."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_Leave_Mounted()
        {
            lPc.Add(pc.Object);

            room.Leave(pc.Object, Direction.East, true);

            Assert.AreEqual(0, room.PlayerCharacters.Count);
            evnt.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            pc.VerifySet(e => e.Stamina = It.IsAny<int>(), Times.Never);
            notify.Verify(e => e.Room(pc.Object, null, room, It.Is<ITranslationMessage>(f => f.Message == "Pc leaves East."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Room_FinishLoad_LoadPercentNpc()
        {
            loadPercentage.Setup(e => e.Object).Returns(npc.Object);

            room.LoadableItems.Add(loadPercentage.Object);

            room.FinishLoad();

            List<INonPlayerCharacter> npcList = new List<INonPlayerCharacter>(room.NonPlayerCharacters);
            Assert.IsTrue(npcList.Contains(npc.Object));
        }

        [TestMethod]
        public void Room_FinishLoad_LoadPercentItem()
        {
            room.LoadableItems.Add(loadPercentage.Object);

            room.FinishLoad();

            Assert.IsTrue(room.Items.Contains(item.Object));
        }

        [TestMethod]
        public void Room_FinishLoad_ReloadValut()
        {
            IReadOnlyList<IItem> items = new List<IItem>() { item.Object };

            room.Attributes.Add(RoomAttribute.Vault);
            fileIO.Setup(e => e.Exists("vault\\1-2.vault")).Returns(true);
            fileIO.Setup(e => e.ReadAllText("vault\\1-2.vault")).Returns("contents");
            serializer.Setup(e => e.Deserialize<IReadOnlyList<IItem>>("contents")).Returns(items);

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
            world.Setup(e => e.Precipitation).Returns(74);

            Assert.AreEqual("ZonePrecipitationHighBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_HighEnd()
        {
            world.Setup(e => e.Precipitation).Returns(73);

            Assert.AreEqual("ZonePrecipitationHighEnd", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraHighBegin()
        {
            world.Setup(e => e.Precipitation).Returns(89);

            Assert.AreEqual("ZonePrecipitationExtraHighBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraHighEnd()
        {
            world.Setup(e => e.Precipitation).Returns(88);

            Assert.AreEqual("ZonePrecipitationExtraHighEnd", room.PrecipitationNotification);
        }
        #endregion High
        #region Low
        [TestMethod]
        public void Room_PrecipitationNotification_LowBegin()
        {
            world.Setup(e => e.Precipitation).Returns(25);

            Assert.AreEqual("ZonePrecipitationLowBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_LowEnd()
        {
            world.Setup(e => e.Precipitation).Returns(26);

            Assert.AreEqual("ZonePrecipitationLowEnd", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraLowBegin()
        {
            world.Setup(e => e.Precipitation).Returns(10);

            Assert.AreEqual("ZonePrecipitationExtraLowBegin", room.PrecipitationNotification);
        }

        [TestMethod]
        public void Room_PrecipitationNotification_ExtraLowEnd()
        {
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
            world.Setup(e => e.WindSpeed).Returns(74);

            Assert.AreEqual("ZoneWindSpeedHighBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_HighEnd()
        {
            world.Setup(e => e.WindSpeed).Returns(73);

            Assert.AreEqual("ZoneWindSpeedHighEnd", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraHighBegin()
        {
            world.Setup(e => e.WindSpeed).Returns(89);

            Assert.AreEqual("ZoneWindSpeedExtraHighBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraHighEnd()
        {
            world.Setup(e => e.WindSpeed).Returns(88);

            Assert.AreEqual("ZoneWindSpeedExtraHighEnd", room.WindSpeedNotification);
        }
        #endregion High
        #region Low
        [TestMethod]
        public void Room_WindSpeedNotification_LowBegin()
        {
            world.Setup(e => e.WindSpeed).Returns(25);

            Assert.AreEqual("ZoneWindSpeedLowBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_LowEnd()
        {
            world.Setup(e => e.WindSpeed).Returns(26);

            Assert.AreEqual("ZoneWindSpeedLowEnd", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraLowBegin()
        {
            world.Setup(e => e.WindSpeed).Returns(10);

            Assert.AreEqual("ZoneWindSpeedExtraLowBegin", room.WindSpeedNotification);
        }

        [TestMethod]
        public void Room_WindSpeedNotification_ExtraLowEnd()
        {
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
