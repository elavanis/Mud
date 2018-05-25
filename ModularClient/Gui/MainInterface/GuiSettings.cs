using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.MainInterface
{
    public partial class GuiSettings : Form
    {
        private Settings _settings;
        public GuiSettings(Settings settings)
        {
            InitializeComponent();
            _settings = settings;
            numericUpDown_FontSize.Value = settings.FontSize;

            foreach (string key in settings.ShortCutKeys.Keys)
            {
                comboBox_ShortCutKey.Items.Add(key);
            }
        }

        private void ResizeScreen(object sender, EventArgs e)
        {
            Font font = new Font(label_FontSize.Font.FontFamily, (float)numericUpDown_FontSize.Value);
            label_FontSize.Font = font;
            numericUpDown_FontSize.Font = font;
            _settings.FontSize = (int)numericUpDown_FontSize.Value;
        }

        private void UpdateSettings(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string name = textBox.Name.Split('_')[1];
                PropertyInfo info = _settings.GetType().GetProperty(name);
                info.SetValue(_settings, textBox.Text);
            }
        }

        private void ComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            string keyCode = e.KeyCode.ToString();

            string value = null;
            if (_settings.ShortCutKeys.TryGetValue(keyCode, out value))
            {
                textBox_ShortCutCommand.Text = value;
                comboBox_ShortCutKey.Text = keyCode;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                _settings.ShortCutKeys.Add(keyCode, "");
                comboBox_ShortCutKey.Text = keyCode;
                comboBox_ShortCutKey.Items.Add(keyCode);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void UpdateShortCutText(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox_ShortCutKey.Text))
            {
                if (_settings.ShortCutKeys.ContainsKey(comboBox_ShortCutKey.Text))
                {
                    _settings.ShortCutKeys[comboBox_ShortCutKey.Text] = textBox_ShortCutCommand.Text;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_ShortCutCommand.Text = _settings.ShortCutKeys[comboBox_ShortCutKey.Text];
        }
    }
}
