using System.Collections.Generic;
using System.Net;

namespace Objects.Global.Settings.Interface
{
    public interface ISettings
    {
        int AssignableStatPoints { get; set; }
        int BaseStatValue { get; set; }
        int MaxLevel { get; set; }
        int MaxPlayerLevel { get; set; }
        int MaxCalculationLevel { get; }
        double Multiplier { get; set; }
        string AssetsDirectory { get; set; }
        string BugDirectory { get; set; }
        string BulletinBoardDirectory { get; set; }
        string DamageIdDirectory { get; set; }
        string PlayerCharacterDirectory { get; set; }
        string VaultDirectory { get; set; }
        string ZoneDirectory { get; set; }
        bool UseCachingFileIO { get; set; }
        string AsciiArt { get; set; }
        int Port { get; set; }
        bool SendMapPosition { get; set; }
        double RandomDropPercent { get; set; }
        double DropBeingPlusOnePercent { get; set; }
        double ElementalSpawnPercent { get; set; }
        List<IPAddress> BannedIps { get; set; }
        bool LogStats { get; set; }
        string StatsDirectory { get; set; }
    }
}