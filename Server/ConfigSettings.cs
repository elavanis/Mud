using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ConfigSettings
    {
        public string PlayerCharacterDirectory { get; set; }
        public string ZoneDirectory { get; set; }
        public string LogDirectory { get; set; }
        public string AssetsDirectory { get; set; }
        public int Port { get; set; }
        public bool SendMapPosition { get; set; }

        public string AsciiArt { get; set; }

    }
}
