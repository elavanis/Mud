using Mud;
using Objects.Global;
using ServerTelnetCommunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelnetCommunication;
using TelnetCommunication.Interface;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Pause Profiling");
            //Console.ReadLine();

            MudInstance instance = new MudInstance();
            LoadServerSettings();

            instance.StartMud();

            //Console.WriteLine("Restart Profiling");
            //Console.ReadLine();

            ConnectionHandler connectionHandler = new ConnectionHandler(new JsonMudMessage());
        }

        private static void LoadServerSettings()
        {
            //ConfigSettings c = new ConfigSettings();
            //using (TextWriter tw = new StreamWriter(@"C:\Git\Mud\Server\AppConfig2.json"))
            //{
            //    tw.Write(GlobalReference.GlobalValues.Serialization.Serialize(c));
            //}
            ConfigSettings config = GlobalReference.GlobalValues.Serialization.Deserialize<ConfigSettings>(File.ReadAllText("AppConfig.json"));

            GlobalReference.GlobalValues.Settings.AsciiArt = config.AsciiArt;
            GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory = config.PlayerCharacterDirectory;
            GlobalReference.GlobalValues.Settings.ZoneDirectory = config.ZoneDirectory;
            GlobalReference.GlobalValues.Settings.AssetsDirectory = config.AssetsDirectory;
            GlobalReference.GlobalValues.Settings.VaultDirectory = config.VaultDirectory;
            GlobalReference.GlobalValues.Settings.BugDirectory = config.BugDirectory;
            GlobalReference.GlobalValues.Settings.Port = config.Port;
            GlobalReference.GlobalValues.Settings.SendMapPosition = config.SendMapPosition;
            GlobalReference.GlobalValues.Settings.LogStats = config.LogStats;
            GlobalReference.GlobalValues.Settings.LogStatsLocation = config.LogStatsLocation;
            GlobalReference.GlobalValues.Settings.ElementalSpawnPercent = config.ElemenatlSpawnPercent;
            GlobalReference.GlobalValues.Settings.RandomDropPercent = config.RandomDropPercent;
            GlobalReference.GlobalValues.Settings.DropBeingPlusOnePercent = config.DropBeingPlusOnePercent;

            string[] ips = config.BannedIps.Split(',');
            foreach (string ip in ips)
            {
                IPAddress address = null;
                if (IPAddress.TryParse(ip, out address))
                {
                    GlobalReference.GlobalValues.Settings.BannedIps.Add(address);
                }
            }

            GlobalReference.GlobalValues.Logger.Settings.LogDirectory = config.LogDirectory;
        }
    }
}