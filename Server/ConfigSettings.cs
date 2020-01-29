using System.Collections.Generic;
using Objects.Moon.Interface;

namespace Server
{
    public class ConfigSettings
    {
        public string AssetsDirectory { get; set; }
        public string BugDirectory { get; set; }
        public string BulletinBoardDirectory { get; set; }
        public string LogDirectory { get; set; }
        public string PlayerCharacterDirectory { get; set; }
        public string VaultDirectory { get; set; }
        public string WeaponIdDirectory { get; set; }
        public string ZoneDirectory { get; set; }
        public bool LogStats { get; set; }
        public string StatsDirectory { get; set; }
        public bool UseCachingFileIO { get; set; }
        public int Port { get; set; }
        public bool SendMapPosition { get; set; }
        public string BannedIps { get; set; }
        public double ElemenatlSpawnPercent { get; set; }
        public double RandomDropPercent { get; set; }
        public double DropBeingPlusOnePercent { get; set; }

        public string AsciiArt { get; set; }
        public List<IMoon> Moons { get; set; }
    }
}
