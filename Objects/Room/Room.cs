using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static Objects.Global.Direction.Directions;
using Objects.LoadPercentage.Interface;
using Objects.Trap.Interface;
using System.Collections.ObjectModel;
using Objects.Language;
using Shared.Sound.Interface;

namespace Objects.Room
{
    public class Room : BaseObject, IRoom, ILoadableItems
    {
        private static ReadOnlyCollection<INonPlayerCharacter> BlankNonPlayerCharacters { get; } = new List<INonPlayerCharacter>().AsReadOnly();
        private static ReadOnlyCollection<IPlayerCharacter> BlankPlayerCharacters { get; } = new List<IPlayerCharacter>().AsReadOnly();
        private static ReadOnlyCollection<IMobileObject> BlankMobs { get; } = new List<IMobileObject>().AsReadOnly();
        private static ReadOnlyCollection<IItem> BlankItems { get; } = new List<IItem>().AsReadOnly();

        public Room(string examineDescription, string lookDescription, string shortDescription) : base(examineDescription, lookDescription, "", shortDescription)
        {
            KeyWords.Add("room");

            RoomPrecipitationHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationHighBegin;
            RoomPrecipitationHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationHighEnd;
            RoomPrecipitationExtraHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraHighBegin;
            RoomPrecipitationExtraHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraHighEnd;
            RoomWindSpeedHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedHighBegin;
            RoomWindSpeedHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedHighEnd;
            RoomWindSpeedExtraHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraHighBegin;
            RoomWindSpeedExtraHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraHighEnd;
            RoomPrecipitationLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationLowBegin;
            RoomPrecipitationLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationLowEnd;
            RoomPrecipitationExtraLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraLowBegin;
            RoomPrecipitationExtraLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraLowEnd;
            RoomWindSpeedLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedLowBegin;
            RoomWindSpeedLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedLowEnd;
            RoomWindSpeedExtraLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraLowBegin;
            RoomWindSpeedExtraLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraLowEnd;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Zone, Id);
        }

        [ExcludeFromCodeCoverage]
        public int MovementCost { get; set; }

        private object _itemLock = new object();
        private List<IItem> _items = new List<IItem>();
        [ExcludeFromCodeCoverage]
        public IReadOnlyList<IItem> Items
        {
            get
            {
                lock (_itemLock)
                {
                    if (_items.Count == 0)
                    {
                        return BlankItems;  //save memory allocations when returning a blank list
                    }
                    else
                    {
                        return new List<IItem>(_items).AsReadOnly();
                    }
                }
            }
            set
            {
                lock (_items)
                {
                    _items = new List<IItem>(value);
                }
            }
        }


        [ExcludeFromCodeCoverage]
        public List<ITrap> Traps { get; } = new List<ITrap>();

        [ExcludeFromCodeCoverage]
        public List<ILoadPercentage> LoadableItems { get; } = new List<ILoadPercentage>();

        private object _nonPlayerCharactersLock = new object();
        private List<INonPlayerCharacter> _nonPlayerCharacters = new List<INonPlayerCharacter>();
        [ExcludeFromCodeCoverage]
        public IReadOnlyList<INonPlayerCharacter> NonPlayerCharacters
        {
            get
            {
                lock (_nonPlayerCharactersLock)
                {
                    if (_nonPlayerCharacters.Count == 0)
                    {
                        return BlankNonPlayerCharacters;  //save memory allocations when returning a blank list
                    }
                    else
                    {
                        return new List<INonPlayerCharacter>(_nonPlayerCharacters).AsReadOnly();
                    }
                }
            }
            set //used during deserialization
            {
                lock (_nonPlayerCharactersLock)
                {
                    _nonPlayerCharacters = new List<INonPlayerCharacter>(value);
                }
            }
        }

        private object _playerCharactersLock = new object();
        private List<IPlayerCharacter> _playerCharacters = new List<IPlayerCharacter>();
        [ExcludeFromCodeCoverage]
        public IReadOnlyList<IPlayerCharacter> PlayerCharacters
        {
            get
            {
                lock (_playerCharactersLock)
                {
                    if (_playerCharacters.Count == 0)
                    {
                        return BlankPlayerCharacters;   //save memory allocations when returning a blank list
                    }
                    else
                    {
                        return new List<IPlayerCharacter>(_playerCharacters).AsReadOnly();
                    }
                }
            }

            set //used during deserialization
            {
                lock (_playerCharactersLock)
                {
                    _playerCharacters = new List<IPlayerCharacter>(value);
                }
            }
        }

        private object _otherMobsLock = new object();
        private List<IMobileObject> _otherMobs = new List<IMobileObject>();
        [ExcludeFromCodeCoverage]
        public IReadOnlyList<IMobileObject> OtherMobs
        {
            get
            {
                lock (_otherMobsLock)
                {
                    if (_otherMobs.Count == 0)
                    {
                        return BlankMobs;   //save memory allocations when returning a blank list
                    }
                    else
                    {
                        return new List<IMobileObject>(_otherMobs).AsReadOnly();
                    }
                }
            }

            set //used during deserialization
            {
                lock (_otherMobsLock)
                {
                    _otherMobs = new List<IMobileObject>(value);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public string? Owner { get; set; }
        [ExcludeFromCodeCoverage]
        public HashSet<string> Guests { get; set; } = new HashSet<string>();

        [ExcludeFromCodeCoverage]
        public List<ISound> Sounds { get; } = new List<ISound>();

        public string SerializedSounds
        {
            get
            {
                return GlobalReference.GlobalValues.Serialization.Serialize(Sounds);
            }
        }

        #region Exits
        [ExcludeFromCodeCoverage]
        public IExit? North { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit? East { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit? South { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit? West { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit? Up { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit? Down { get; set; }
        #endregion Exits

        public IResult? CheckEnter(IMobileObject mobileObject)
        {
            if (mobileObject is INonPlayerCharacter npc)
            {
                if (Attributes.Contains(RoomAttribute.Small))
                {
                    if (NonPlayerCharacters.Count != 0)
                    {
                        return new Result("The room is to small to fit another person in there.", true);
                    }
                }

                if (Attributes.Contains(RoomAttribute.NoNPC))
                {
                    return new Result("Non player characters can not enter here.", true);
                }
            }

            if (mobileObject is IPlayerCharacter pc)
            {
                if (Attributes.Contains(RoomAttribute.Small))
                {
                    if (PlayerCharacters.Count != 0)
                    {
                        return new Result("The room is to small to fit another person in there.", true);
                    }
                }
            }

            if (Owner != null)
            {
                if (Owner != mobileObject.KeyWords[0]
                    && !Guests.Contains(mobileObject.KeyWords[0]))
                {
                    return new Result($"That property belongs to {Owner} and you are not on the guest list.", true);
                }
            }

            return null;
        }

        public IResult? CheckLeave(IMobileObject mobileObject)
        {
            IResult result = CheckFlee(mobileObject);
            if (result != null)
            {
                return result;
            }

            if (mobileObject.IsInCombat)
            {
                //TODO Update this message when flee is implemented to mention fleeing.
                return new Result("You can not leave while your fighting for your life.", true);
            }

            return null;
        }

        public IResult? CheckFlee(IMobileObject mobileObject)
        {
            //mob does not have enough stamina to cover room movement cost
            //we want to make them fully recover so they are not stuck
            if ((mobileObject.MaxStamina < MovementCost
                && mobileObject.Stamina < mobileObject.MaxStamina))

            {
                return new Result("You need to rest before you attempt to leave.", true);
            }

            //move does not have enough stamina to leave, rest
            if (mobileObject.MaxStamina >= MovementCost
                && mobileObject.Stamina < MovementCost)
            {
                return new Result("You need to rest before you attempt to leave.", true);
            }

            return null;
        }

        public IResult? CheckLeaveDirection(IMobileObject mobileObject, Direction direction)
        {
            //check if there is a guard in the room blocking the direction the mob is trying to leave
            IRoom room = mobileObject.Room;
            foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
            {
                if (npc.Personalities.Count > 0)
                {
                    foreach (IPersonality personality in npc.Personalities)
                    {
                        IGuard guard = personality as IGuard;
                        if (guard != null)
                        {
                            if (guard.GuardDirections.Contains(direction))
                            {
                                return new Result(guard.BlockLeaveMessage, true);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void Enter(IMobileObject performer)
        {
            AddMobileObjectToRoom(performer);
            performer.Room = this;
            performer.RoomId = new RoomId(performer.Room);

            GlobalReference.GlobalValues.Engine.Event.EnterRoom(performer);


            //Announce the mob entering
            string capitalSentenceDesripton = GlobalReference.GlobalValues.StringManipulator.CapitalizeFirstLetter(performer.SentenceDescription);
            GlobalReference.GlobalValues.Notify.Room(performer, null, this, new TranslationMessage($"{capitalSentenceDesripton} enters the room."), null, true);
        }

        public void AddMobileObjectToRoom(IMobileObject mob)
        {
            if (mob is INonPlayerCharacter npc)
            {
                lock (_nonPlayerCharactersLock)
                {
                    _nonPlayerCharacters.Add(npc);
                }
                return;
            }

            if (mob is IPlayerCharacter pc)
            {
                lock (_playerCharactersLock)
                {
                    _playerCharacters.Add(pc);
                }
                return;
            }

            lock (_otherMobsLock)
            {
                _otherMobs.Add(mob);
            }
        }

        public void AddItemToRoom(IItem item, int position = int.MaxValue)
        {
            lock (_itemLock)
            {
                if (position == int.MaxValue)
                {
                    _items.Add(item);
                }
                else
                {
                    _items.Insert(position, item);
                }

                if (Attributes.Contains(RoomAttribute.Vault))
                {
                    SaveVault();
                }
            }
        }

        public bool Leave(IMobileObject performer, Direction direction, bool mounted)
        {
            if (!mounted)
            {
                performer.Stamina -= MovementCost;
            }

            GlobalReference.GlobalValues.Engine.Event.LeaveRoom(performer, direction);

            bool wasAbleToLeave = RemoveMobileObjectFromRoom(performer);

            if (wasAbleToLeave)
            {
                //Announce the mob leaving
                string capitalSentenceDesripton = GlobalReference.GlobalValues.StringManipulator.CapitalizeFirstLetter(performer.SentenceDescription);
                GlobalReference.GlobalValues.Notify.Room(performer, null, this, new TranslationMessage($"{capitalSentenceDesripton} leaves {direction}."), null, true);
            }

            return wasAbleToLeave;
        }

        public bool RemoveMobileObjectFromRoom(IMobileObject mob)
        {
            if (mob is INonPlayerCharacter npc)
            {
                lock (_nonPlayerCharactersLock)
                {
                    return _nonPlayerCharacters.Remove(npc);
                }
            }

            if (mob is IPlayerCharacter pc)
            {
                lock (_playerCharactersLock)
                {
                    return _playerCharacters.Remove(pc);
                }
            }

            lock (_otherMobs)
            {
                return _otherMobs.Remove(mob);
            }
        }

        public bool RemoveItemFromRoom(IItem item)
        {
            lock (_itemLock)
            {
                bool success = _items.Remove(item);
                if (Attributes.Contains(RoomAttribute.Vault) && success)
                {
                    SaveVault();
                }
                return success;
            }
        }

        private void SaveVault()
        {
            string serializedItems = GlobalReference.GlobalValues.Serialization.Serialize(_items);
            string file = Path.Combine(GlobalReference.GlobalValues.Settings.VaultDirectory, $"{Zone}-{Id}.vault");
            GlobalReference.GlobalValues.FileIO.WriteFile(file, serializedItems);
        }

        [ExcludeFromCodeCoverage]
        public HashSet<RoomAttribute> Attributes { get; } = new HashSet<RoomAttribute>();

        public enum RoomAttribute
        {
            Indoor,
            /// <summary>
            /// Npc will not wander into this room
            /// </summary>
            NoNPC,
            Outdoor,
            /// <summary>
            /// Only 1 pc in the room at a time
            /// </summary>
            Small,
            /// <summary>
            /// Unable to use recall to go to the pc home point
            /// </summary>
            NoRecall,
            /// <summary>
            /// Aggressive actions will be ignored in this room
            /// </summary>
            Peaceful,
            NoLight,
            Light,
            /// <summary>
            /// Characters will be notified of weather changes
            /// </summary>
            Weather,
            /// <summary>
            /// Tracking will not find paths through this room
            /// </summary>
            NoTrack,
            /// <summary>
            /// Room will save when items are gotten or dropped
            /// </summary>
            Vault,
            /// <summary>
            /// No Elementals will spawn due to weather
            /// </summary>
            NoSpawnElemental


        }

        #region Weather
        #region Weather Messages
        #region Weather High Points
        public string RoomPrecipitationHighBegin { get; set; }
        public string RoomPrecipitationHighEnd { get; set; }
        public string RoomPrecipitationExtraHighBegin { get; set; }
        public string RoomPrecipitationExtraHighEnd { get; set; }
        public string RoomWindSpeedHighBegin { get; set; }
        public string RoomWindSpeedHighEnd { get; set; }
        public string RoomWindSpeedExtraHighBegin { get; set; }
        public string RoomWindSpeedExtraHighEnd { get; set; }
        #endregion Weather High Points

        #region Weather Low Points
        public string RoomPrecipitationLowBegin { get; set; }
        public string RoomPrecipitationLowEnd { get; set; }
        public string RoomPrecipitationExtraLowBegin { get; set; }
        public string RoomPrecipitationExtraLowEnd { get; set; }
        public string RoomWindSpeedLowBegin { get; set; }
        public string RoomWindSpeedLowEnd { get; set; }
        public string RoomWindSpeedExtraLowBegin { get; set; }
        public string RoomWindSpeedExtraLowEnd { get; set; }
        #endregion Weather Low Points
        #endregion Weather Messages

        public string PrecipitationNotification
        {
            get
            {
                return GetWeatherMessage("Precipitation");
            }
        }

        public string WindSpeedNotification
        {
            get
            {
                return GetWeatherMessage("WindSpeed");
            }
        }

        private string? GetWeatherMessage(string weatherType)
        {
            string? weatherMessage = null;
            switch (weatherType)
            {
                case "Precipitation":
                    if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.HighBegin)
                    {
                        weatherMessage = RoomPrecipitationHighBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.ExtraHighBegin)
                    {
                        weatherMessage = RoomPrecipitationExtraHighBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.HighEnd)
                    {
                        weatherMessage = RoomPrecipitationHighEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.ExtraHighEnd)
                    {
                        weatherMessage = RoomPrecipitationExtraHighEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.LowBegin)
                    {
                        weatherMessage = RoomPrecipitationLowBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.ExtraLowBegin)
                    {
                        weatherMessage = RoomPrecipitationExtraLowBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.LowEnd)
                    {
                        weatherMessage = RoomPrecipitationLowEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.Precipitation == GlobalReference.GlobalValues.World.ExtraLowEnd)
                    {
                        weatherMessage = RoomPrecipitationExtraLowEnd;
                    }
                    break;
                case "WindSpeed":
                    if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.HighBegin)
                    {
                        weatherMessage = RoomWindSpeedHighBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.ExtraHighBegin)
                    {
                        weatherMessage = RoomWindSpeedExtraHighBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.HighEnd)
                    {
                        weatherMessage = RoomWindSpeedHighEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.ExtraHighEnd)
                    {
                        weatherMessage = RoomWindSpeedExtraHighEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.LowBegin)
                    {
                        weatherMessage = RoomWindSpeedLowBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.ExtraLowBegin)
                    {
                        weatherMessage = RoomWindSpeedExtraLowBegin;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.LowEnd)
                    {
                        weatherMessage = RoomWindSpeedLowEnd;
                    }
                    else if (GlobalReference.GlobalValues.World.WindSpeed == GlobalReference.GlobalValues.World.ExtraLowEnd)
                    {
                        weatherMessage = RoomWindSpeedExtraLowEnd;
                    }
                    break;
            }

            return weatherMessage;
        }


        #endregion Weather

        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            base.FinishLoad(zoneObjectSyncValue);

            if (Attributes.Contains(RoomAttribute.Vault))
            {
                ReloadVault();
            }
        }

        private void ReloadVault()
        {
            string file = Path.Combine(GlobalReference.GlobalValues.Settings.VaultDirectory, $"{Zone}-{Id}.vault");
            if (GlobalReference.GlobalValues.FileIO.Exists(file))
            {
                string fileContents = GlobalReference.GlobalValues.FileIO.ReadAllText(file);
                IReadOnlyList<IItem> items = GlobalReference.GlobalValues.Serialization.Deserialize<IReadOnlyList<IItem>>(fileContents);

                foreach (IItem item in items)
                {
                    _items.Add(item);
                }
            }
        }
    }
}