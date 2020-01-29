using Objects.Global.Settings.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Objects.Global.Settings
{
    public class Settings : ISettings
    {
        [ExcludeFromCodeCoverage]
        public int BaseStatValue { get; set; } = 7;

        [ExcludeFromCodeCoverage]
        public int AssignableStatPoints { get; set; } = 18;

        [ExcludeFromCodeCoverage]
        public double Multiplier { get; set; } = 1.1d;

        [ExcludeFromCodeCoverage]
        public int MaxLevel { get; set; } = 107; //if this goes higher exp will need to be BigInteger 

        [ExcludeFromCodeCoverage]
        public int MaxPlayerLevel { get; set; } = 100;

        [ExcludeFromCodeCoverage]
        public int MaxCalculationLevel { get; set; } = 130;

        [ExcludeFromCodeCoverage]
        public string PlayerCharacterDirectory { get; set; } = "..\\Players";

        [ExcludeFromCodeCoverage]
        public string BulletinBoardDirectory { get; set; } = "..\\BulletinBoards";

        [ExcludeFromCodeCoverage]
        public string VaultDirectory { get; set; } = "..\\Vaults";

        [ExcludeFromCodeCoverage]
        public string ZoneDirectory { get; set; } = "..\\World";

        [ExcludeFromCodeCoverage]
        public string AssetsDirectory { get; set; } = "..\\Assets";

        [ExcludeFromCodeCoverage]
        public string BugDirectory { get; set; } = "..\\Bugs";

        [ExcludeFromCodeCoverage]
        public string WeaponIdDirectory { get; set; } = "..\\WeaponId";

        [ExcludeFromCodeCoverage]
        public bool UseCachingFileIO { get; set; }

        [ExcludeFromCodeCoverage]
        public string AsciiArt { get; set; }

        [ExcludeFromCodeCoverage]
        public int Port { get; set; }

        [ExcludeFromCodeCoverage]
        public bool SendMapPosition { get; set; }

        [ExcludeFromCodeCoverage]
        public double RandomDropPercent { get; set; } = 10;

        [ExcludeFromCodeCoverage]
        public double DropBeingPlusOnePercent { get; set; } = 10;

        [ExcludeFromCodeCoverage]
        public double ElementalSpawnPercent { get; set; } = .01;

        [ExcludeFromCodeCoverage]
        public List<IPAddress> BannedIps { get; set; } = new List<IPAddress>();

        [ExcludeFromCodeCoverage]
        public bool LogStats { get; set; }

        [ExcludeFromCodeCoverage]
        public string StatsDirectory { get; set; } = "..\\Stats";
    }
}
