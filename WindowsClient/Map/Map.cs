using Shared.FileIO;
using Shared.TagWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TelnetCommunication;
using WindowsClient.AssetValidation;

namespace WindowsClient.Map
{
    public partial class Map : Form
    {
        private TelnetHandler _telnetHandler;
        private string _oldMapFileName;
        public Map(TelnetHandler telnetHandler)
        {
            _telnetHandler = telnetHandler;
            _oldMapFileName = null;
            InitializeComponent();
            pictureBox_Map.BackgroundImage = new Bitmap("map\\map.jpg");

            pictureBox_Map.BackColor = Color.Transparent;
        }

        public void Update(string rawMessage)
        {
            Message message = new Message(rawMessage);
            string fileName = Path.Combine("Maps", $"{message.Zone}-{message.Z}.png");

            if (_oldMapFileName != fileName) //don't bother reloading the map if it will be the same
            {
                if (!File.Exists(fileName))
                {
                    RequestMap(fileName);
                    return;
                }
                else
                {
                    //request validation the file we have is the latest
                    if (!ValidateAssets.AssetHashes.ContainsKey(fileName)
                        && File.Exists(fileName))
                    {
                        TagWrapper tagWrapper = new TagWrapper();

                        _telnetHandler.OutQueue.Enqueue($"VALIDATEASSET|{ fileName}");
                    }

                    Image oldImage = pictureBox_Map.Image;
                    Image map;

                    using (MemoryStream ms = new MemoryStream(new FileIO().ReadBytes(fileName)))
                    {
                        map = Image.FromStream(ms);
                    }


                    using (Graphics g = Graphics.FromImage(map))
                    {
                        Brush brush = new SolidBrush(Color.Red);
                        g.FillRectangle(brush, int.Parse(message.X), ReverseY(int.Parse(message.Y), map) - 10, 10, 10);
                    }
                    pictureBox_Map.Image = map;

                    oldImage?.Dispose();
                }
            }
        }

        private float ReverseY(int y, Image map)
        {
            return map.Height - y;
        }

        private void RequestMap(string file)
        {
            _telnetHandler.OutQueue.Enqueue(string.Format("RequestAsset|Map|{0}", file));
        }

        private class Message
        {
            public string Zone { get; set; }
            public string Z { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public Message(string message)
            {
                message = message.Replace("<Map>", "").Replace("</Map>", "");
                string[] array = message.Split('|');
                Zone = array[0];
                Z = array[1];
                X = array[2];
                Y = array[3];
            }
        }
    }
}
