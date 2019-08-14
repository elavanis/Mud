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

        public void RecursivelySetZone()
        {
            foreach (IRoom room in Rooms.Values)
            {
                room.Zone = Id;

                foreach (IItem item in room.Items)
                {
                    SetItemId(item);
                }

                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    SetCommonMobValues(npc);

                    foreach (IPersonality personality in npc.Personalities)
                    {
                        IMerchant merchant = personality as IMerchant;
                        if (merchant != null)
                        {
                            foreach (IItem item in merchant.Sellables)
                            {
                                SetItemId(item);
                            }
                        }
                    }
                }

                foreach (IMobileObject mob in room.OtherMobs)
                {
                    SetCommonMobValues(mob);
                }
            }
        }

        private void SetCommonMobValues(IMobileObject mob)
        {
            mob.Zone = Id;
            foreach (IItem item in mob.Items)
            {
                SetItemId(item);
            }

            foreach (IItem item in mob.EquipedEquipment)
            {
                SetItemId(item);
            }
        }

        private void SetItemId(IItem item)
        {
            item.Zone = Id;
            IContainer container = item as IContainer;
            if (container != null)
            {
                foreach (IItem innerItem in container.Items)
                {
                    SetItemId(innerItem);
                }
            }
        }
    }
}
