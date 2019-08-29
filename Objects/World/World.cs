using Objects.World.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Zone.Interface;
using System.Collections.Concurrent;
using Objects.Global;
using System.IO;
using static Objects.Global.Logging.LogSettings;
using System.Diagnostics.CodeAnalysis;
using Objects.Room.Interface;
using Objects.Mob;
using Objects.Interface;
using static Objects.Room.Room;
using Objects.Command.Interface;
using Objects.Command;
using Objects.Personality.Interface;
using static Objects.Mob.MobileObject;
using Objects.Magic.Interface;
using System.Diagnostics;
using Objects.Crafting.Interface;
using Objects.Language;
using Objects.Global.Direction;
using static Objects.Skill.Skills.Track;
using Shared.PerformanceCounters;
using Shared.PerformanceCounters.Interface;
using Objects.Mob.SpecificNPC;
using Objects.Language.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Skill.Skills;

namespace Objects.World
{
    public class World : IWorld
    {
        private object _zoneRefreshPadlock;
        private int _lastZoneReload = 0;
        private object _tickPadlock;
        private ConcurrentQueue<IMobileObject> _followMobQueue = new ConcurrentQueue<IMobileObject>();
        private DateTime _lastSave = DateTime.UtcNow;
        private int _lastLogMinute = -1;
        private HashSet<IMount> _loadedMounts = new HashSet<IMount>();
        private OrderablePartitioner<Tuple<int, int>> _zonePartitioner;
        private List<IZone> _zones;
        private bool RegerateTick
        {
            get
            {
                return GlobalReference.GlobalValues.TickCounter % 18 == 0;
            }
        }


        public World()
        {
            _zoneRefreshPadlock = new object();
            _tickPadlock = new object();

            WeatherTriggers = new Dictionary<int, string>();
            WeatherTriggers.Add(HighBegin, nameof(HighBegin));
            WeatherTriggers.Add(HighEnd, nameof(HighEnd));
            WeatherTriggers.Add(ExtraHighBegin, nameof(ExtraHighBegin));
            WeatherTriggers.Add(ExtraHighEnd, nameof(ExtraHighEnd));
            WeatherTriggers.Add(LowBegin, nameof(LowBegin));
            WeatherTriggers.Add(ExtraLowBegin, nameof(ExtraLowBegin));
            WeatherTriggers.Add(LowEnd, nameof(LowEnd));
            WeatherTriggers.Add(ExtraLowEnd, nameof(ExtraLowEnd));
        }

        #region Weather
        [ExcludeFromCodeCoverage]
        private List<int> NotifyDown { get; set; } = new List<int>() { 73, 88, 25, 10 };
        [ExcludeFromCodeCoverage]
        private List<int> NotifyUp { get; set; } = new List<int>() { 74, 89, 26, 11 };
        [ExcludeFromCodeCoverage]
        private bool NotifyPrecipitation { get; set; } = false;
        [ExcludeFromCodeCoverage]
        private bool NotifyWindSpeed { get; set; } = false;

        [ExcludeFromCodeCoverage]
        public int Precipitation { get; set; } = 50;
        [ExcludeFromCodeCoverage]
        public int PrecipitationGoal { get; set; } = 50;
        [ExcludeFromCodeCoverage]
        public int WindSpeed { get; set; } = 50;
        [ExcludeFromCodeCoverage]
        public int WindSpeedGoal { get; set; } = 50;

        #region Weather High Points
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationHighBegin { get; set; } = "It beings to rain.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationHighEnd { get; set; } = "It stops raining.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationExtraHighBegin { get; set; } = "The rain is coming down in sheets now making it hard to see.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationExtraHighEnd { get; set; } = "The rain begins to lighten.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedHighBegin { get; set; } = "The wind picks up and begins to howl.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedHighEnd { get; set; } = "The wind calms down to a nice breeze.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedExtraHighBegin { get; set; } = "The wind becomes a roar making it hard to hear while whipping at you, threatening to knock you over.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedExtraHighEnd { get; set; } = "The wind lightens to a loud howl.";
        #endregion Weather High Points

        #region Weather Low Points
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationLowBegin { get; set; } = "The weather has become dry.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationLowEnd { get; set; } = "The weather is no longer arid.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationExtraLowBegin { get; set; } = "The weather has become dry enough for fires to start.";
        [ExcludeFromCodeCoverage]
        public string WorldPrecipitationExtraLowEnd { get; set; } = "The fear of wild fires have subsided.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedLowBegin { get; set; } = "The wind slows down and become still.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedLowEnd { get; set; } = "The wind picks up to a nice breeze.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedExtraLowBegin { get; set; } = "The wind has completely stopped.";
        [ExcludeFromCodeCoverage]
        public string WorldWindSpeedExtraLowEnd { get; set; } = "The wind has started to move again.";
        #endregion Weather Low Points

        [ExcludeFromCodeCoverage]
        public int HighBegin { get; set; } = 74;
        [ExcludeFromCodeCoverage]
        public int ExtraHighBegin { get; set; } = 89;
        [ExcludeFromCodeCoverage]
        public int HighEnd { get; set; } = 73;
        [ExcludeFromCodeCoverage]
        public int ExtraHighEnd { get; set; } = 88;
        [ExcludeFromCodeCoverage]
        public int LowBegin { get; set; } = 25;
        [ExcludeFromCodeCoverage]
        public int ExtraLowBegin { get; set; } = 10;
        [ExcludeFromCodeCoverage]
        public int LowEnd { get; set; } = 26;
        [ExcludeFromCodeCoverage]
        public int ExtraLowEnd { get; set; } = 11;

        [ExcludeFromCodeCoverage]
        public Dictionary<int, string> WeatherTriggers { get; set; }

        private void UpdateWeather()
        {
            if (GlobalReference.GlobalValues.TickCounter % 4 == 0)
            {
                bool precipitatonDirectionUp = true;
                bool windSpeedDirectionUp = true;

                NotifyPrecipitation = false;
                NotifyWindSpeed = false;

                if (Precipitation == PrecipitationGoal)
                {
                    PrecipitationGoal = GlobalReference.GlobalValues.Random.Next(100);
                }

                if (WindSpeed == WindSpeedGoal)
                {
                    WindSpeedGoal = GlobalReference.GlobalValues.Random.Next(100);
                }

                if (PrecipitationGoal > Precipitation)
                {
                    Precipitation++;
                }
                else if (PrecipitationGoal < Precipitation)
                {
                    Precipitation--;
                    precipitatonDirectionUp = false;
                }

                if (WeatherTriggers.Keys.Contains(Precipitation))
                {
                    if ((precipitatonDirectionUp && NotifyUp.Contains(Precipitation))
                        || (!precipitatonDirectionUp && NotifyDown.Contains(Precipitation)))
                    {
                        NotifyPrecipitation = true;
                    }
                }

                if (WindSpeedGoal > WindSpeed)
                {
                    WindSpeed++;
                }
                else if (WindSpeedGoal < WindSpeed)
                {
                    WindSpeed--;
                    windSpeedDirectionUp = false;
                }

                if (WeatherTriggers.Keys.Contains(WindSpeed))
                {
                    if ((windSpeedDirectionUp && NotifyUp.Contains(WindSpeed))
                        || (!windSpeedDirectionUp && NotifyDown.Contains(WindSpeed)))

                    {
                        NotifyWindSpeed = true;
                    }
                }
            }
        }
        #endregion Weather

        #region WorldCommands
        [ExcludeFromCodeCoverage]
        public ConcurrentQueue<string> WorldCommands { get; } = new ConcurrentQueue<string>();
        [ExcludeFromCodeCoverage]
        public ConcurrentDictionary<string, string> WorldResults { get; } = new ConcurrentDictionary<string, string>();

        private void DoWorldCommands()
        {
            string command = null;
            while (WorldCommands.TryDequeue(out command))
            {
                switch (command)
                {
                    case "GameStats":
                        GenerateGameStats();
                        break;
                }
            }
        }


        private void GenerateGameStats()
        {
            string result = GlobalReference.GlobalValues.GameStats.GenerateGameStats();

            WorldResults.AddOrUpdate("GameStats", result, (k, v) => result);
        }

        #endregion WorldCommands

        #region Zone
        [ExcludeFromCodeCoverage]
        private Dictionary<int, string> _zoneIdToFileMap { get; } = new Dictionary<int, string>();

        [ExcludeFromCodeCoverage]
        public Dictionary<int, IZone> Zones { get; } = new Dictionary<int, IZone>();

        public void LoadWorld()
        {
            string zoneLocation = GlobalReference.GlobalValues.Settings.ZoneDirectory;
            GlobalReference.GlobalValues.FileIO.EnsureDirectoryExists(zoneLocation);
            string[] zones = GlobalReference.GlobalValues.FileIO.GetFilesFromDirectory(zoneLocation, "*.zone");

            if (zones.Length == 0)
            {
                throw new FileNotFoundException("No zone files found in " + zoneLocation);
            }

            foreach (string file in zones)
            {
                string filePath = Path.GetFullPath(file);

                GlobalReference.GlobalValues.Logger.Log(LogLevel.DEBUG, "Loading " + filePath);

                IZone zone = DeserializeZone(GlobalReference.GlobalValues.FileIO.ReadAllText(filePath));
                zone.FinishLoad();
                Zones.Add(zone.Id, zone);
                _zoneIdToFileMap.Add(zone.Id, file);
            }

            _zones = new List<IZone>(Zones.Values);
            _zonePartitioner = Partitioner.Create(0, _zones.Count);
        }

        public IZone DeserializeZone(string serializedZone)
        {
            IZone zone = GlobalReference.GlobalValues.Serialization.Deserialize<Zone.Zone>(serializedZone);

            GlobalReference.GlobalValues.Logger.Log(zone, LogLevel.DEBUG, "Loading Zone");

            if (zone == null)
            {
                string message = "Unable to deserialize string as Zone." + Environment.NewLine + serializedZone;
                GlobalReference.GlobalValues.Logger.Log(LogLevel.ERROR, message);
                throw new Exception(message);
            }

            AddRoomToMobs(zone);

            return zone;
        }

        private void AddRoomToMobs(IZone zone)
        {
            foreach (IRoom room in zone.Rooms.Values)
            {
                foreach (IMobileObject mob in room.NonPlayerCharacters)
                {
                    mob.Room = room;
                }
            }
        }

        public void SaveWorld()
        {
            string zoneDirectory = GlobalReference.GlobalValues.Settings.ZoneDirectory;
            foreach (IZone zone in Zones.Values)
            {
                GlobalReference.GlobalValues.FileIO.WriteFile(Path.Combine(zoneDirectory, zone.Name + ".zone"), SerializeZone(zone));
            }
        }

        public string SerializeZone(IZone zone)
        {
            foreach (IRoom room in zone.Rooms.Values)
            {
                foreach (IMobileObject mob in room.NonPlayerCharacters)
                {
                    mob.Room = null;
                }

                foreach (IPlayerCharacter pc in room.PlayerCharacters)
                {
                    room.RemoveMobileObjectFromRoom(pc);
                }
            }

            return GlobalReference.GlobalValues.Serialization.Serialize(zone);
        }

        private void ReloadZones()
        {
            if (_lastZoneReload != GlobalReference.GlobalValues.GameDateTime.GameDateTime.Day)
            {
                lock (_zoneRefreshPadlock)
                {
                    if (_lastZoneReload != GlobalReference.GlobalValues.GameDateTime.GameDateTime.Day)
                    {
                        _lastZoneReload = GlobalReference.GlobalValues.GameDateTime.GameDateTime.Day;

                        //we can not load the zones into the list while iterating over the list
                        //so we create a list of zones that will be reloaded
                        List<Tuple<int, IZone>> zonesToReload = new List<Tuple<int, IZone>>();

                        foreach (IZone zone in Zones.Values)
                        {
                            if (zone.ResetTime.IsLessThan(GlobalReference.GlobalValues.GameDateTime.GameDateTime))
                            {
                                string filePath = _zoneIdToFileMap[zone.Id];
                                GlobalReference.GlobalValues.Logger.Log(LogLevel.DEBUG, "Reloading " + filePath);

                                IZone newZone = DeserializeZone(GlobalReference.GlobalValues.FileIO.ReadAllText(filePath));
                                newZone.FinishLoad();
                                zonesToReload.Add(new Tuple<int, IZone>(newZone.Id, newZone));
                            }
                        }

                        //now we update the zones that will be reset
                        foreach (Tuple<int, IZone> zone in zonesToReload)
                        {
                            Zones[zone.Item1] = zone.Item2;
                        }
                    }
                }
            }
        }
        #endregion Zone

        #region PlayerCharacters
        [ExcludeFromCodeCoverage]
        public ConcurrentQueue<IPlayerCharacter> AddPlayerQueue { get; } = new ConcurrentQueue<IPlayerCharacter>();

        [ExcludeFromCodeCoverage]
        private object CharacterLock { get; } = new object();

        [ExcludeFromCodeCoverage]
        private List<IPlayerCharacter> Characters { get; } = new List<IPlayerCharacter>();

        public List<IPlayerCharacter> CurrentPlayers
        {
            get
            {
                lock (CharacterLock)
                {
                    return new List<IPlayerCharacter>(Characters);
                }
            }
        }

        public IPlayerCharacter LoadCharacter(string name)
        {
            lock (CharacterLock)
            {
                //if player is already in game
                foreach (IPlayerCharacter character in Characters)
                {
                    if (character.Name.ToUpper() == name.ToUpper())
                    {
                        return character;
                    }
                }
            }

            string playerCharacterDir = GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory;
            string[] players = SavedPlayers(playerCharacterDir);

            //player not in game, try the directory
            foreach (string file in players)
            {
                if (Path.GetFileNameWithoutExtension(file).ToUpper() == name.ToUpper())
                {
                    IPlayerCharacter character = DeserializePlayerCharacter(GlobalReference.GlobalValues.FileIO.ReadAllText(file));
                    character.FinishLoad();
                    return character;
                }
            }

            //we could not find the player, return null
            return null;
        }

        private string[] SavedPlayers(string playerCharacterDir)
        {
            GlobalReference.GlobalValues.FileIO.EnsureDirectoryExists(playerCharacterDir);

            return GlobalReference.GlobalValues.FileIO.GetFilesFromDirectory(playerCharacterDir);
        }

        private IPlayerCharacter DeserializePlayerCharacter(string serializedPlayerCharcter)
        {
            IPlayerCharacter pc = GlobalReference.GlobalValues.Serialization.Deserialize<PlayerCharacter>(serializedPlayerCharcter);

            if (pc == null)
            {
                string message = "Unable to deserialize string as PlayerCharacter." + Environment.NewLine + serializedPlayerCharcter;
                GlobalReference.GlobalValues.Logger.Log(LogLevel.ERROR, message);
                throw new Exception(message);
            }

            return pc;
        }

        public void LogOutCharacter(IPlayerCharacter playerCharacter)
        {
            lock (CharacterLock)
            {
                SaveCharcter(playerCharacter);
                Characters.Remove(playerCharacter);
                playerCharacter.Room?.RemoveMobileObjectFromRoom(playerCharacter);
                Dismount(playerCharacter);
                lock (_loadedMounts)
                {
                    _loadedMounts.Remove(playerCharacter.Mount);
                }
            }
        }

        public void SaveCharcter(IPlayerCharacter character)
        {
            IRoom characterRoom = character.Room;
            string fileName = Path.Combine(GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory, character.Name + ".char");

            GlobalReference.GlobalValues.FileIO.WriteFile(fileName, SerializePlayerCharacter(character));

            character.Room = characterRoom;
        }

        public string SerializePlayerCharacter(IPlayerCharacter character)
        {
            character.Room = null;
            IMount mount = character.Mount;
            IRoom mountRoom = null;
            List<IMobileObject> riders = null;
            if (mount != null)
            {
                riders = mount.Riders;
                mountRoom = mount.Room;
                mount.Riders = null;
                mount.Room = null;
            }

            string result = GlobalReference.GlobalValues.Serialization.Serialize(character);

            if (mount != null)
            {
                character.Mount.Riders = riders;
                mount.Room = mountRoom;
            }

            return result;
        }

        public IPlayerCharacter CreateCharacter(string userName, string password)
        {
            IPlayerCharacter pc = new PlayerCharacter();
            pc.Name = userName;
            pc.Password = password;
            pc.StrengthStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.DexterityStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.ConstitutionStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.IntelligenceStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.WisdomStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.CharismaStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
            pc.LevelPoints = GlobalReference.GlobalValues.Settings.AssignableStatPoints;
            pc.Level = 1;
            pc.Health = pc.MaxHealth;
            pc.Stamina = pc.MaxStamina;
            pc.Mana = pc.MaxStamina;
            pc.SentenceDescription = pc.Name;
            pc.ShortDescription = pc.Name;
            pc.LookDescription = pc.Name;
            pc.KeyWords.Add(pc.Name);
            pc.GuildPoints = 1;

            AddPlayerQueue.Enqueue(pc);

            return pc;
        }
        #endregion PlayerCharacters

        public void PerformTick()
        {
            lock (_tickPadlock)
            {
                PerformCombatTick();
                PutPlayersIntoWorld();
                UpdateWeather();
                ReloadZones();
                UpdatePerformanceCounters();
                AutoSaveCharacters();

#if DEBUG
                foreach (IZone zone in Zones.Values)
                {
                    ProcessRooms(zone);
                }
#else
                Parallel.ForEach(_zonePartitioner, (range, loopState) =>
                {
                    // Loop over each range element without a delegate invocation.
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        IZone zone = _zones[i];
                        ProcessRooms(zone);
                    }
                });
#endif

                CatchPlayersOutSideOfTheWorldDueToReloadedZones();

                NotifyPrecipitation = false;
                NotifyWindSpeed = false;

                ProcessSerialCommands();

                DoWorldCommands();

                GlobalReference.GlobalValues.Logger.FlushLogs();
            }
        }





        #region Follow Methods
        private void ProcessFollowMobs()
        {
            while (_followMobQueue.Count > 0)
            {
                if (_followMobQueue.TryDequeue(out IMobileObject performer))
                {
                    HashSet<IRoom> searchedRooms = new HashSet<IRoom>();
                    //double check the room in case the follow target moved into the same room as the follower;
                    if (performer.Room != performer.FollowTarget.Room)
                    {
                        searchedRooms.Add(performer.Room);
                        SearchOtherRooms(performer, searchedRooms);
                    }
                }
            }
        }

        private void SearchOtherRooms(IMobileObject performer, HashSet<IRoom> searchedRooms)
        {
            Queue<Trail> newTrails = new Queue<Trail>();
            IRoom currentRoom = performer.Room;

            Trail trail = null;
            foreach (Directions.Direction direction in Enum.GetValues(typeof(Directions.Direction)))
            {
                Trail brandNewTrail = new Trail() { Direction = direction, Distance = 0 };
                trail = LookForMobInNextRoom(performer, searchedRooms, newTrails, currentRoom, direction, brandNewTrail);
                if (trail != null)
                {
                    performer.EnqueueCommand(trail.Direction.ToString());
                    return;
                }
            }

            while (newTrails.Count > 0)
            {
                Trail dequeuedTrail = newTrails.Dequeue();
                foreach (Directions.Direction direction in Enum.GetValues(typeof(Directions.Direction)))
                {
                    trail = LookForMobInNextRoom(performer, searchedRooms, newTrails, dequeuedTrail.Room, direction, dequeuedTrail);
                    if (trail != null)
                    {
                        performer.EnqueueCommand(trail.Direction.ToString());
                        return;
                    }
                }
            }

            GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage($"You have lost track of the {performer.FollowTarget.SentenceDescription} and had to quit following them."));
            performer.FollowTarget = null;
        }

        private Trail LookForMobInNextRoom(IMobileObject performer, HashSet<IRoom> searchedRooms, Queue<Trail> newTrails, IRoom currentRoom, Directions.Direction direction, Trail existingTrail)
        {
            Trail trail = null;
            switch (direction)
            {
                case Directions.Direction.North:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.North, performer, newTrails, existingTrail);
                    break;
                case Directions.Direction.East:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.East, performer, newTrails, existingTrail);
                    break;
                case Directions.Direction.South:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.South, performer, newTrails, existingTrail);
                    break;
                case Directions.Direction.West:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.West, performer, newTrails, existingTrail);
                    break;
                case Directions.Direction.Up:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.Up, performer, newTrails, existingTrail);
                    break;
                case Directions.Direction.Down:
                    trail = AddNextRoom(searchedRooms, direction, currentRoom.Down, performer, newTrails, existingTrail);
                    break;
            }

            return trail;
        }

        private Trail AddNextRoom(HashSet<IRoom> searchedRooms, Directions.Direction direction, IExit exit, IMobileObject performer, Queue<Trail> newTrails, Trail trail)
        {
            if (trail.Distance > 3) //don't follow someone that has gotten to far away to follow
            {
                return null;
            }

            if (exit != null)
            {
                IRoom nextRoom = Zones[exit.Zone].Rooms[exit.Room];

                if (searchedRooms.Contains(nextRoom))
                {
                    return null;
                }

                trail.Distance++;
                IMobileObject foundMob = FindMobInRoom(nextRoom, performer);
                if (foundMob != null)
                {
                    return trail;
                }

                if (!searchedRooms.Contains(nextRoom))
                {
                    trail.Room = nextRoom;
                    newTrails.Enqueue(trail);
                    searchedRooms.Add(nextRoom);
                }
            }

            return null;
        }

        private IMobileObject FindMobInRoom(IRoom roomToSearch, IMobileObject performer)
        {
            if (roomToSearch.NonPlayerCharacters.Contains(performer.FollowTarget)
                || roomToSearch.PlayerCharacters.Contains(performer.FollowTarget))
            {
                return performer.FollowTarget;
            }
            else
            {
                return null;
            }
        }

        #endregion Follow Methods

        #region Tick Methods
        #region Mounts
        private void MoveMounts()
        {
            foreach (IMount mount in _loadedMounts)
            {
                for (int i = 0; i < mount.Movement; i++)
                {
                    if (mount.CommmandQueueCount > 0)
                    {
                        ProcessMobCommand(mount);
                    }
                    else
                    {
                        break;
                    }
                }

                if (RegerateTick)
                {
                    MobRegenerate(mount);
                }
            }
        }

        public void AddMountToWorld(IMount mount)
        {
            lock (_loadedMounts)
            {
                _loadedMounts.Add(mount);
            }
        }

        public void RemoveMountFromWorld(IMount mount)
        {
            lock (_loadedMounts)
            {
                _loadedMounts.Remove(mount);
            }
        }

        #endregion Mounts

        private void AutoSaveCharacters()
        {
            if (DateTime.UtcNow.Subtract(_lastSave).TotalMinutes >= 15)
            {
                foreach (IPlayerCharacter pc in CurrentPlayers)
                {
                    SaveCharcter(pc);
                }

                _lastSave = DateTime.UtcNow;
            }
        }

        private void ProcessSerialCommands()
        {
            ProcessFollowMobs();
            MoveMounts();
        }

        private void UpdatePerformanceCounters()
        {
            if (GlobalReference.GlobalValues.Settings.LogStats)
            {
                Counters localCounter = new Counters();

                //we are fine hitting the raw values instead of the property because we are not threading yet
                localCounter.ConnnectedPlayers = Characters.Count;
                localCounter.CPU = (decimal)Math.Round(GlobalReference.GlobalValues.TickTimes.MedianTime, 2);
                localCounter.MaxTickTimeInMs = (int)Math.Round(GlobalReference.GlobalValues.TickTimes.MaxTime);

                float memoryConsumption = Process.GetCurrentProcess().WorkingSet64 / (1024f * 1024f);
                localCounter.Memory = (decimal)Math.Round(memoryConsumption, 2);


#if DEBUG
                foreach (IZone zone in Zones.Values)
                {
                    foreach (IRoom room in zone.Rooms.Values)
                    {
                        foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                        {
                            if (npc is IElemental)
                            {
                                localCounter.Elementals++;
                            }
                        }
                    }
                }
#else
                Parallel.ForEach(Zones.Values, zone =>
                {
                    foreach (IRoom room in zone.Rooms.Values)
                    {
                        foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                        {
                            if (npc is IElemental)
                            {
                                lock (localCounter)
                                {
                                    localCounter.Elementals++;
                                }
                            }
                        }
                    }
                });
#endif


                GlobalReference.GlobalValues.Counters = localCounter;
                GlobalReference.GlobalValues.CountersLog.Add(localCounter);

                //fail safe to not to run out of memory
                while (GlobalReference.GlobalValues.CountersLog.Count > 1000) //5 min at 2x second is 600 so plenty of space
                {
                    GlobalReference.GlobalValues.CountersLog.RemoveAt(0);
                }

                DateTime currentTime = DateTime.UtcNow;
                if (_lastLogMinute != currentTime.Minute
                    && currentTime.Minute % 5 == 0) //only write logs every 5 minutes
                {
                    WriteLogStats(currentTime);
                    _lastLogMinute = currentTime.Minute;
                    GlobalReference.GlobalValues.CountersLog.Clear();
                }
            }
        }

        private void WriteLogStats(DateTime dateTime)
        {
            try
            {
                GlobalReference.GlobalValues.FileIO.EnsureDirectoryExists(Path.Combine(GlobalReference.GlobalValues.Settings.LogStatsLocation, dateTime.ToString("yyyyMMdd")));
                string fileLocation = Path.Combine(GlobalReference.GlobalValues.Settings.LogStatsLocation, dateTime.ToString("yyyyMMdd"), "Stats.stat");

                List<ICounters> counters = new List<ICounters>();
                if (GlobalReference.GlobalValues.FileIO.Exists(fileLocation))
                {
                    counters = GlobalReference.GlobalValues.Serialization.Deserialize<List<ICounters>>(GlobalReference.GlobalValues.FileIO.ReadAllText(fileLocation));
                }
                counters.AddRange(GlobalReference.GlobalValues.CountersLog);

                string fileContents = GlobalReference.GlobalValues.Serialization.Serialize(counters);
                GlobalReference.GlobalValues.FileIO.WriteFile(fileLocation, fileContents);
            }
            catch (Exception ex)
            {
                GlobalReference.GlobalValues.Logger.Log(LogLevel.ERROR, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void PerformCombatTick()
        {
            GlobalReference.GlobalValues.Engine.Combat.ProcessCombatRound();
        }

        private void PutPlayersIntoWorld()
        {
            IPlayerCharacter pc = null;
            while (AddPlayerQueue.TryDequeue(out pc))
            {
                if (pc.Room != null)
                {
                    pc.Room.AddMobileObjectToRoom(pc);
                }
                else if (pc.RoomId != null)
                {
                    try
                    {
                        pc.Room = Zones[pc.RoomId.Zone].Rooms[pc.RoomId.Id];
                    }
                    catch
                    {
                        //for some reason the room does not exist.
                        //Put the player in the default start room;
                        pc.Room = Zones[1].Rooms[1];
                    }
                    pc.Room.AddMobileObjectToRoom(pc);
                }
                //first time loading the character
                else
                {
                    IRoom startRoom = Zones[1].Rooms[1];
                    pc.Room = startRoom;
                    startRoom.Enter(pc);
                }

                lock (CharacterLock)
                {
                    Characters.Add(pc);
                }
                pc.EnqueueCommand("Look");


                //if the player just started they should not be mounted
                Dismount(pc);
            }


        }

        private void ProcessRooms(IZone zone)
        {
            foreach (IRoom room in zone.Rooms.Values)
            {
                ProcessRoom(room);
            }
        }

        private void ProcessRoom(IRoom room)
        {
            PerformHeartBeatBigTick(room);

            bool notifyWeather = false;
            if ((NotifyPrecipitation || NotifyWindSpeed)
                && room.Attributes.Contains(Room.Room.RoomAttribute.Weather))
            {
                notifyWeather = true;
            }

            foreach (IMobileObject mob in room.NonPlayerCharacters)
            {
                ProcessCommonMobStuff(room, notifyWeather, mob);
            }

            foreach (IMobileObject mob in room.PlayerCharacters)
            {
                ProcessCommonMobStuff(room, notifyWeather, mob);
            }

            SpawnElementals(room);
        }

        private void SpawnElementals(IRoom room)
        {
            //don't spawn indoors or outdoors where no elementals can spawn
            if (!room.Attributes.Contains(RoomAttribute.NoSpawnElemental)
                && !room.Attributes.Contains(RoomAttribute.Indoor))
            {
                if (GlobalReference.GlobalValues.Random.PercentDiceRoll(GlobalReference.GlobalValues.Settings.ElementalSpawnPercent))
                {
                    if (Precipitation >= 75)
                    {
                        SpawnElemental(room, ElementType.Water);
                    }

                    if (Precipitation <= 25)
                    {
                        SpawnElemental(room, ElementType.Fire);
                    }

                    if (WindSpeed >= 75)
                    {
                        SpawnElemental(room, ElementType.Air);
                    }

                    if (WindSpeed <= 25)
                    {
                        SpawnElemental(room, ElementType.Earth);
                    }
                }
            }
        }

        private static void SpawnElemental(IRoom room, ElementType elementType)
        {
            Elemental elemental = new Elemental(elementType);
            elemental.FinishLoad();
            room.AddMobileObjectToRoom(elemental);
            elemental.Room = room;
            ITranslationMessage translationMessage = new TranslationMessage($"A {elemental.KeyWords[0]} elemental appears out of nothing.");
            GlobalReference.GlobalValues.Notify.Room(elemental, null, room, translationMessage, null, true);
        }

        private void PerformHeartBeatBigTick(IBaseObject obj)
        {
            GlobalReference.GlobalValues.Engine.Event.HeartbeatBigTick(obj);
        }

        private void ProcessCommonMobStuff(IRoom room, bool notifyWeather, IMobileObject mob)
        {
            //only process 1 command per tick
            if (mob.LastProccessedTick != GlobalReference.GlobalValues.TickCounter)
            {
                mob.LastProccessedTick = GlobalReference.GlobalValues.TickCounter;
                ProcessPlayerNotifications(mob);
                ProcessMobCommunication(mob);
                ProcessEnchantments(room);
                AddToFollowQueueIfNeeded(mob);
                ProcessMobPersonality(mob); //not needed for pc but will not run because pc will not cast as npc
                ProcessMobCommand(mob);
                PerformHeartBeatBigTick(mob);
                if (notifyWeather)
                {
                    if (NotifyPrecipitation)
                    {
                        ProcessPrecipitation(mob, room);
                    }
                    if (NotifyWindSpeed)
                    {
                        ProcessWindSpeed(mob, room);
                    }
                }

                CleanupEnchantments(mob);

                if (RegerateTick)
                {
                    MobRegenerate(mob);
                }
            }
        }

        private void CleanupEnchantments(IMobileObject mob)
        {
            if (mob.Enchantments.Count > 0)
            {
                for (int i = mob.Enchantments.Count; i > 0; i--)
                {
                    IEnchantment enchantment = mob.Enchantments[i - 1];
                    if (enchantment.EnchantmentEndingDateTime <= DateTime.UtcNow)
                    {
                        mob.Enchantments.RemoveAt(i - 1);
                    }
                    else if (enchantment.DefeatInfo != null)
                    {
                        if (enchantment.DefeatInfo.DoesPayerDefeatEnchantment(mob))
                        {
                            mob.Enchantments.RemoveAt(i - 1);
                        }
                    }
                }
            }
        }

        private void AddToFollowQueueIfNeeded(IMobileObject mob)
        {
            if (mob.FollowTarget != null && mob.CommmandQueueCount == 0)
            {
                if (mob.FollowTarget.Health > 0)
                {
                    //the follow target is alive
                    if (mob.FollowTarget.Room == mob.Room)
                    {
                        //the follow target is in the same room, nothing to do
                        return;
                    }
                    else
                    {
                        //the follow target is alive but in another room, add this to the list of mobs to process later
                        _followMobQueue.Enqueue(mob);
                    }
                }
                else
                {
                    //the follow target died, remove it and notify the mob
                    GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage($"{mob.FollowTarget.SentenceDescription} died and you quit following them."));
                    mob.FollowTarget = null;
                }
            }
        }

        private void ProcessPlayerNotifications(IMobileObject mob)
        {
            IPlayerCharacter pc = mob as IPlayerCharacter;
            if (pc != null)
            {
                foreach (ICraftsmanObject item in pc.CraftsmanObjects)
                {
                    if (item.Completion < DateTime.Now
                        && item.NextNotifcation < DateTime.Now)
                    {
                        GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage($"\"{item.CraftmanDescripition}\" has completed your item in zone {item.CraftsmanId.Zone}."));
                        item.NextNotifcation = DateTime.Now.AddMinutes(5);
                    }
                }
            }
        }

        private static void ProcessEnchantments(IRoom room)
        {
            if (room.Enchantments.Count > 0)
            {
                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    foreach (IEnchantment enchantment in room.Enchantments)
                    {
                        enchantment.HeartbeatBigTick(npc);
                    }
                }
            }

            if (room.PlayerCharacters.Count > 0)
            {
                foreach (IPlayerCharacter pc in room.PlayerCharacters)
                {
                    foreach (IEnchantment enchantment in room.Enchantments)
                    {
                        //don't fire expired enchantments
                        //work around because we can't clear the enchantments if on kills the player with out throwing an error
                        if (enchantment.EnchantmentEndingDateTime > DateTime.UtcNow)
                        {
                            enchantment.HeartbeatBigTick(pc);
                        }
                    }
                }
            }

            foreach (IEnchantment enchantment in room.Enchantments)
            {
                enchantment.HeartbeatBigTick(room);
            }
        }

        private void MobRegenerate(IMobileObject mob)
        {
            int healthGainDivisor; //standing ~ 16.66 minute heal
            switch (mob.Position)
            {
                case CharacterPosition.Sleep:   //4x heal
                    healthGainDivisor = 25;
                    break;
                case CharacterPosition.Relax:   //3x heal
                    healthGainDivisor = 33;
                    break;
                case CharacterPosition.Sit:     //2x heal
                    healthGainDivisor = 50;
                    break;
                case CharacterPosition.Mounted: //1x heal
                case CharacterPosition.Stand:
                default:
                    healthGainDivisor = 100;
                    break;
            }

            mob.Health += Math.Max(mob.MaxHealth / healthGainDivisor, 1);
            mob.Mana += Math.Max(mob.MaxMana / healthGainDivisor, 1);
            mob.Stamina += Math.Max(mob.MaxStamina / healthGainDivisor, 1);

            mob.Health = Math.Min(mob.MaxHealth, mob.Health);
            mob.Mana = Math.Min(mob.MaxMana, mob.Mana);
            mob.Stamina = Math.Min(mob.MaxStamina, mob.Stamina);
        }

        private void ProcessPrecipitation(IMobileObject mob, IRoom room)
        {
            GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(room.PrecipitationNotification));
        }

        private void ProcessWindSpeed(IMobileObject mob, IRoom room)
        {
            GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(room.WindSpeedNotification));
        }

        private void ProcessMobCommunication(IMobileObject mob)
        {
            IResult result = null;
            while (result == null || result.AllowAnotherCommand == true)
            {
                string communication = mob.DequeueCommunication();
                if (communication != null)
                {
                    ICommand command = GlobalReference.GlobalValues.Parser.Parse(communication);
                    if (command != null)
                    {
                        IMobileObjectCommand mobCommand = GlobalReference.GlobalValues.CommandList.GetCommand(mob, command.CommandName);

                        if (mobCommand == null)
                        {
                            result = new Result("Unknown command.", true);
                        }
                        else
                        {
                            result = mobCommand.PerformCommand(mob, command);
                        }
                        GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(result.ResultMessage));
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void ProcessMobPersonality(IMobileObject mob)
        {
            INonPlayerCharacter npc = mob as INonPlayerCharacter;
            if (npc != null && npc.PossingMob == null)  //don't process the npc personalities if possessed
            {
                string command = null;
                foreach (IPersonality personality in npc.Personalities)
                {
                    command = personality.Process(npc, command);
                }

                if (command != null)
                {
                    npc.EnqueueCommand(command);
                }
            }
        }

        private void ProcessMobCommand(IMobileObject mob)
        {
            IResult result = null;
            while (result == null || result.AllowAnotherCommand == true)
            {
                string stringCommand = mob.DequeueCommand();
                if (stringCommand != null)
                {
                    ICommand command = GlobalReference.GlobalValues.Parser.Parse(stringCommand);
                    if (command != null
                        && !string.IsNullOrEmpty(command.CommandName))
                    {
                        IMobileObjectCommand mobCommand = GlobalReference.GlobalValues.CommandList.GetCommand(mob, command.CommandName);
                        if (mobCommand != null)
                        {
                            result = mobCommand.PerformCommand(mob, command);
                        }
                        else
                        {
                            string message = $@"Unable to figure out how to {command.CommandName}.
To see a list of all commands type MAN.
To see info on how to use a command type MAN and then the COMMAND.";
                            result = new Result(message, true);
                        }

                        if (result != null
                            && result.ResultMessage != null)
                        {
                            GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(result.ResultMessage, null));
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void CatchPlayersOutSideOfTheWorldDueToReloadedZones()
        {
            //there is no reason to lock this as we lock on the tick 
            //so we will never have more than 1 running at once.

            foreach (IPlayerCharacter pc in Characters)
            {
                IRoom pcRoom = pc.Room;
                if (pcRoom != null
                    && pc.LastProccessedTick != GlobalReference.GlobalValues.TickCounter
                    && pcRoom != Zones[pcRoom.Zone].Rooms[pcRoom.Id])
                {
                    ProcessRoom(pc.Room);
                }
            }
        }

        public IResult Dismount(IMobileObject performer)
        {
            IMount mountTheyWereOn = null;
            lock (_loadedMounts)
            {
                foreach (IMount mount in _loadedMounts)
                {
                    if (mount.Riders.Remove(performer))
                    {
                        mountTheyWereOn = mount;
                    }
                }
            }

            if (mountTheyWereOn == null)
            {
                return new Result("You are not riding a mount.", true);
            }
            else
            {
                return new Result($"You dismount from the {mountTheyWereOn.SentenceDescription}.", true);
            }
        }


        #endregion Tick Methods
    }
}
