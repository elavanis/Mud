using System;
using System.Collections.Generic;
using System.Text;
using Objects.Moon.Interface;

namespace Server
{
    public class ConfigSettings
    {
        public string PlayerCharacterDirectory { get; set; }
        public string ZoneDirectory { get; set; }
        public string LogDirectory { get; set; }
        public string AssetsDirectory { get; set; }
        public string VaultDirectory { get; set; }
        public string BugDirectory { get; set; }
        public bool LogStats { get; set; }
        public string LogStatsLocation { get; set; }
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
