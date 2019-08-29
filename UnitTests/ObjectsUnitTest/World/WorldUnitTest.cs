using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Shared.FileIO.Interface;
using Moq;
using Objects.Global.Logging.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.GameDateTime.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Room;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Item.Interface;
using static Objects.Room.Room;
using Objects.Global.Commands.Interface;
using Objects.Command.Interface;
using Objects.Personality.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using static Objects.Mob.MobileObject;
using Objects.Command.World.Interface;
using Objects.Mob;
using static Objects.Global.Logging.LogSettings;
using System.IO;
using Objects.Global.MultiClassBonus.Interface;
using Objects.Global.Settings.Interface;
using Objects.Magic.Interface;
using Objects.Global.TickTimes.Interface;
using Objects.Crafting.Interface;
using Objects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using System.Collections.Concurrent;
using Objects.GameDateTime.Interface;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Shared.PerformanceCounters.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Interface;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Item;

namespace ObjectsUnitTest.World
{
    [TestClass]
    public class WorldUnitTest
    {
        Objects.World.World world;
        Mock<ICombat> combat;
        Mock<ICommand> command;
        Mock<ICommandList> commandList;
        Mock<ICounters> counters;
        Mock<ICraftsmanObject> craftsmanObject;
        Mock<IDefaultValues> defaultValues;
        Mock<IDefeatInfo> defeatInfo;
        Mock<IEnchantment> enchantment;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        Mock<IExit> exit;
        Mock<IFileIO> fileIO;
        Mock<IGameDateTime> gameDateTime;
        Mock<IGameStats> gameStats;
        Mock<IGlobalValues> globalValues;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<ILogger> logger;
        Mock<IMobileObjectCommand> mobCommand;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<IMount> mount;
        Mock<INotify> notify;
        Mock<INonPlayerCharacter> npc;
        Mock<IBaseObjectId> objectId;
        Mock<IMobileObject> otherMob;
        Mock<IParser> parser;
        Mock<IPlayerCharacter> pc;
        Mock<IPersonality> personality;
        Mock<IRandom> random;
        Mock<IResult> result;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<ISerialization> serialization;
        Mock<ISettings> settings;
        Mock<ITagWrapper> tagWrapper;
        Mock<ITickTimes> tickTimes;
        Mock<IZone> zone;



        List<ICraftsmanObject> craftsmanObjects;
        Objects.Zone.Zone deserializeZone;
        Dictionary<int, IRoom> dictionaryRoom;
        List<IEnchantment> enchantments;
        ConcurrentQueue<IMobileObject> followMobQueue;
        HashSet<IMount> loadedMounts;
        PropertyInfo notifiyPrecipitation;
        PropertyInfo notifiyWindSpeed;
        List<IPlayerCharacter> pcList;
        List<IPlayerCharacter> pcRoomList;
        List<IMobileObject> riders;



        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            combat = new Mock<ICombat>();
            command = new Mock<ICommand>();
            commandList = new Mock<ICommandList>();
            counters = new Mock<ICounters>();
            craftsmanObject = new Mock<ICraftsmanObject>();
            defaultValues = new Mock<IDefaultValues>();
            defeatInfo = new Mock<IDefeatInfo>();
            enchantment = new Mock<IEnchantment>();
            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            exit = new Mock<IExit>();
            fileIO = new Mock<IFileIO>();
            gameDateTime = new Mock<IGameDateTime>();
            gameStats = new Mock<IGameStats>();
            globalValues = new Mock<IGlobalValues>();
            inGameDateTime = new Mock<IInGameDateTime>();
            logger = new Mock<ILogger>();
            mobCommand = new Mock<IMobileObjectCommand>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            mount = new Mock<IMount>();
            notify = new Mock<INotify>();
            npc = new Mock<INonPlayerCharacter>();
            objectId = new Mock<IBaseObjectId>();
            otherMob = new Mock<IMobileObject>();
            parser = new Mock<IParser>();
            pc = new Mock<IPlayerCharacter>();
            personality = new Mock<IPersonality>();
            random = new Mock<IRandom>();
            result = new Mock<IResult>();
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            serialization = new Mock<ISerialization>();
            settings = new Mock<ISettings>();
            tagWrapper = new Mock<ITagWrapper>();
            tickTimes = new Mock<ITickTimes>();
            zone = new Mock<IZone>();


            craftsmanObjects = new List<ICraftsmanObject>();
            deserializeZone = new Objects.Zone.Zone();
            dictionaryRoom = new Dictionary<int, IRoom>();
            enchantments = new List<IEnchantment>();
            followMobQueue = new ConcurrentQueue<IMobileObject>();
            pcRoomList = new List<IPlayerCharacter>();
            riders = new List<IMobileObject>();

            command.Setup(e => e.CommandName).Returns("South");
            commandList.Setup(e => e.GetCommand(mount.Object, "South")).Returns(mobCommand.Object);
            commandList.Setup(e => e.GetCommand(npc.Object, "South")).Returns(mobCommand.Object);
            craftsmanObject.Setup(e => e.Completion).Returns(new DateTime(1, 1, 1));
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftsmanId).Returns(objectId.Object);
            craftsmanObject.Setup(e => e.NextNotifcation).Returns(new DateTime(1, 1, 1));
            craftsmanObjects.Add(craftsmanObject.Object);
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(100);
            enchantment.Setup(e => e.DefeatInfo).Returns(defeatInfo.Object);
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(9999, 12, 31));
            engine.Setup(e => e.Combat).Returns(combat.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            exit.Setup(e => e.Zone).Returns(1);
            exit.Setup(e => e.Room).Returns(2);
            fileIO.Setup(e => e.Exists(@"LogStatsLocation\00010101\Stats.stat")).Returns(true);
            fileIO.Setup(e => e.ReadAllText(@"LogStatsLocation\00010101\Stats.stat")).Returns("serial");
            fileIO.Setup(e => e.GetFilesFromDirectory("PlayerCharacterDirectory")).Returns(new string[] { "c:\\test.char" });
            fileIO.Setup(e => e.ReadAllText("c:\\test.char")).Returns("serializedPlayer");
            fileIO.Setup(e => e.GetFilesFromDirectory("ZoneDirectory", "*.zone")).Returns(new string[] { "c:\\zone.zone" });
            fileIO.Setup(e => e.ReadAllText("c:\\zone.zone")).Returns("serializedZone");

            globalValues.Setup(e => e.TickCounter).Returns(0);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            mobCommand.Setup(e => e.PerformCommand(mount.Object, command.Object)).Returns(result.Object);
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0 coins");
            mount.Setup(e => e.CommmandQueueCount).Returns(1);
            mount.Setup(e => e.DequeueCommand()).Returns("South");
            mount.Setup(e => e.Movement).Returns(4);
            mount.Setup(e => e.Riders).Returns(riders);
            npc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            objectId.Setup(e => e.Zone).Returns(1);
            parser.Setup(e => e.Parse("command")).Returns(command.Object);
            parser.Setup(e => e.Parse("say hi")).Returns(command.Object);
            parser.Setup(e => e.Parse("South")).Returns(command.Object);
            pc.Setup(e => e.CraftsmanObjects).Returns(new List<ICraftsmanObject>());
            pc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            pc.Setup(e => e.FollowTarget).Returns(npc.Object);
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            pc.Setup(e => e.Name).Returns("test");
            pc.Setup(e => e.Room).Returns(room.Object);
            personality.Setup(e => e.Process(npc.Object, null)).Returns("test");
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            result.Setup(e => e.ResultMessage).Returns("result");
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.Items).Returns(new List<IItem>());
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room.Setup(e => e.OtherMobs).Returns(new List<IMobileObject>());
            room.Setup(e => e.PrecipitationNotification).Returns("rain");
            room.Setup(e => e.WindSpeedNotification).Returns("wind");
            room.Setup(e => e.Zone).Returns(1);
            room2.Setup(e => e.Zone).Returns(1);
            room2.Setup(e => e.Id).Returns(1);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room2.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room2.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room2.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            serialization.Setup(e => e.Deserialize<List<ICounters>>("serial")).Returns(new List<ICounters>());
            serialization.Setup(e => e.Deserialize<Objects.Zone.Zone>("serial")).Returns(deserializeZone);
            serialization.Setup(e => e.Serialize(It.IsAny<object>())).Returns("abc");
            settings.Setup(e => e.LogStats).Returns(true);
            settings.Setup(e => e.LogStatsLocation).Returns("LogStatsLocation");
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("PlayerCharacterDirectory");
            settings.Setup(e => e.ZoneDirectory).Returns("ZoneDirectory");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tickTimes.Setup(e => e.MedianTime).Returns(1m);
            zone.Setup(e => e.ResetTime).Returns(gameDateTime.Object);
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);

            dictionaryRoom.Add(0, room.Object);
            enchantments.Add(enchantment.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Counters = counters.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.CountersLog = new List<ICounters>();
            GlobalReference.GlobalValues.Serialization = serialization.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            GlobalReference.GlobalValues.GameStats = gameStats.Object;

            world = new Objects.World.World();
            world.Zones.Add(1, zone.Object);
            PropertyInfo propertyInfoWorld = world.GetType().GetProperty("Characters", BindingFlags.NonPublic | BindingFlags.Instance);
            pcList = (List<IPlayerCharacter>)propertyInfoWorld.GetValue(world);
            FieldInfo fieldInfo = world.GetType().GetField("_loadedMounts", BindingFlags.NonPublic | BindingFlags.Instance);
            loadedMounts = (HashSet<IMount>)fieldInfo.GetValue(world);
            notifiyPrecipitation = world.GetType().GetProperty("NotifyPrecipitation", BindingFlags.NonPublic | BindingFlags.Instance);
            notifiyWindSpeed = world.GetType().GetProperty("NotifyWindSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo = world.GetType().GetField("_followMobQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            followMobQueue = (ConcurrentQueue<IMobileObject>)fieldInfo.GetValue(world);
        }

        #region PerformTick
        #region PerformCombatTick
        [TestMethod]
        public void World_PerformTick_PerformCombatTick()
        {
            world.PerformTick();

            combat.Verify(e => e.ProcessCombatRound(), Times.Once);
        }
        #endregion PerformCombatTick

        #region PutPlayersIntoWorld
        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoom()
        {
            room.Setup(e => e.PlayerCharacters).Returns(pcList);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoomId()
        {
            pc.Setup(e => e.RoomId).Returns(new RoomId(1, 0));
            pc.Setup(e => e.Room).Returns<IRoom>(null);
            pc.SetupSequence(e => e.Room)
                .Returns(null)
                .Returns(room.Object);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoomIdButInvalid()
        {
            dictionaryRoom.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns<IRoom>(null);
            pc.Setup(e => e.RoomId).Returns(new RoomId(1, 2));
            pc.SetupSequence(e => e.Room)
                .Returns(null)
                .Returns(room.Object);


            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_NoRoomInfo()
        {
            dictionaryRoom.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns<IRoom>(null);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            room.Verify(e => e.Enter(pc.Object), Times.Once);
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_DismountCalled()
        {
            dictionaryRoom.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns<IRoom>(null);
            loadedMounts.Add(mount.Object);
            riders.Add(pc.Object);

            world.AddPlayerQueue.Enqueue(pc.Object);

            Assert.IsTrue(riders.Contains(pc.Object));

            world.PerformTick();

            Assert.AreEqual(0, riders.Count);


        }
        #endregion PutPlayersIntoWorld

        #region UpdateWeather
        [TestMethod]
        public void World_PerformTick_UpdateWeather()
        {
            world.PerformTick();

            Assert.AreEqual(0, world.PrecipitationGoal);
            Assert.AreEqual(0, world.WindSpeedGoal);
            Assert.AreEqual(49, world.Precipitation);
            Assert.AreEqual(49, world.WindSpeed);
        }

        [TestMethod]
        public void World_PerformTick_UpdateWeather_MoveUpScale()
        {
            world.Precipitation = 10;
            world.WindSpeed = 10;

            world.PerformTick();

            Assert.AreEqual(11, world.Precipitation);
            Assert.AreEqual(11, world.WindSpeed);
            Assert.IsFalse((bool)notifiyPrecipitation.GetValue(world));
            Assert.IsFalse((bool)notifiyWindSpeed.GetValue(world));
        }
        #endregion UpdateWeather

        #region ReloadZones
        [TestMethod]
        public void World_PerformTick_ReloadZones()
        {
            gameDateTime.Setup(e => e.Day).Returns(1);
            gameDateTime.Setup(e => e.IsLessThan(gameDateTime.Object)).Returns(true);

            world.Zones.Clear();  //clears out the zone added at initialization

            PropertyInfo info = world.GetType().GetProperty("_zoneIdToFileMap", BindingFlags.Instance | BindingFlags.NonPublic);

            ((Dictionary<int, string>)info.GetValue(world)).Add(0, @"LogStatsLocation\00010101\Stats.stat");
            world.Zones.Add(0, zone.Object);

            world.PerformTick();

            Assert.AreEqual(1, world.Zones.Count);
            Assert.AreSame(deserializeZone, world.Zones[0]);
        }
        #endregion ReloadZones

        #region UpdatePerformanceCounters
        [TestMethod]
        public void WorldPerformTick_UpdatePerformanceCounters()
        {
            world.PerformTick();

            Assert.AreEqual(0, GlobalReference.GlobalValues.Counters.ConnnectedPlayers);
            Assert.AreEqual(1, GlobalReference.GlobalValues.Counters.CPU);
            Assert.AreEqual(0, GlobalReference.GlobalValues.Counters.MaxTickTimeInMs);
            Assert.AreNotEqual(0, GlobalReference.GlobalValues.Counters.Memory);
        }

        [TestMethod]
        public void WorldPerformTick_UpdatePerformanceCounters_WriteLogStats()
        {
            MethodInfo methodInfo = world.GetType().GetMethod("WriteLogStats", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(world, new object[] { new DateTime() });

            serialization.Verify(e => e.Deserialize<List<ICounters>>("serial"), Times.Once);
            fileIO.Verify(e => e.WriteFile(@"LogStatsLocation\00010101\Stats.stat", "abc"), Times.Once);
        }
        #endregion UpdatePerformanceCounters

        #region SaveCharacters
        [TestMethod]
        public void World_PerformTick_ProcessRoom_SaveCharacters_15Minutes()
        {
            DateTime startupDateTime = new DateTime(1, 1, 1);

            pcList.Add(pc.Object);

            FieldInfo field = world.GetType().GetField("_lastSave", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(world, startupDateTime);

            world.PerformTick();

            fileIO.Verify(e => e.WriteFile(@"PlayerCharacterDirectory\test.char", "abc"), Times.Once);
            Assert.IsTrue((DateTime)field.GetValue(world) > startupDateTime);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SaveCharacters_Not15Minutes()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();
            DateTime startupDateTime = DateTime.UtcNow;

            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            pc.Setup(e => e.Name).Returns("test");
            pc.Setup(e => e.Room).Returns(room.Object);
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serialized");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            pcList.Add(pc.Object);

            FieldInfo field = world.GetType().GetField("_lastSave", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(world, startupDateTime);


            world.PerformTick();

            fileIO.Verify(e => e.WriteFile(@"c:\test.char", "serialized"), Times.Never);
            Assert.IsTrue((DateTime)field.GetValue(world) == startupDateTime);
        }
        #endregion SaveCharacters

        #region ProcessRoom
        [TestMethod]
        public void World_PerformTick_ProcessRoom_VerifyWeatherMessage()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.Weather });
            world.Precipitation = 10;
            world.WindSpeed = 10;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.Is<ITranslationMessage>(f => f.Message == "rain")), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.Is<ITranslationMessage>(f => f.Message == "wind")), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "rain")), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "wind")), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CommunicationCommandNotReconnized()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            npc.SetupSequence(e => e.DequeueCommunication())
               .Returns("not valid")
               .Returns(null);
            pc.SetupSequence(e => e.DequeueCommunication())
               .Returns("not valid")
               .Returns(null);
            pc.Setup(e => e.FollowTarget).Returns<IMobileObject>(null);

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()), Times.Never);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()), Times.Never);
            parser.Verify(e => e.Parse("not valid"), Times.Exactly(2));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessEnchantments()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());

            world.PerformTick();

            //the enchantment does not count per parameter but instead counts the number of times it was called
            //so we have to combine the number of times the pc and npc were called.
            enchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Exactly(2));
            enchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobPersonality()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });

            world.PerformTick();

            npc.Verify(e => e.EnqueueCommand("test"));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobCommand()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.DequeueCommand()).Returns("command");

            world.PerformTick();

            notify.Verify(e => e.Mob(npc.Object, It.Is<ITranslationMessage>(f => f.Message == "result")), Times.Once);
            npc.VerifySet(e => e.LastProccessedTick = 0, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobCommand_UnknownCommand()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            pc.SetupSequence(e => e.DequeueCommand()).Returns("South");

            world.PerformTick();

            string expectedMessage = @"Unable to figure out how to South.
To see a list of all commands type MAN.
To see info on how to use a command type MAN and then the COMMAND.";

            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == expectedMessage)), Times.Once);
            pc.VerifySet(e => e.LastProccessedTick = 0, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_PerformHeartBeatBigTick()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
            evnt.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateStand()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Position).Returns(CharacterPosition.Stand);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateMounted()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Position).Returns(CharacterPosition.Mounted);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateSit()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Position).Returns(CharacterPosition.Sit);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 2, Times.Once);
            npc.VerifySet(e => e.Mana = 20, Times.Once);
            npc.VerifySet(e => e.Stamina = 200, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateRelax()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Position).Returns(CharacterPosition.Relax);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 3, Times.Once);
            npc.VerifySet(e => e.Mana = 30, Times.Once);
            npc.VerifySet(e => e.Stamina = 303, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateSleep()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Position).Returns(CharacterPosition.Sleep);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 4, Times.Once);
            npc.VerifySet(e => e.Mana = 40, Times.Once);
            npc.VerifySet(e => e.Stamina = 400, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessPlayerNotifications()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            pc.Setup(e => e.CraftsmanObjects).Returns(craftsmanObjects);

            world.PerformTick();

            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
            craftsmanObject.VerifySet(e => e.NextNotifcation = It.IsAny<DateTime>());
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_AddToFollowQueue()
        {
            pc.Setup(e => e.FollowTarget).Returns(npc.Object);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            npc.Setup(e => e.Health).Returns(100);

            world.PerformTick();
            pc.VerifyGet(e => e.FollowTarget); //verify the follow target was read to see who they are following
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CleanupEnchantments_OldEnchantments()
        {
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(1, 1, 1));
            pc.Setup(e => e.Enchantments).Returns(enchantments);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            Assert.AreEqual(1, enchantments.Count);
            world.PerformTick();
            Assert.AreEqual(0, enchantments.Count);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CleanupEnchantments_FailToDefeat()
        {
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(9999, 1, 1));
            defeatInfo.Setup(e => e.DoesPayerDefeatEnchantment(pc.Object)).Returns(false);
            pc.Setup(e => e.Enchantments).Returns(enchantments);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            Assert.AreEqual(1, enchantments.Count);
            world.PerformTick();
            Assert.AreEqual(1, enchantments.Count);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CleanupEnchantments_DefeatEnchantment()
        {
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(9999, 1, 1));
            defeatInfo.Setup(e => e.DoesPayerDefeatEnchantment(pc.Object)).Returns(true);
            pc.Setup(e => e.Enchantments).Returns(enchantments);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            Assert.AreEqual(1, enchantments.Count);
            world.PerformTick();
            Assert.AreEqual(0, enchantments.Count);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Water()
        {
            world.Precipitation = 100;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("water"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Fire()
        {
            world.Precipitation = 0;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("fire"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Air()
        {
            world.WindSpeed = 100;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("air"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Earth()
        {
            world.WindSpeed = 0;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("earth"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }
        #endregion ProcessRoom

        #region CatchPlayersOutSideOfTheWorldDueToReloadedZones
        [TestMethod]
        public void World_PerformTick_CatchPlayersOutSideOfTheWorldDueToReloadedZones()
        {
            dictionaryRoom.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns(room2.Object);
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            pcList.Add(pc.Object);

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room2.Object), Times.Once);
        }
        #endregion CatchPlayersOutSideOfTheWorldDueToReloadedZones

        #region ProccessSerialCommands
        [TestMethod]
        public void World_PerformTick_ProccessSerialCommands_ProcessFollowMobs()
        {
            followMobQueue.Enqueue(pc.Object);
            npc.Setup(e => e.Room).Returns(room2.Object);
            room.Setup(e => e.Down).Returns(exit.Object);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });

            dictionaryRoom.Add(2, room2.Object);

            world.PerformTick();

            pc.Verify(e => e.EnqueueCommand("Down"));
        }

        [TestMethod]
        public void World_PerformTick_ProccessSerialCommands_ProcessFollowMobs_ToFar()
        {
            Mock<IRoom> room3 = new Mock<IRoom>();
            Mock<IRoom> room4 = new Mock<IRoom>();
            Mock<IRoom> room5 = new Mock<IRoom>();
            Mock<IRoom> room6 = new Mock<IRoom>();
            Mock<IExit> exit2 = new Mock<IExit>();
            Mock<IExit> exit3 = new Mock<IExit>();
            Mock<IExit> exit4 = new Mock<IExit>();
            Mock<IExit> exit5 = new Mock<IExit>();
            Mock<IExit> exit6 = new Mock<IExit>();

            followMobQueue.Enqueue(pc.Object);
            npc.Setup(e => e.Room).Returns(room6.Object);
            npc.Setup(e => e.SentenceDescription).Returns("npc");
            room.Setup(e => e.North).Returns(exit.Object);
            room2.Setup(e => e.North).Returns(exit2.Object);
            room3.Setup(e => e.North).Returns(exit3.Object);
            room4.Setup(e => e.North).Returns(exit4.Object);
            room5.Setup(e => e.North).Returns(exit5.Object);
            room6.Setup(e => e.North).Returns(exit6.Object);
            room3.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room3.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room3.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            room4.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room4.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room4.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            room5.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room5.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room5.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            room6.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room6.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room6.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room6.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            exit.Setup(e => e.Zone).Returns(1);
            exit.Setup(e => e.Room).Returns(2);
            exit2.Setup(e => e.Zone).Returns(1);
            exit2.Setup(e => e.Room).Returns(3);
            exit3.Setup(e => e.Zone).Returns(1);
            exit3.Setup(e => e.Room).Returns(4);
            exit4.Setup(e => e.Zone).Returns(1);
            exit4.Setup(e => e.Room).Returns(5);
            exit5.Setup(e => e.Zone).Returns(1);
            exit5.Setup(e => e.Room).Returns(6);
            dictionaryRoom.Add(2, room2.Object);
            dictionaryRoom.Add(3, room3.Object);
            dictionaryRoom.Add(4, room4.Object);
            dictionaryRoom.Add(5, room5.Object);
            dictionaryRoom.Add(6, room6.Object);

            world.PerformTick();
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "You have lost track of the npc and had to quit following them.")), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProccessSerialCommands_Mounts()
        {
            loadedMounts.Add(mount.Object);

            world.PerformTick();

            //mount has 4 movement and will keep giving the command south until it's movement runs out
            mount.Verify(e => e.DequeueCommand(), Times.Exactly(4));
        }

        #endregion ProccessSerialCommands

        #region DoWorldCommand
        [TestMethod]
        public void World_PerformTick_DoWorldCommand()
        {
            world.WorldCommands.Enqueue("GameStats");
            gameStats.SetupSequence(e => e.GenerateGameStats())
                .Returns("result")
                .Returns("result2");

            world.PerformTick();

            string result = null;
            world.WorldResults.TryGetValue("GameStats", out result);
            Assert.AreEqual("result", result);

            world.WorldCommands.Enqueue("GameStats");

            world.PerformTick();
            world.WorldResults.TryGetValue("GameStats", out result);
            Assert.AreEqual("result2", result);
        }
        #endregion DoWorldCommand
        #endregion PerformTick

        #region Load Save Stuff
        [TestMethod]
        public void World_SaveCharcter()
        {
            world.SaveCharcter(pc.Object);

            pc.VerifySet(e => e.Room = null, Times.Once);
            fileIO.Verify(e => e.WriteFile(@"PlayerCharacterDirectory\test.char", "abc"), Times.Once);
        }

        [TestMethod]
        public void World_LoadCharacter_AllReadyInGame()
        {
            pcList.Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("test");
            Assert.AreSame(pc.Object, result);
        }

        [TestMethod]
        public void World_LoadCharacter_LoadFromFile()
        {
            PlayerCharacter realPc = new PlayerCharacter();

            serialization.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns(realPc);

            IPlayerCharacter result = world.LoadCharacter("test");
            Assert.AreSame(realPc, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void World_LoadCharacter_LoadFromFileUnableToDeserialize()
        {
            serialization.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns<PlayerCharacter>(null);

            try
            {
                world.LoadCharacter("test");
            }
            catch
            {
                logger.Verify(e => e.Log(LogLevel.ERROR, "Unable to deserialize string as PlayerCharacter.\r\nserializedPlayer"), Times.Once);
                throw;
            }
        }

        [TestMethod]
        public void World_LoadCharacter_PlayerNotFound()
        {
            fileIO.Setup(e => e.GetFilesFromDirectory("c:\\")).Returns(new string[] { });

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void World_LoadWorld_NoFilesFound()
        {
            fileIO.Setup(e => e.GetFilesFromDirectory("ZoneDirectory", "*.zone")).Returns(new string[] { });

            world.LoadWorld();
        }

        [TestMethod]
        public void World_LoadWorld_ZoneLoads()
        {
            world.Zones.Clear(); //remove the item added from above
            Objects.Zone.Zone realZone = new Objects.Zone.Zone();
            PropertyInfo info = world.GetType().GetProperty("_zoneIdToFileMap", BindingFlags.Instance | BindingFlags.NonPublic);

            serialization.Setup(e => e.Deserialize<Objects.Zone.Zone>("serializedZone")).Returns(realZone);
            realZone.Rooms.Add(1, room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.OtherMobs).Returns(new List<IMobileObject>() { otherMob.Object });

            world.LoadWorld();

            npc.Verify(e=>e.FinishLoad(-1), Times.Once);
            otherMob.Verify(e => e.FinishLoad(-1), Times.Once);
            Assert.AreSame(realZone, world.Zones[0]);
            string storedFileName = ((Dictionary<int, string>)info.GetValue(world))[0];
            Assert.AreEqual("c:\\zone.zone", storedFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void World_LoadWorld_ZoneUnableToDeserialize()
        {
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { "c:\\zone.zone" });
            fileIO.Setup(e => e.ReadAllText("c:\\zone.zone")).Returns("serializedZone");
            xmlSerializer.Setup(e => e.Deserialize<Objects.Zone.Zone>("serializedZone")).Returns<Objects.Zone.Zone>(null);
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            try
            {
                world.LoadWorld();
            }
            catch
            {
                logger.Verify(e => e.Log(LogLevel.ERROR, "Unable to deserialize string as Zone.\r\nserializedZone"), Times.Once);
                throw;
            }
        }

        [TestMethod]
        public void World_SaveWorld()
        {
            world.Zones.Clear();  //clears out the zone added at initialization

            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();
            Mock<ISettings> settings = new Mock<ISettings>();

            rooms.Add(0, room.Object);
            zone.Setup(e => e.Name).Returns("zone");
            zone.Setup(e => e.Rooms).Returns(rooms);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(pcList);
            pcList.Add(pc.Object);
            serializer.Setup(e => e.Serialize(zone.Object)).Returns("serializedZone");
            world.Zones.Add(0, zone.Object);
            settings.Setup(e => e.ZoneDirectory).Returns("c:\\");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.SaveWorld();

            npc.VerifySet(e => e.Room = null);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            fileIO.Verify(e => e.WriteFile("c:\\zone.zone", "serializedZone"), Times.Once);
        }
        #endregion Load Save Stuff

        [TestMethod]
        public void World_LogOutCharacter_CharacterFound()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();
            List<IMobileObject> riders = new List<IMobileObject>();

            pc.Setup(e => e.Name).Returns("name");
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.Mount).Returns(mount.Object);
            room.Setup(e => e.PlayerCharacters).Returns(pcRoomList);
            pcRoomList.Add(pc.Object);
            pcList.Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serializedPC");
            loadedMounts.Add(mount.Object);
            riders.Add(pc.Object);
            mount.Setup(e => e.Riders).Returns(riders);

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            world.LogOutCharacter(pc.Object);

            fileIO.Verify(e => e.WriteFile("c:\\name.char", "serializedPC"));
            Assert.AreEqual(0, pcList.Count);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            Assert.AreEqual(0, riders.Count);
            Assert.AreEqual(0, loadedMounts.Count);
        }

        [TestMethod]
        public void World_CreateCharacter()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IMultiClassBonus> multicClassBonus = new Mock<IMultiClassBonus>();

            settings.Setup(e => e.BaseStatValue).Returns(1);
            settings.Setup(e => e.AssignableStatPoints).Returns(2);

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MultiClassBonus = multicClassBonus.Object;

            IPlayerCharacter result = world.CreateCharacter("userName", "password");

            Assert.AreEqual("userName", result.Name);
            Assert.AreEqual("password", result.Password);
            Assert.AreEqual(1, result.StrengthStat);
            Assert.AreEqual(1, result.DexterityStat);
            Assert.AreEqual(1, result.ConstitutionStat);
            Assert.AreEqual(1, result.IntelligenceStat);
            Assert.AreEqual(1, result.WisdomStat);
            Assert.AreEqual(1, result.CharismaStat);
            Assert.AreEqual(1, result.Level);
            Assert.AreEqual(result.MaxHealth, result.Health);
            Assert.AreEqual(result.MaxStamina, result.Stamina);
            Assert.AreEqual(result.MaxMana, result.Mana);
            Assert.AreEqual("userName", result.SentenceDescription);
            Assert.AreEqual("userName", result.ShortDescription);
            Assert.AreEqual("userName", result.LookDescription);
            Assert.IsTrue(result.KeyWords.Contains("userName"));
            Assert.AreEqual(1, result.GuildPoints);
            Assert.AreEqual(1, world.AddPlayerQueue.Count);
        }

        [TestMethod]
        public void World_AddMountToWorld()
        {
            world.AddMountToWorld(mount.Object);

            Assert.AreEqual(1, loadedMounts.Count);
            Assert.IsTrue(loadedMounts.Contains(mount.Object));
        }

        [TestMethod]
        public void World_RemoveMountFromWorld()
        {
            loadedMounts.Add(mount.Object);

            world.RemoveMountFromWorld(mount.Object);

            Assert.AreEqual(0, loadedMounts.Count);
            Assert.IsFalse(loadedMounts.Contains(mount.Object));
        }

        [TestMethod]
        public void World_WriteDismount_MountFound()
        {
            List<IMobileObject> lmob = new List<IMobileObject>();
            lmob.Add(npc.Object);
            loadedMounts.Add(mount.Object);
            mount.Setup(e => e.Riders).Returns(lmob);
            mount.Setup(e => e.SentenceDescription).Returns("horse");

            IResult result = world.Dismount(npc.Object);

            Assert.AreEqual("You dismount from the horse.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void World_WriteDismount_MountNotFound()
        {
            IResult result = world.Dismount(npc.Object);

            Assert.AreEqual("You are not riding a mount.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }
    }
}
