using System.Collections.Generic;
using Objects.Moon.Interface;

namespace Server
{
    public class ConfigSettings
    {
        public ConfigSettings(string assetsDirectory, string bugDirectory, string bulletinBoardDirectory, string logDirectory, string playerCharacterDirectory, string vaultDirectory, string weaponIdDirectory, string zoneDirectory, bool logStats, string statsDirectory, bool useCachingFileIO, int port, bool sendMapPosition, string bannedIps, double elemenatlSpawnPercent, double randomDropPercent, double dropBeingPlusOnePercent, string asciiArt, List<IMoon> moons)
        {
            AssetsDirectory = assetsDirectory;
            BugDirectory = bugDirectory;
            BulletinBoardDirectory = bulletinBoardDirectory;
            LogDirectory = logDirectory;
            PlayerCharacterDirectory = playerCharacterDirectory;
            VaultDirectory = vaultDirectory;
            WeaponIdDirectory = weaponIdDirectory;
            ZoneDirectory = zoneDirectory;
            LogStats = logStats;
            StatsDirectory = statsDirectory;
            UseCachingFileIO = useCachingFileIO;
            Port = port;
            SendMapPosition = sendMapPosition;
            BannedIps = bannedIps;
            ElemenatlSpawnPercent = elemenatlSpawnPercent;
            RandomDropPercent = randomDropPercent;
            DropBeingPlusOnePercent = dropBeingPlusOnePercent;
            AsciiArt = asciiArt;
            Moons = moons;
        }

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
