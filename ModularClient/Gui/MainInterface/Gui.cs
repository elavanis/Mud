using Client.AssetValidation;
using Client.Extensions;
using Client.Map;
using Client.Sound;
using Client.Trigger;
using ClientTelentCommucication;
using MessageParser;
using Newtonsoft.Json;
using Shared.FileIO;
using Shared.FileIO.Interface;
using Shared.Sound.Interface;
using Shared.TelnetItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TelnetCommunication;
using static Shared.TagWrapper.TagWrapper;

namespace Client.MainInterface
{
    public partial class GraphicalUserInterFace : Form
    {
        private int margin = 3;
        private TelnetHandler _telnetHandler;
        private SoundHandler _soundHandler;
        private PreviousCommands _previousCommands;
        private Dictionary<TagType, List<Trigger.Trigger>> _triggers;
        private Map.Map _mapWindow = null;

        public GraphicalUserInterFace()
        {
            InitializeComponent();
        }

        #region StartUp
        private void LoadForm(object sender, EventArgs e)
        {
            Settings.Initialize();
            _previousCommands = new PreviousCommands();
            //_triggers = new Dictionary<TagType, List<Trigger.Trigger>>();
            SetTriggersDictioanry(new TriggerSettings());
            try
            {
                _telnetHandler = new ClientHandler(Settings.ServerAdress, Settings.Port, new JsonMudMessage());
                timer_UpdateTimer.Start();
                //richTextBox1.ReadOnly = true;
                SetDoubleBufferToOn(myRichTextBox_MainText);
            }
            catch (Exception ex)
            {
                string message = "Unable to connect" + Environment.NewLine + ex.Message;
                MessageBox.Show(message);
                this.Close();
                return;
            }

            SelectTextEntry(sender, e);
            ResetFontSize();
            if (Settings.Map)
            {
                LoadMap();
            }
            if (Settings.Sound)
            {
                LoadSound();
            }
        }
        private void SetDoubleBufferToOn(MyRichTextBox richTextBox1)
        {
            PropertyInfo aProp = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);

            aProp.SetValue(richTextBox1, true, null);
        }
        #endregion Startup

        #region Menus
        private void GuiSettings(object sender, EventArgs e)
        {
            GuiSettings guiSettings = new GuiSettings();
            guiSettings.ShowDialog();
            ResetFontSize();
            Settings.Save();
        }

        private void ResetFontSize()
        {
            Font font = new Font(myRichTextBox_MainText.Font.FontFamily, Settings.FontSize);
            myRichTextBox_MainText.Font = font;
            textBox_Intelisense.Font = font;
            textBox_CommandBox.Font = font;
            menuStrip_Menu.Font = font;

            int formHeight = textBox_CommandBox.Parent.ClientSize.Height;
            textBox_CommandBox.Top = formHeight - (margin + textBox_CommandBox.Height);
            textBox_Intelisense.Top = textBox_CommandBox.Top - (textBox_Intelisense.Height + margin);
            myRichTextBox_MainText.Top = menuStrip_Menu.Size.Height + 3;
            myRichTextBox_MainText.Height = textBox_Intelisense.Top - (margin * 2 + myRichTextBox_MainText.Top);

        }

        private void ToggleMap(object sender, EventArgs e)
        {
            if (_mapWindow == null || _mapWindow.IsDisposed)
            {
                LoadMap();
            }
            else
            {
                _mapWindow.Close();
                mapToolStripMenuItem.Checked = false;
            }

            Settings.Save();
        }

        private void LoadMap()
        {
            _mapWindow = new Map.Map(_telnetHandler);
            _mapWindow.Show();
            mapToolStripMenuItem.Checked = true;
        }

        private void ToggleSound(object sender, EventArgs e)
        {
            if (_soundHandler == null)
            {
                LoadSound();
            }
            else
            {
                _soundHandler.StopAll();
                _soundHandler = null;
                soundToolStripMenuItem.Checked = false;
            }
            Settings.Save();
        }

        private void LoadSound()
        {
            _soundHandler = new SoundHandler(_telnetHandler);
            soundToolStripMenuItem.Checked = true;
        }

        private void TriggerSettings(object sender, EventArgs e)
        {
            TriggerSettings triggerSettings = new TriggerSettings();
            triggerSettings.ShowDialog();
            SetTriggersDictioanry(triggerSettings);
            ResetFontSize();
        }

        private void SetTriggersDictioanry(TriggerSettings triggerSettings)
        {
            _triggers = new Dictionary<TagType, List<Trigger.Trigger>>();
            foreach (Trigger.Trigger trigger in triggerSettings._triggers.Values)
            {
                List<Trigger.Trigger> listOfTriggers;
                _triggers.TryGetValue(trigger.TagType, out listOfTriggers);
                if (listOfTriggers == null)
                {
                    listOfTriggers = new List<Trigger.Trigger>();
                    _triggers.Add(trigger.TagType, listOfTriggers);
                }
                listOfTriggers.Add(trigger);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.G))
            {
                GuiSettings(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                ToggleMap(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                ToggleSound(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion Menus

        #region TextEntry
        private void SelectTextEntry(object sender, EventArgs e)
        {
            textBox_CommandBox.Select();
            textBox_CommandBox.Focus();
        }

        private void CatchTabAutoComplete(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    if (textBox_Intelisense.Text != string.Empty)
                    {
                        textBox_CommandBox.Text = textBox_Intelisense.Text;
                    }
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    SendCommandText();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                default:
                    string command = null;

                    if (Settings.ShortCutKeys.TryGetValue(e.KeyCode.ToString(), out command))
                    {
                        SendCommandText(command);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
            }

            //switch (e.KeyCode)
            //{
            //    case Keys.Tab:
            //        e.Handled = true;
            //        break;
            //    case Keys.Enter:
            //    case Keys.NumPad0:
            //    case Keys.NumPad1:
            //    case Keys.NumPad2:
            //    case Keys.NumPad3:
            //    case Keys.NumPad4:
            //    case Keys.NumPad5:
            //    case Keys.NumPad6:
            //    case Keys.NumPad7:
            //    case Keys.NumPad8:
            //    case Keys.NumPad9:
            //        e.Handled = true;
            //        e.SuppressKeyPress = true;
            //        break;
            //}
        }

        private void CatchTabAutoComplete(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab
                || e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = true;
            }
        }

        private void UpdateIntelisenseOption(object sender, EventArgs e)
        {
            textBox_Intelisense.Text = _previousCommands.Intelisense(textBox_CommandBox.Text);
        }

        private void CatchTypeInMainScreen(object sender, PreviewKeyDownEventArgs e)
        {
            SelectTextEntry(null, null);
        }

        private void Intelisense(object sender, EventArgs e)
        {
            textBox_Intelisense.Text = _previousCommands.Intelisense(textBox_CommandBox.Text);
        }
        #endregion TextEntry

        #region SendReceiveMessage
        #region Send Messages
        private void SendCommandText()
        {
            SendCommandText(textBox_CommandBox.Text);
            _previousCommands.Add(textBox_CommandBox.Text);

            textBox_CommandBox.Text = "";
        }

        private void SendCommandText(string text)
        {
            _telnetHandler.OutQueue.Enqueue(text);
            List<ParsedMessage> list = new List<ParsedMessage>();
            list.Add(new ParsedMessage() { Message = text + Environment.NewLine, TagType = TagType.ClientCommand });
            myRichTextBox_MainText.AddFormatedText(list);
        }

        #endregion Send Messages
        #region ReceiveMessage
        private void UpdateGui(object sender, EventArgs e)
        {
            string message;

            while (_telnetHandler.InQueue.TryDequeue(out message))
            {
                if (message.StartsWith("<Sound>"))
                {
                    _soundHandler?.HandleSounds(message);
                }
                else if (message.StartsWith("<Map>"))
                {
                    if (_mapWindow != null && !_mapWindow.IsDisposed)
                    {
                        _mapWindow.Update(message);
                    }
                }
                else if (message.StartsWith("<Data>"))
                {
                    SaveFile(message);
                }
                else if (message.StartsWith("<FileValidation>"))
                {
                    ValidateAssets.Validate(message);
                }
                else
                {
                    List<ParsedMessage> parsedMessage = Parser.Parse(message);
                    myRichTextBox_MainText.AddFormatedText(parsedMessage);
                    ProcessTriggers(parsedMessage);

                    foreach (var item in parsedMessage)
                    {
                        _previousCommands.AddOnScreenWords(item.Message);
                    }
                }
            }

            if (myRichTextBox_MainText.Lines.Length > Settings.MaxLines)
            {
                myRichTextBox_MainText.BeginUpdate();

                myRichTextBox_MainText.SelectionStart = 0;
                myRichTextBox_MainText.SelectionLength = myRichTextBox_MainText.GetFirstCharIndexFromLine(myRichTextBox_MainText.Lines.Length - Settings.MaxLines);
                myRichTextBox_MainText.SelectedText = "";

                myRichTextBox_MainText.SelectionStart = myRichTextBox_MainText.Text.Length;
                myRichTextBox_MainText.ScrollToCaret();
                myRichTextBox_MainText.EndUpdate();
            }
        }

        private void ProcessTriggers(List<ParsedMessage> parsedMessages)
        {
            foreach (ParsedMessage parsedMessage in parsedMessages)
            {
                //because the message will probably have a line return at the end
                string message = parsedMessage.Message.Trim();
                List<Trigger.Trigger> matchedTriggers = null;
                if (_triggers.TryGetValue(parsedMessage.TagType, out matchedTriggers))
                {
                    foreach (Trigger.Trigger trigger in matchedTriggers)
                    {
                        if (trigger.RegexCompiled.IsMatch(message))
                        {
                            string command = trigger.CompiledCode.Invoke(message);

                            if (command != null)
                            {
                                _telnetHandler.OutQueue.Enqueue(command);
                            }
                        }
                    }
                }
            }
        }

        private void SaveFile(string message)
        {
            string newMessage = message.Remove(0, message.IndexOf("{"));
            newMessage = newMessage.Substring(0, newMessage.LastIndexOf("}") + 1);

            Data data = JsonConvert.DeserializeObject<Data>(newMessage, JsonMudMessage.Settings);
            Directory.CreateDirectory(Path.GetDirectoryName(data.AssetName));
            IFileIO io = new FileIO();
            io.WriteFileBase64(data.AssetName, data.Base64Encoding);
        }
        #endregion ReceiveMessage
        #endregion SendReceiveMessage
    }
}
