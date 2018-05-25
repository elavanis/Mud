using Objects.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Zone.Interface
{
    public interface IZone : ILoadable
    {
        bool RepeatZoneProcessing { get; set; }
        object LockObject { get; }
        int ZoneObjectSyncOptions { get; set; }
        int InGameDaysTillReset { get; set; }
        DateTime ResetTime { get; set; }
        Dictionary<int, IRoom> Rooms { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string ZonePrecipitationExtraHighBegin { get; set; }
        string ZonePrecipitationExtraHighEnd { get; set; }
        string ZonePrecipitationExtraLowBegin { get; set; }
        string ZonePrecipitationExtraLowEnd { get; set; }
        string ZonePrecipitationHighBegin { get; set; }
        string ZonePrecipitationHighEnd { get; set; }
        string ZonePrecipitationLowBegin { get; set; }
        string ZonePrecipitationLowEnd { get; set; }
        string ZoneWindSpeedExtraHighBegin { get; set; }
        string ZoneWindSpeedExtraHighEnd { get; set; }
        string ZoneWindSpeedExtraLowBegin { get; set; }
        string ZoneWindSpeedExtraLowEnd { get; set; }
        string ZoneWindSpeedHighBegin { get; set; }
        string ZoneWindSpeedHighEnd { get; set; }
        string ZoneWindSpeedLowBegin { get; set; }
        string ZoneWindSpeedLowEnd { get; set; }

        void RecursivelySetZone();
    }
}
