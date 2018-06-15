using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.Sound;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Objects.Global.Direction.Directions;
using static Shared.TagWrapper.TagWrapper;
using Objects.LoadPercentage.Interface;
using Objects.Trap.Interface;
using System.Collections.ObjectModel;

namespace Objects.Room
{
    public class Room : BaseObject, IRoom, IContainer, ILoadableItems
    {

        private static ReadOnlyCollection<INonPlayerCharacter> BlankNonPlayerCharacters { get; } = new List<INonPlayerCharacter>().AsReadOnly();
        private static ReadOnlyCollection<IPlayerCharacter> BlankPlayerCharacters { get; } = new List<IPlayerCharacter>().AsReadOnly();

        public Room()
        {
            KeyWords.Add("room");
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Zone, Id);
        }

        [ExcludeFromCodeCoverage]
        public int MovementCost { get; set; }

        [ExcludeFromCodeCoverage]
        public List<IItem> Items { get; } = new List<IItem>();

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
            set
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
            set
            {
                lock (_playerCharactersLock)
                {
                    _playerCharacters = new List<IPlayerCharacter>(value);
                }
            }
        }



        [ExcludeFromCodeCoverage]
        public List<Shared.Sound.Interface.ISound> Sounds { get; } = new List<Shared.Sound.Interface.ISound>();

        public string SerializedSounds
        {
            get
            {
                return GlobalReference.GlobalValues.Serialization.Serialize(Sounds);
            }
        }

        #region Exits
        [ExcludeFromCodeCoverage]
        public IExit North { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit East { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit South { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit West { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit Up { get; set; }
        [ExcludeFromCodeCoverage]
        public IExit Down { get; set; }
        #endregion Exits

        public IResult CheckEnter(IMobileObject mobileObject)
        {
            INonPlayerCharacter npc = mobileObject as INonPlayerCharacter;
            if (npc != null)
            {
                if (Attributes.Contains(RoomAttribute.Small))
                {
                    if (NonPlayerCharacters.Count != 0)
                    {
                        return new Result(false, "The room is to small to fit another person in there.");
                    }
                }

                if (Attributes.Contains(RoomAttribute.NoNPC))
                {
                    return new Result(false, "Non player characters can not enter here.");
                }
            }

            IPlayerCharacter pc = mobileObject as IPlayerCharacter;
            if (pc != null)
            {
                if (Attributes.Contains(RoomAttribute.Small))
                {
                    if (PlayerCharacters.Count != 0)
                    {
                        return new Result(false, "The room is to small to fit another person in there.");
                    }
                }
            }

            return null;
        }

        public IResult CheckLeave(IMobileObject mobileObject)
        {
            IResult result = CheckFlee(mobileObject);
            if (result != null)
            {
                return result;
            }

            if (mobileObject.IsInCombat)
            {
                //TODO Update this message when flee is implemented to mention fleeing.
                return new Result(false, "You can not leave while your fighting for your life.");
            }

            return null;
        }

        public IResult CheckFlee(IMobileObject mobileObject)
        {
            //mob does not have enough stamina to cover room movement cost
            //we want to make them fully recover so they are not stuck
            if ((mobileObject.MaxStamina < MovementCost
                && mobileObject.Stamina < mobileObject.MaxStamina))

            {
                return new Result(false, "You need to rest before you attempt to leave.");
            }

            //move does not have enough stamina to leave, rest
            if (mobileObject.MaxStamina >= MovementCost
                && mobileObject.Stamina < MovementCost)
            {
                return new Result(false, "You need to rest before you attempt to leave.");
            }

            return null;
        }

        public IResult CheckLeaveDirection(IMobileObject mobileObject, Direction direction)
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
                                return new Result(false, guard.BlockLeaveMessage);
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
        }

        public void AddMobileObjectToRoom(IMobileObject mob)
        {
            INonPlayerCharacter npc = mob as INonPlayerCharacter;
            if (npc != null)
            {
                lock (_nonPlayerCharactersLock)
                {
                    _nonPlayerCharacters.Add(npc);
                }
            }
            else
            {
                IPlayerCharacter pc = mob as IPlayerCharacter;
                if (pc != null)
                {
                    lock (_playerCharactersLock)
                    {
                        _playerCharacters.Add(pc);
                    }
                }
            }
        }



        public bool Leave(IMobileObject performer, Direction direction)
        {
            performer.Stamina -= MovementCost;

            GlobalReference.GlobalValues.Engine.Event.LeaveRoom(performer, direction);

            return RemoveMobileObjectFromRoom(performer);
        }

        public bool RemoveMobileObjectFromRoom(IMobileObject mob)
        {
            INonPlayerCharacter npc = mob as INonPlayerCharacter;
            if (npc != null)
            {
                lock (_nonPlayerCharactersLock)
                {
                    return _nonPlayerCharacters.Remove(npc);
                }
            }
            else
            {
                IPlayerCharacter pc = mob as IPlayerCharacter;
                if (pc != null)
                {
                    lock (_playerCharactersLock)
                    {
                        return _playerCharacters.Remove(pc);
                    }
                }
            }

            return false;
        }

        [ExcludeFromCodeCoverage]
        public List<RoomAttribute> Attributes { get; } = new List<RoomAttribute>();

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
            /// A players house
            /// </summary>
            PlayerHouse,
            /// <summary>
            /// Aggressive actions will be ignored in this room
            /// </summary>
            Peaceful,
            /// <summary>
            /// Only the owner can enter
            /// </summary>
            Private,
            /// <summary>
            /// Saves the contents of this room
            /// </summary>
            Save,
            NoLight,
            Light,
            /// <summary>
            /// Characters will be notified of weather changes
            /// </summary>
            Weather,
            /// <summary>
            /// Tracking will not find paths through this room
            /// </summary>
            NoTrack
        }

        #region Weather
        #region Weather Messages
        #region Weather High Points
        private string roomPrecipitationHighBegin;
        public string RoomPrecipitationHighBegin
        {
            get
            {
                if (roomPrecipitationHighBegin == null)
                {
                    roomPrecipitationHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationHighBegin;
                }
                return roomPrecipitationHighBegin;
            }
            set
            {
                roomPrecipitationHighBegin = value;
            }
        }

        private string roomPrecipitationHighEnd;
        public string RoomPrecipitationHighEnd
        {
            get
            {
                if (roomPrecipitationHighEnd == null)
                {
                    roomPrecipitationHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationHighEnd;
                }
                return roomPrecipitationHighEnd;
            }
            set
            {
                roomPrecipitationHighEnd = value;
            }
        }

        private string roomPrecipitationExtraHighBegin;
        public string RoomPrecipitationExtraHighBegin
        {
            get
            {
                if (roomPrecipitationExtraHighBegin == null)
                {
                    roomPrecipitationExtraHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraHighBegin;
                }
                return roomPrecipitationExtraHighBegin;
            }
            set
            {
                roomPrecipitationExtraHighBegin = value;
            }
        }

        private string roomPrecipitationExtraHighEnd;
        public string RoomPrecipitationExtraHighEnd
        {
            get
            {
                if (roomPrecipitationExtraHighEnd == null)
                {
                    roomPrecipitationExtraHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraHighEnd;
                }
                return roomPrecipitationExtraHighEnd;
            }
            set
            {
                roomPrecipitationExtraHighEnd = value;
            }
        }

        private string roomWindSpeedHighBegin;
        public string RoomWindSpeedHighBegin
        {
            get
            {
                if (roomWindSpeedHighBegin == null)
                {
                    roomWindSpeedHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedHighBegin;
                }
                return roomWindSpeedHighBegin;
            }
            set
            {
                roomWindSpeedHighBegin = value;
            }
        }

        private string roomWindSpeedHighEnd;
        public string RoomWindSpeedHighEnd
        {
            get
            {
                if (roomWindSpeedHighEnd == null)
                {
                    roomWindSpeedHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedHighEnd;
                }
                return roomWindSpeedHighEnd;
            }
            set
            {
                roomWindSpeedHighEnd = value;
            }
        }

        private string roomWindSpeedExtraHighBegin;
        public string RoomWindSpeedExtraHighBegin
        {
            get
            {
                if (roomWindSpeedExtraHighBegin == null)
                {
                    roomWindSpeedExtraHighBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraHighBegin;
                }
                return roomWindSpeedExtraHighBegin;
            }
            set
            {
                roomWindSpeedExtraHighBegin = value;
            }
        }

        private string roomWindSpeedExtraHighEnd;
        public string RoomWindSpeedExtraHighEnd
        {
            get
            {
                if (roomWindSpeedExtraHighEnd == null)
                {
                    roomWindSpeedExtraHighEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraHighEnd;
                }
                return roomWindSpeedExtraHighEnd;
            }
            set
            {
                roomWindSpeedExtraHighEnd = value;
            }
        }
        #endregion Weather High Points

        #region Weather Low Points
        private string roomPrecipitationLowBegin;
        public string RoomPrecipitationLowBegin
        {
            get
            {
                if (roomPrecipitationLowBegin == null)
                {
                    roomPrecipitationLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationLowBegin;
                }
                return roomPrecipitationLowBegin;
            }
            set
            {
                roomPrecipitationLowBegin = value;
            }
        }

        private string roomPrecipitationLowEnd;
        public string RoomPrecipitationLowEnd
        {
            get
            {
                if (roomPrecipitationLowEnd == null)
                {
                    roomPrecipitationLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationLowEnd;
                }
                return roomPrecipitationLowEnd;
            }
            set
            {
                roomPrecipitationLowEnd = value;
            }
        }

        private string roomPrecipitationExtraLowBegin;
        public string RoomPrecipitationExtraLowBegin
        {
            get
            {
                if (roomPrecipitationExtraLowBegin == null)
                {
                    roomPrecipitationExtraLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraLowBegin;
                }
                return roomPrecipitationExtraLowBegin;
            }
            set
            {
                roomPrecipitationExtraLowBegin = value;
            }
        }

        private string roomPrecipitationExtraLowEnd;
        public string RoomPrecipitationExtraLowEnd
        {
            get
            {
                if (roomPrecipitationExtraLowEnd == null)
                {
                    roomPrecipitationExtraLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZonePrecipitationExtraLowEnd;
                }
                return roomPrecipitationExtraLowEnd;
            }
            set
            {
                roomPrecipitationExtraLowEnd = value;
            }
        }

        private string roomWindSpeedLowBegin;
        public string RoomWindSpeedLowBegin
        {
            get
            {
                if (roomWindSpeedLowBegin == null)
                {
                    roomWindSpeedLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedLowBegin;
                }
                return roomWindSpeedLowBegin;
            }
            set
            {
                roomWindSpeedLowBegin = value;
            }
        }

        private string roomWindSpeedLowEnd;
        public string RoomWindSpeedLowEnd
        {
            get
            {
                if (roomWindSpeedLowEnd == null)
                {
                    roomWindSpeedLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedLowEnd;
                }
                return roomWindSpeedLowEnd;
            }
            set
            {
                roomWindSpeedLowEnd = value;
            }
        }

        private string roomWindSpeedExtraLowBegin;
        public string RoomWindSpeedExtraLowBegin
        {
            get
            {
                if (roomWindSpeedExtraLowBegin == null)
                {
                    roomWindSpeedExtraLowBegin = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraLowBegin;
                }
                return roomWindSpeedExtraLowBegin;
            }
            set
            {
                roomWindSpeedExtraLowBegin = value;
            }
        }

        private string roomWindSpeedExtraLowEnd;
        public string RoomWindSpeedExtraLowEnd
        {
            get
            {
                if (roomWindSpeedExtraLowEnd == null)
                {
                    roomWindSpeedExtraLowEnd = GlobalReference.GlobalValues.World.Zones[Zone].ZoneWindSpeedExtraLowEnd;
                }
                return roomWindSpeedExtraLowEnd;
            }
            set
            {
                roomWindSpeedExtraLowEnd = value;
            }
        }
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

        private string GetWeatherMessage(string weatherType)
        {
            string weatherMessage = null;
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
    }
}
