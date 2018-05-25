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
    public class Settings
    {
        public Settings()
        {
            ServerAdress = ConfigurationManager.AppSettings["ServerAdress"];
            Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            MaxLines = int.Parse(ConfigurationManager.AppSettings["MaxLines"]);
            FontSize = int.Parse(ConfigurationManager.AppSettings["FontSize"]);
            Map = bool.Parse(ConfigurationManager.AppSettings["Map"]);
            Sound = bool.Parse(ConfigurationManager.AppSettings["Sound"]);
        }

        public string ServerAdress { get; set; }
        public int Port { get; set; }
        public int MaxLines { get; internal set; }
        public int FontSize { get; set; }
        public bool Map { get; set; }
        public bool Sound { get; set; }


        private Dictionary<TagType, Color> _colorDictionary = null;

        public Dictionary<TagType, Color> ColorDictionary
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

        private Dictionary<string, string> shortCutKeys = null;
        public Dictionary<string, string> ShortCutKeys
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

        internal void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            PropertyInfo[] properties = this.GetType().GetProperties();
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
                    config.AppSettings.Settings[property.Name].Value = property.GetValue(this).ToString();
                }

            }

            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
