using Objects.Zone.Interface;
using System.Collections.Generic;
using Objects.Room.Interface;
using Objects.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Personality.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.GameDateTime.Interface;
using System.Text;
using System;

namespace Objects.Zone
{
    public class Zone : BaseObject, IZone, ILoadable
    {
        [ExcludeFromCodeCoverage]
        public bool RepeatZoneProcessing { get; set; }

        [ExcludeFromCodeCoverage]
        public int ZoneObjectSyncOptions { get; set; } = -1;

        [ExcludeFromCodeCoverage]
        public int InGameDaysTillReset { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public IGameDateTime ResetTime { get; set; } = new GameDateTime.GameDateTime();

        [ExcludeFromCodeCoverage]
        public string Name { get; set; }

        [ExcludeFromCodeCoverage]
        public Dictionary<int, IRoom> Rooms { get; set; } = new Dictionary<int, IRoom>();

        [ExcludeFromCodeCoverage]
        public int Level { get; private set; }

        #region Weather
        #region Weather High Points
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraHighEnd { get; set; }
        #endregion Weather High Points

        #region Weather Low Points
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraLowEnd { get; set; }
        #endregion Weather Low Points
        #endregion Weather

        /// <summary>
        /// The zoneObjectSyncValue is not used at the Zone level but is needed for the interface
        /// </summary>
        /// <param name="zoneObjectSyncValue"></param>
        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            if (InGameDaysTillReset != -1)
            {
                ResetTime = GlobalReference.GlobalValues.GameDateTime.GameDateTime.AddDays(InGameDaysTillReset);
            }
            else
            {
                ResetTime = GlobalReference.GlobalValues.GameDateTime.GameDateTime.AddDays(999999); //sets reset to 45 years in real life, close enough to never
            }

            if (ZoneObjectSyncOptions != -1)
            {
                zoneObjectSyncValue = GlobalReference.GlobalValues.Random.Next(ZoneObjectSyncOptions);
            }

            FinishLoadingObjects(zoneObjectSyncValue);

            SetMedianZoneLevel();
        }

        #region To Cs File
        public string ToCsFile(int zoneId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Header(zoneId));

            foreach (IRoom room in Rooms.Values)
            {
                stringBuilder.AppendLine(BuildRoom(room));
            }

            stringBuilder.AppendLine(@"        #endregion Rooms
");

            stringBuilder.AppendLine(ConnectRooms());

            stringBuilder.Append(@"    }
}");
            return stringBuilder.ToString();
        }

        private string Header(int zoneId)
        {
            return @"using GenerateZones;
using GenerateZones.Zones;
using MiscShared;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;

namespace GeneratedZones
{
    public class GeneratedZone : BaseZone, IZoneCode
    {
        public GeneratedZone() : base({zoneId})
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GeneratedZone);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms".Replace("{zoneId}", zoneId.ToString());
        }

        private string BuildRoom(IRoom room)
        {
            return @"        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.LookDescription = ""YOU ARE IN A MAZE OF TWISTY LITTLE PASSAGES, ALL ALIKE."";
            room.ShortDescription = ""Underground cavern"";

            return room;
        }".Replace("1", room.Id.ToString());
        }

        private string ConnectRooms()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(@"        private void ConnectRooms()
        {");

            foreach (IRoom room in Rooms.Values)
            {
                if (room.East != null)
                {
                    stringBuilder.AppendLine("            ZoneHelper.ConnectRoom(Zone.Rooms[{1}], Direction.East, Zone.Rooms[{2}]);".Replace("{1}", room.Id.ToString()).Replace("{2}", room.East.Room.ToString()));
                }

                if (room.South != null)
                {
                    stringBuilder.AppendLine("            ZoneHelper.ConnectRoom(Zone.Rooms[{1}], Direction.South, Zone.Rooms[{2}]);".Replace("{1}", room.Id.ToString()).Replace("{2}", room.South.Room.ToString()));
                }
            }

            stringBuilder.Append(@"        }");

            return stringBuilder.ToString();
        }
        #endregion To Cs File

        private void SetMedianZoneLevel()
        {
            List<int> mobsLevel = new List<int>();
            foreach (IRoom room in Rooms.Values)
            {
                foreach (IMobileObject mob in room.NonPlayerCharacters)
                {
                    mobsLevel.Add(mob.Level);
                }
            }

            if (mobsLevel.Count == 0)
            {
                Level = 0;
            }
            else
            {
                Level = mobsLevel[mobsLevel.Count / 2];
            }
        }

        private void FinishLoadingObjects(int zoneObjectSyncValue)
        {
            foreach (IRoom room in Rooms.Values)
            {
                room.FinishLoad(zoneObjectSyncValue);
                room.ZoneObjectSyncLoad(zoneObjectSyncValue);

                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    npc.FinishLoad(zoneObjectSyncValue);
                    npc.ZoneObjectSyncLoad(zoneObjectSyncValue);
                }

                foreach (IMobileObject mob in room.OtherMobs)
                {
                    mob.FinishLoad(zoneObjectSyncValue);
                    mob.ZoneObjectSyncLoad(zoneObjectSyncValue);
                }

                foreach (IItem item in room.Items)
                {
                    item.FinishLoad(zoneObjectSyncValue);
                    item.ZoneObjectSyncLoad(zoneObjectSyncValue);

                    IContainer container = item as IContainer;
                    if (container != null)
                    {
                        RecursiveLoadItem(container, ZoneObjectSyncOptions);
                    }
                }
            }
        }

        private void RecursiveLoadItem(IContainer container, int zoneObjectSyncOptions)
        {
            foreach (IItem item in container.Items)
            {
                item.FinishLoad(zoneObjectSyncOptions);
                item.ZoneObjectSyncLoad(zoneObjectSyncOptions);

                IContainer container2 = item as IContainer;
                if (container2 != null)
                {
                    RecursiveLoadItem(container2, ZoneObjectSyncOptions);
                }
            }
        }
    }
}
