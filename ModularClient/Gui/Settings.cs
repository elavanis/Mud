using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shared.TagWrapper.TagWrapper;

namespace Client
{
    public static class Settings
    {
        public static void Initialize()
        {
            ServerAdress = ConfigurationManager.AppSettings["ServerAdress"];
            Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            MaxLines = int.Parse(ConfigurationManager.AppSettings["MaxLines"]);
            FontSize = int.Parse(ConfigurationManager.AppSettings["FontSize"]);
            Map = bool.Parse(ConfigurationManager.AppSettings["Map"]);
            Sound = bool.Parse(ConfigurationManager.AppSettings["Sound"]);
        }

        public static string ServerAdress { get; set; }
        public static int Port { get; set; }
        public static int MaxLines { get; internal set; }
        public static int FontSize { get; set; }
        public static bool Map { get; set; }
        public static bool Sound { get; set; }


        private static Dictionary<TagType, Color> _colorDictionary = null;

        public static Dictionary<TagType, Color> ColorDictionary
        {
            get
            {
                if (_colorDictionary == null)
                {
                    _colorDictionary = new Dictionary<TagType, Color>();

                    foreach (TagType item in Enum.GetValues(typeof(TagType)))
                    {
                        try
                        {
                            Color c = Color.FromName(ConfigurationManager.AppSettings[item.ToString()]);
                            _colorDictionary.Add(item, c);
                        }
                        catch
                        {
                            _colorDictionary.Add(item, Color.White);
                        }
                    }
                }

                return _colorDictionary;
            }
        }

        private static Dictionary<string, string> shortCutKeys = null;
        public static Dictionary<string, string> ShortCutKeys
        {
            get
            {
                if (shortCutKeys == null)
                {
                    shortCutKeys = new Dictionary<string, string>();
                    foreach (string key in ConfigurationManager.AppSettings.AllKeys)
                    {
                        if (key.StartsWith("ShortCutKey"))
                        {
                            string realKey = key.Replace("ShortCutKey", "");
                            shortCutKeys.Add(realKey, ConfigurationManager.AppSettings[key]);
                        }
                    }
                }

                return shortCutKeys;
            }
        }

        internal static void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            PropertyInfo[] properties = typeof(Settings).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "ShortCutKeys")
                {
                    foreach (string key in ShortCutKeys.Keys)
                    {
                        string savedKey = "ShortCutKey" + key;

                        //add the key if it is missing
                        if (!config.AppSettings.Settings.AllKeys.Contains(savedKey))
                        {
                            config.AppSettings.Settings.Add(savedKey, "");
                        }

                        //update the key or remove it if it blank
                        if (string.IsNullOrWhiteSpace(ShortCutKeys[key]))
                        {
                            config.AppSettings.Settings.Remove(savedKey);
                        }
                        else
                        {
                            config.AppSettings.Settings[savedKey].Value = ShortCutKeys[key];
                        }
                    }
                }
                else if (property.Name == "ColorDictionary")
                {
                    foreach (TagType key in ColorDictionary.Keys)
                    {
                        string savedKey = key.ToString();
                        //add the key if it is missing
                        if (!config.AppSettings.Settings.AllKeys.Contains(savedKey))
                        {
                            config.AppSettings.Settings.Add(savedKey, "");
                        }

                        config.AppSettings.Settings[savedKey].Value = ColorDictionary[key].ToString();
                    }
                }
                else
                {
                    config.AppSettings.Settings[property.Name].Value = property.GetValue(null).ToString();
                }

            }

            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
