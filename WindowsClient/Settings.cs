using Newtonsoft.Json;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace WindowsClient
{
    public class Settings
    {
        public string ServerAdress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 52475;
        public int MaxLines { get; set; } = 1000;
        public int FontSize { get; set; } = 8;
        public bool DisplayMap { get; set; } = true;
        public bool PlaySound { get; set; } = true;

        public Dictionary<TagType, Color> ColorDictionary { get; set; } = new Dictionary<TagType, Color>();
        public Dictionary<string, string> ShortCutKeys { get; set; } = new Dictionary<string, string>();




        #region Save/Load
        public Settings(IFileIO fileIO)
        {
            _io = fileIO;
        }
        private const string _userSettingsPath = "UserSettings.json";
        private IFileIO _io;
        private static JsonSerializerSettings? _jsonSettings;
        private static JsonSerializerSettings JsonSettings
        {
            get
            {
                if (_jsonSettings == null)
                {
                    var temp = new JsonSerializerSettings();
                    temp.TypeNameHandling = TypeNameHandling.Objects;
                    _jsonSettings = temp;
                }
                return _jsonSettings;
            }
        }

        public void Save()
        {
            _io.WriteFile(_userSettingsPath, JsonConvert.SerializeObject(this, Formatting.Indented, JsonSettings));
        }

        public void Load()
        {
            Settings settings;

            if (_io.Exists(_userSettingsPath))
            {
                settings = JsonConvert.DeserializeObject<Settings>(_io.ReadAllText(_userSettingsPath), JsonSettings);
            }
            else
            {
                settings = new Settings(_io);
                settings.ColorDictionary.Add(TagType.AsciiArt, Color.Gainsboro);
                settings.ColorDictionary.Add(TagType.ClientCommand, Color.White);
                settings.ColorDictionary.Add(TagType.Connection, Color.SteelBlue);
                settings.ColorDictionary.Add(TagType.Communication, Color.Purple);
                settings.ColorDictionary.Add(TagType.DamageDelt, Color.Gold);
                settings.ColorDictionary.Add(TagType.DamageReceived, Color.DarkRed);
                settings.ColorDictionary.Add(TagType.Info, Color.LightBlue);
                settings.ColorDictionary.Add(TagType.Item, Color.Yellow);
                settings.ColorDictionary.Add(TagType.Mob, Color.Cyan);
                settings.ColorDictionary.Add(TagType.MountStamina, Color.Orange);
                settings.ColorDictionary.Add(TagType.NonPlayerCharacter, Color.Gray);
                settings.ColorDictionary.Add(TagType.PlayerCharacter, Color.MediumPurple);
                settings.ColorDictionary.Add(TagType.Room, Color.Green);
                settings.ColorDictionary.Add(TagType.Health, Color.Red);
                settings.ColorDictionary.Add(TagType.Mana, Color.Blue);
                settings.ColorDictionary.Add(TagType.Stamina, Color.DarkGreen);


                settings.ShortCutKeys.Add("NumPad0", "Kill");
                settings.ShortCutKeys.Add("NumPad1", "Sleep");
                settings.ShortCutKeys.Add("NumPad2", "South");
                settings.ShortCutKeys.Add("NumPad3", "Down");
                settings.ShortCutKeys.Add("NumPad4", "West");
                settings.ShortCutKeys.Add("NumPad5", "Look");
                settings.ShortCutKeys.Add("NumPad6", "East");
                settings.ShortCutKeys.Add("NumPad7", "Stand");
                settings.ShortCutKeys.Add("NumPad8", "North");
                settings.ShortCutKeys.Add("NumPad9", "Up");


            }

            ServerAdress = settings.ServerAdress;
            Port = settings.Port;
            MaxLines = settings.MaxLines;
            FontSize = settings.FontSize;
            DisplayMap = settings.DisplayMap;
            PlaySound = settings.PlaySound;
            ColorDictionary = settings.ColorDictionary;
            ShortCutKeys = settings.ShortCutKeys;  
        }
        #endregion Save/Load

        public class ColorHolder
        {
            public Color Color { get; set; }
        }
    }
}
