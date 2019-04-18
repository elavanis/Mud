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

namespace ObjectsUnitTest.World
{
    [TestClass]
    public class WorldUnitTest
    {
        Objects.World.World world;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        Mock<ICombat> combat;
        Mock<IRandom> random;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IRoom> room;
        Mock<IZone> zone;
        Mock<INotify> notify;
        Mock<ITagWrapper> tagWrapper;
        Dictionary<int, IRoom> dictionaryRoom;
        Mock<IGameDateTime> gameDateTime;
        Mock<ISettings> settings;
        Mock<ISerialization> serialization;
        Mock<IFileIO> fileIO;
        Mock<IGlobalValues> globalValues;
        Mock<IDefaultValues> defaultValues;
        PropertyInfo propertyInfoWorld;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            combat = new Mock<ICombat>();
            random = new Mock<IRandom>();
            inGameDateTime = new Mock<IInGameDateTime>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            room = new Mock<IRoom>();
            zone = new Mock<IZone>();
            notify = new Mock<INotify>();
            tagWrapper = new Mock<ITagWrapper>();
            gameDateTime = new Mock<IGameDateTime>();
            settings = new Mock<ISettings>();
            serialization = new Mock<ISerialization>();
            fileIO = new Mock<IFileIO>();
            globalValues = new Mock<IGlobalValues>();
            defaultValues = new Mock<IDefaultValues>();

            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ICounters> counters = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();
            dictionaryRoom = new Dictionary<int, IRoom>();

            dictionaryRoom.Add(0, room.Object);
            engine.Setup(e => e.Combat).Returns(combat.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);
            zone.Setup(e => e.ResetTime).Returns(gameDateTime.Object);
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            npc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            pc.Setup(e => e.CraftsmanObjects).Returns(new List<ICraftsmanObject>());
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.FollowTarget).Returns(npc.Object);
            pc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            tickTimes.Setup(e => e.MedianTime).Returns(1m);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            settings.Setup(e => e.LogStats).Returns(true);
            settings.Setup(e => e.LogStatsLocation).Returns("c:\\");
            serialization.Setup(e => e.Serialize(It.IsAny<object>())).Returns("abc");
            serialization.Setup(e => e.Deserialize<List<ICounters>>("serial")).Returns(new List<ICounters>());
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            globalValues.Setup(e => e.TickCounter).Returns(0);
            fileIO.Setup(e => e.Exists("c:\\00010101\\Stats.stat")).Returns(true);
            fileIO.Setup(e => e.ReadAllText("c:\\00010101\\Stats.stat")).Returns("serial");
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(100);

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

            world = new Objects.World.World();
            world.Zones.Add(0, zone.Object);
            propertyInfoWorld = world.GetType().GetProperty("Characters", BindingFlags.NonPublic | BindingFlags.Instance);

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
            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();

            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            pc.Setup(e => e.Room).Returns(room.Object);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoomId()
        {
            pc.Setup(e => e.Room).Returns((IRoom)null);

            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            pc.Setup(e => e.RoomId).Returns(new RoomId(1, 1));
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
            pc.Setup(e => e.Room).Returns((IRoom)null);

            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
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
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            pc.Setup(e => e.Room).Returns<IRoom>(null);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            room.Verify(e => e.Enter(pc.Object), Times.Once);
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
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
            PropertyInfo notifyPrecipitation = world.GetType().GetProperty("NotifyPrecipitation", BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo notifiyWindSpeed = world.GetType().GetProperty("NotifyWindSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
            Mock<ICounters> counter = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            world.Precipitation = 10;
            world.WindSpeed = 10;

            GlobalReference.GlobalValues.Counters = counter.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            Assert.AreEqual(11, world.Precipitation);
            Assert.AreEqual(11, world.WindSpeed);
            Assert.IsFalse((bool)notifyPrecipitation.GetValue(world));
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
            Mock<IFileIO> fileIo = new Mock<IFileIO>();
            Mock<ISerialization> serialization = new Mock<ISerialization>();
            Objects.Zone.Zone deserializeZone = new Objects.Zone.Zone();

            ((Dictionary<int, string>)info.GetValue(world)).Add(0, "blah");
            world.Zones.Add(0, zone.Object);
            fileIo.Setup(e => e.ReadAllText("blah")).Returns("seraializedZone");
            serialization.Setup(e => e.Deserialize<Objects.Zone.Zone>("seraializedZone")).Returns(deserializeZone);

            GlobalReference.GlobalValues.FileIO = fileIo.Object;
            GlobalReference.GlobalValues.Serialization = serialization.Object;

            world.PerformTick();

            Assert.AreEqual(1, world.Zones.Count);
            Assert.AreSame(deserializeZone, world.Zones[0]);
        }
        #endregion ReloadZones

        #region UpdatePerformanceCounters
        [TestMethod]
        public void WorldPerformTick_UpdatePerformanceCounters()
        {
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            Assert.AreEqual(0, GlobalReference.GlobalValues.Counters.ConnnectedPlayers);
            Assert.AreEqual(0, GlobalReference.GlobalValues.Counters.CPU);
            Assert.AreEqual(0, GlobalReference.GlobalValues.Counters.MaxTickTimeInMs);
            Assert.AreNotEqual(0, GlobalReference.GlobalValues.Counters.Memory);
        }

        [TestMethod]
        public void WorldPerformTick_UpdatePerformanceCounters_WriteLogStats()
        {
            MethodInfo methodInfo = world.GetType().GetMethod("WriteLogStats", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(world, new object[] { new DateTime() });

            serialization.Verify(e => e.Deserialize<List<ICounters>>("serial"), Times.Once);
            fileIO.Verify(e => e.WriteFile("c:\\00010101\\Stats.stat", "abc"), Times.Once);
        }
        #endregion UpdatePerformanceCounters

        #region SaveCharacters
        [TestMethod]
        public void World_PerformTick_ProcessRoom_SaveCharacters_15Minutes()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();
            DateTime startupDateTime = new DateTime(1, 1, 1);

            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            pc.Setup(e => e.Name).Returns("test");
            pc.Setup(e => e.Room).Returns(room.Object);
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serialized");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            List<IPlayerCharacter> pcList = (List<IPlayerCharacter>)propertyInfoWorld.GetValue(world);
            pcList.Add(pc.Object);

            FieldInfo field = world.GetType().GetField("_lastSave", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(world, startupDateTime);


            world.PerformTick();

            fileIO.Verify(e => e.WriteFile(@"c:\test.char", "serialized"), Times.Once);
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

            List<IPlayerCharacter> pcList = (List<IPlayerCharacter>)propertyInfoWorld.GetValue(world);
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
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mobCommand = new Mock<IMobileObjectCommand>();
            Mock<IResult> result = new Mock<IResult>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.Weather });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.PrecipitationNotification).Returns("rain");
            room.Setup(e => e.WindSpeedNotification).Returns("wind");
            world.Precipitation = 10;
            world.WindSpeed = 10;
            npc.SetupSequence(e => e.DequeueCommunication())
                .Returns("say hi")
                .Returns(null);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            parser.Setup(e => e.Parse("say hi")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("say");
            commandList.Setup(e => e.GetCommand(npc.Object, "say")).Returns(mobCommand.Object);
            result.Setup(e => e.ResultMessage).Returns("result");
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            mobCommand.Verify(e => e.PerformCommand(npc.Object, command.Object));
            //npc.Verify(e => e.EnqueueMessage("result"), Times.Once);
            //npc.Verify(e => e.EnqueueMessage("rain"), Times.Once);
            //npc.Verify(e => e.EnqueueMessage("wind"), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()), Times.Exactly(3));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CommunicationCommandNotReconnized()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.SetupSequence(e => e.DequeueCommunication())
                .Returns("say hi")
                .Returns(null);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            parser.Setup(e => e.Parse("say hi")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("say");
            commandList.Setup(e => e.GetCommand(npc.Object, "say")).Returns<IMobileObjectCommand>(null);

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessEnchantments()
        {
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());

            world.PerformTick();

            //the enchantment does not count per parameter but instead counts the number of times it was called
            //so we have to combine the number of times the pc and npc were called.
            enchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobPersonality()
        {
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Mock<IPersonality> personality = new Mock<IPersonality>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });
            personality.Setup(e => e.Process(npc.Object, null)).Returns("test");

            world.PerformTick();

            npc.Verify(e => e.EnqueueCommand("test"));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobCommand()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mobCommand = new Mock<IMobileObjectCommand>();
            Mock<IResult> result = new Mock<IResult>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            npc.SetupSequence(e => e.DequeueCommand())
                .Returns("command")
                .Returns(null)
                .Returns("3");
            parser.Setup(e => e.Parse("command")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("command");
            commandList.Setup(e => e.GetCommand(npc.Object, "command")).Returns(mobCommand.Object);
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);
            result.Setup(e => e.ResultMessage).Returns("result");

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            world.PerformTick();

            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()));
            npc.VerifySet(e => e.LastProccessedTick = 0, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobCommand_UnknownCommand()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mobCommand = new Mock<IMobileObjectCommand>();
            Mock<IResult> result = new Mock<IResult>();
            ITranslationMessage message = null;

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            npc.SetupSequence(e => e.DequeueCommand())
                .Returns("3");
            parser.Setup(e => e.Parse("3")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("4");
            commandList.Setup(e => e.GetCommand(npc.Object, "command")).Returns(mobCommand.Object);
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);
            result.Setup(e => e.ResultMessage).Returns("result");
            notify.Setup(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>())).Callback<IMobileObject, ITranslationMessage>((mob, translationMessage) => { message = translationMessage; });

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            world.PerformTick();

            Assert.AreEqual(@"Unable to figure out how to 4.
To see a list of all commands type MAN.
To see infon on how to use a command type MAN and then the COMMAND.", message.Message);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_PerformHeartBeatBigTick()
        {
            Mock<ICounters> counter = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Counters = counter.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateStand()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Stand);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateMounted()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Mounted);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateSit()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
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
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
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
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
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
            List<ICraftsmanObject> craftsmanObjects = new List<ICraftsmanObject>();
            Mock<ICraftsmanObject> craftsmanObject = new Mock<ICraftsmanObject>();
            Mock<IBaseObjectId> objectId = new Mock<IBaseObjectId>();
            Mock<ITagWrapper> tagwrapper = new Mock<ITagWrapper>();

            craftsmanObjects.Add(craftsmanObject.Object);
            craftsmanObject.Setup(e => e.Completion).Returns(new DateTime(1, 1, 1));
            craftsmanObject.Setup(e => e.NextNotifcation).Returns(new DateTime(1, 1, 1));
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftsmanId).Returns(objectId.Object);
            objectId.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            pc.Setup(e => e.CraftsmanObjects).Returns(craftsmanObjects);

            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;

            world.PerformTick();

            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
            craftsmanObject.VerifySet(e => e.NextNotifcation = It.IsAny<DateTime>());
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_AddToFolloQueue()
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
            List<IEnchantment> enchantments = new List<IEnchantment>();
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            enchantments.Add(enchantment.Object);
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
            List<IEnchantment> enchantments = new List<IEnchantment>();
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            Mock<IDefeatInfo> defeatInfo = new Mock<IDefeatInfo>();

            enchantments.Add(enchantment.Object);
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(9999, 1, 1));
            enchantment.Setup(e => e.DefeatInfo).Returns(defeatInfo.Object);
            defeatInfo.Setup(e => e.DoesPayerDefeatEnchantment(pc.Object)).Returns(false);
            pc.Setup(e => e.Enchantments).Returns(enchantments);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            Assert.AreEqual(1, enchantments.Count);
            world.PerformTick();
            Assert.AreEqual(1, enchantments.Count);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Water()
        {
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            world.Precipitation = 100;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("water"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Fire()
        {
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            world.Precipitation = 0;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("fire"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Air()
        {
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            world.WindSpeed = 100;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("air"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_SpawnElemental_Earth()
        {
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            world.WindSpeed = 0;

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(It.Is<INonPlayerCharacter>(f => f.KeyWords.Contains("earth"))), Times.Once);
            notify.Verify(e => e.Room(It.IsAny<INonPlayerCharacter>(), null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CleanupEnchantments_DefeatEnchantment()
        {
            List<IEnchantment> enchantments = new List<IEnchantment>();
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            Mock<IDefeatInfo> defeatInfo = new Mock<IDefeatInfo>();

            enchantments.Add(enchantment.Object);
            enchantment.Setup(e => e.EnchantmentEndingDateTime).Returns(new DateTime(9999, 1, 1));
            enchantment.Setup(e => e.DefeatInfo).Returns(defeatInfo.Object);
            defeatInfo.Setup(e => e.DoesPayerDefeatEnchantment(pc.Object)).Returns(true);
            pc.Setup(e => e.Enchantments).Returns(enchantments);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });

            Assert.AreEqual(1, enchantments.Count);
            world.PerformTick();
            Assert.AreEqual(0, enchantments.Count);
        }
        #endregion ProcessRoom

        #region CatchPlayersOutSideOfTheWorldDueToReloadedZones
        [TestMethod]
        public void World_PerformTick_CatchPlayersOutSideOfTheWorldDueToReloadedZones()
        {
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Mock<IRoom> room2 = new Mock<IRoom>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns(room2.Object);
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            ((List<IPlayerCharacter>)propertyInfoWorld.GetValue(world)).Add(pc.Object);
            room2.Setup(e => e.Zone).Returns(1);
            room2.Setup(e => e.Id).Returns(1);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room2.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room2.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room2.Object), Times.Once);
        }
        #endregion CatchPlayersOutSideOfTheWorldDueToReloadedZones

        #region ProccessSerialCommands
        [TestMethod]
        public void World_PerformTick_ProccessSerialCommands_ProcessFollowMobs()
        {
            FieldInfo fieldInfo = world.GetType().GetField("_followMobQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            ((ConcurrentQueue<IMobileObject>)fieldInfo.GetValue(world)).Enqueue(pc.Object);

            Mock<IRoom> room2 = new Mock<IRoom>();
            Mock<IExit> exit = new Mock<IExit>();

            npc.Setup(e => e.Room).Returns(room2.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            room.Setup(e => e.Down).Returns(exit.Object);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room2.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room2.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room2.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            exit.Setup(e => e.Zone).Returns(0);
            exit.Setup(e => e.Room).Returns(2);
            dictionaryRoom.Add(2, room2.Object);


            world.PerformTick();
            pc.Verify(e => e.EnqueueCommand("Down"));
        }

        [TestMethod]
        public void World_PerformTick_ProccessSerialCommands_ProcessFollowMobs_ToFar()
        {
            FieldInfo fieldInfo = world.GetType().GetField("_followMobQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            ((ConcurrentQueue<IMobileObject>)fieldInfo.GetValue(world)).Enqueue(pc.Object);

            Mock<IRoom> room2 = new Mock<IRoom>();
            Mock<IRoom> room3 = new Mock<IRoom>();
            Mock<IRoom> room4 = new Mock<IRoom>();
            Mock<IRoom> room5 = new Mock<IRoom>();
            Mock<IRoom> room6 = new Mock<IRoom>();
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IExit> exit2 = new Mock<IExit>();
            Mock<IExit> exit3 = new Mock<IExit>();
            Mock<IExit> exit4 = new Mock<IExit>();
            Mock<IExit> exit5 = new Mock<IExit>();
            Mock<IExit> exit6 = new Mock<IExit>();

            npc.Setup(e => e.Room).Returns(room6.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.SentenceDescription).Returns("npc");

            room.Setup(e => e.North).Returns(exit.Object);
            room2.Setup(e => e.North).Returns(exit2.Object);
            room3.Setup(e => e.North).Returns(exit3.Object);
            room4.Setup(e => e.North).Returns(exit4.Object);
            room5.Setup(e => e.North).Returns(exit5.Object);
            room6.Setup(e => e.North).Returns(exit6.Object);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room2.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            room2.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
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

            exit.Setup(e => e.Zone).Returns(0);
            exit.Setup(e => e.Room).Returns(2);
            dictionaryRoom.Add(2, room2.Object);
            exit2.Setup(e => e.Zone).Returns(0);
            exit2.Setup(e => e.Room).Returns(3);
            dictionaryRoom.Add(3, room3.Object);
            exit3.Setup(e => e.Zone).Returns(0);
            exit3.Setup(e => e.Room).Returns(4);
            dictionaryRoom.Add(4, room4.Object);
            exit4.Setup(e => e.Zone).Returns(0);
            exit4.Setup(e => e.Room).Returns(5);
            dictionaryRoom.Add(5, room5.Object);
            exit5.Setup(e => e.Zone).Returns(0);
            exit5.Setup(e => e.Room).Returns(6);
            dictionaryRoom.Add(6, room6.Object);

            ITranslationMessage message = null;
            notify.Setup(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>())).Callback<IMobileObject, ITranslationMessage>((mob, translationMessage) => { message = translationMessage; });


            world.PerformTick();
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()), Times.Once);
            Assert.AreEqual("You have lost track of the npc and had to quit following them.", message.Message);
        }
        #endregion ProccessSerialCommands

        #region DoWorldCommand
        [TestMethod]
        public void World_PerformTick_DoWorldCommand()
        {
            Mock<IGameStats> gameStats = new Mock<IGameStats>();

            world.WorldCommands.Enqueue("GameStats");
            world.GameStatsInterface = gameStats.Object;
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

        [TestMethod]
        public void World_SaveCharcter()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();

            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            pc.Setup(e => e.Name).Returns("test");
            pc.Setup(e => e.Room).Returns(room.Object);
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serialized");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            world.SaveCharcter(pc.Object);

            pc.VerifySet(e => e.Room = null, Times.Once);
            fileIO.Verify(e => e.WriteFile(@"c:\test.char", "serialized"), Times.Once);
        }

        [TestMethod]
        public void World_LoadCharacter_AllReadyInGame()
        {
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            ((List<IPlayerCharacter>)propertyInfoWorld.GetValue(world)).Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.AreSame(pc.Object, result);
        }

        [TestMethod]
        public void World_LoadCharacter_LoadFromFile()
        {
            PlayerCharacter realPc = new PlayerCharacter();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> seralizer = new Mock<ISerialization>();
            Mock<IPlayerCharacter> pc2 = new Mock<IPlayerCharacter>();
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            pc2.Setup(e => e.Name).Returns("bob");
            ((List<IPlayerCharacter>)propertyInfoWorld.GetValue(world)).Add(pc2.Object);
            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { "c:\\name.char" });
            fileIO.Setup(e => e.ReadAllText("c:\\name.char")).Returns("serializedPlayer");
            seralizer.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns(realPc);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = seralizer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.AreSame(realPc, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void World_LoadCharacter_LoadFromFileUnableToDeserialize()
        {
            PlayerCharacter realPc = new PlayerCharacter();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> seralizer = new Mock<ISerialization>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { "c:\\name.char" });
            fileIO.Setup(e => e.ReadAllText("c:\\name.char")).Returns("serializedPlayer");
            seralizer.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns<PlayerCharacter>(null);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = seralizer.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            try
            {
                world.LoadCharacter("name");
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
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { });
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void World_LoadWorld_NoFilesFound()
        {
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { });
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.LoadWorld();
        }

        [TestMethod]
        public void World_LoadWorld_ZoneLoads()
        {
            world.Zones.Clear(); //remove the item added from above
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Objects.Zone.Zone realZone = new Objects.Zone.Zone();
            Mock<IRoom> room = new Mock<IRoom>();
            PropertyInfo info = world.GetType().GetProperty("_zoneIdToFileMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { "c:\\zone.zone" });
            fileIO.Setup(e => e.ReadAllText("c:\\zone.zone")).Returns("serializedZone");
            xmlSerializer.Setup(e => e.Deserialize<Objects.Zone.Zone>("serializedZone")).Returns(realZone);
            realZone.Rooms.Add(1, room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>());
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.LoadWorld();

            npc.VerifySet(e => e.Room = room.Object, Times.Once);
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
            List<IPlayerCharacter> pcList = new List<IPlayerCharacter>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Mock<ISettings> settings = new Mock<ISettings>();

            rooms.Add(0, room.Object);
            zone.Setup(e => e.Name).Returns("zone");
            zone.Setup(e => e.Rooms).Returns(rooms);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(pcList);
            pcList.Add(pc.Object);
            xmlSerializer.Setup(e => e.Serialize(zone.Object)).Returns("serializedZone");
            world.Zones.Add(0, zone.Object);
            settings.Setup(e => e.ZoneDirectory).Returns("c:\\");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.SaveWorld();

            npc.VerifySet(e => e.Room = null);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            fileIO.Verify(e => e.WriteFile("c:\\zone.zone", "serializedZone"), Times.Once);
        }

        [TestMethod]
        public void World_LogOutCharacter_CharacterFound()
        {
            List<IPlayerCharacter> listPC = new List<IPlayerCharacter>();
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();

            pc.Setup(e => e.Name).Returns("name");
            pc.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(listPC);
            listPC.Add(pc.Object);
            ((List<IPlayerCharacter>)propertyInfoWorld.GetValue(world)).Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serializedPC");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            world.LogOutCharacter("name");

            Assert.AreEqual(0, ((List<IPlayerCharacter>)propertyInfoWorld.GetValue(world)).Count);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
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
    }
}
