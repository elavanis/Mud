using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shared.TagWrapper.TagWrapper;

namespace Client.Trigger
{
    public partial class TriggerSettings : Form
    {
        public Dictionary<string, Trigger> _triggers = new Dictionary<string, Trigger>();
        private JsonSerializerSettings _settings;
        private string _defaultCode = @"public abstract class DynamicClass
{
	public static string EvaluateDynamic(string input)
	{
		//return null if you do not want to send a command
		//return a string if you want that string sent as a command
		return input;
	}
}";

        public TriggerSettings()
        {
            InitializeComponent();
            LoadTriggerTypes();
            LoadTriggers();
        }

        private void LoadTriggers()
        {
            if (File.Exists("triggers.config"))
            {
                try
                {
                    Dictionary<string, Trigger> triggers = JsonConvert.DeserializeObject<Dictionary<string, Trigger>>(File.ReadAllText("triggers.config"), Settings);
                    _triggers = triggers;
                }
                catch (Exception e)
                {
                }
            }

            UpdateGui();
        }

        private JsonSerializerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    var temp = new JsonSerializerSettings();
                    temp.TypeNameHandling = TypeNameHandling.Objects;
                    _settings = temp;
                }
                return _settings;
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            Trigger trigger = null;
            _triggers.TryGetValue(textBox_Name.Text, out trigger);
            if (trigger == null)
            {
                trigger = new Trigger();
                trigger.Name = textBox_Name.Text;
                _triggers.Add(trigger.Name, trigger);
            }

            trigger.TagType = (TagType)comboBox_TagType.SelectedItem;
            trigger.Regex = textBox_Regex.Text;
            trigger.Code = richTextBox_Code.Text;

            UpdateGui();

            File.WriteAllText("triggers.config", JsonConvert.SerializeObject(_triggers));
        }

        private void button_New_Click(object sender, EventArgs e)
        {
            ClearTriggerSettings();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            _triggers.Remove(comboBox_TriggerName.SelectedItem.ToString());
            UpdateGui();
        }

        private void LoadTriggerTypes()
        {
            foreach (TagType type in Enum.GetValues(typeof(TagType)))
            {
                comboBox_TagType.Items.Add(type);
            }
        }

        private void UpdateGui()
        {
            comboBox_TriggerName.Items.Clear();
            foreach (string key in _triggers.Keys)
            {
                comboBox_TriggerName.Items.Add(key);
            }

            ClearTriggerSettings();
        }

        private void ClearTriggerSettings()
        {
            textBox_Name.Text = "";
            comboBox_TagType.SelectedItem = comboBox_TagType.Items[0];
            textBox_Regex.Text = "";
            richTextBox_Code.Text = _defaultCode;
        }

        private void SetSelectedItem(object sender, EventArgs e)
        {
            Trigger trigger = _triggers[comboBox_TriggerName.SelectedItem.ToString()];
            textBox_Name.Text = trigger.Name;
            comboBox_TagType.SelectedItem = trigger.TagType;
            textBox_Regex.Text = trigger.Regex.ToString();
            richTextBox_Code.Text = trigger.Code;
        }
    }
}
