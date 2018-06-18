using Objects.Global.Engine.Engines.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Global.Direction.Directions;

namespace Objects.World.Interface
{
    public interface IWorld
    {
        int Precipitation { get; set; }
        int PrecipitationGoal { get; set; }
        int WindSpeed { get; set; }
        int WindSpeedGoal { get; set; }
        Dictionary<int, string> WeatherTriggers { get; set; }

        string WorldPrecipitationExtraHighBegin { get; set; }
        string WorldPrecipitationExtraHighEnd { get; set; }
        string WorldPrecipitationExtraLowBegin { get; set; }
        string WorldPrecipitationExtraLowEnd { get; set; }
        string WorldPrecipitationHighBegin { get; set; }
        string WorldPrecipitationHighEnd { get; set; }
        string WorldPrecipitationLowBegin { get; set; }
        string WorldPrecipitationLowEnd { get; set; }
        string WorldWindSpeedExtraHighBegin { get; set; }
        string WorldWindSpeedExtraHighEnd { get; set; }
        string WorldWindSpeedExtraLowBegin { get; set; }
        string WorldWindSpeedExtraLowEnd { get; set; }
        string WorldWindSpeedHighBegin { get; set; }
        string WorldWindSpeedHighEnd { get; set; }
        string WorldWindSpeedLowBegin { get; set; }
        string WorldWindSpeedLowEnd { get; set; }

        int HighBegin { get; set; }
        int ExtraHighBegin { get; set; }
        int HighEnd { get; set; }
        int ExtraHighEnd { get; set; }
        int LowBegin { get; set; }
        int ExtraLowBegin { get; set; }
        int LowEnd { get; set; }
        int ExtraLowEnd { get; set; }


        Dictionary<int, IZone> Zones { get; }

        ConcurrentQueue<IPlayerCharacter> AddPlayerQueue { get; }
        IPlayerCharacter CreateCharacter(string userName, string password);
        IZone DeserializeZone(string serializedZone);
        IPlayerCharacter LoadCharacter(string name);
        void LoadWorld();
        void LogOutCharacter(string name);
        void PerformTick();
        void SaveCharcter(IPlayerCharacter character);
        void SaveWorld();
        string SerializePlayerCharacter(IPlayerCharacter character);
        string SerializeZone(IZone zone);

        ConcurrentDictionary<string, string> WorldResults { get; }
        ConcurrentQueue<string> WorldCommands { get; }
        List<IPlayerCharacter> CurrentPlayers { get; }
    }
}
