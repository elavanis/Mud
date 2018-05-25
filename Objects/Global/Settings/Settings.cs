using Objects.Global.Settings.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

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
        public int MaxLevel { get; set; } = 100;

        [ExcludeFromCodeCoverage]
        public string PlayerCharacterDirectory { get; set; } = "..\\Players";

        [ExcludeFromCodeCoverage]
        public string ZoneDirectory { get; set; } = "..\\World";

        [ExcludeFromCodeCoverage]
        public string AssetsDirectory { get; set; } = "..\\Assets";

        [ExcludeFromCodeCoverage]
        public string AsciiArt { get; set; }

        [ExcludeFromCodeCoverage]
        public int Port { get; set; }

        [ExcludeFromCodeCoverage]
        public bool SendMapPosition { get; set; }
    }
}
