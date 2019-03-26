using Mud;
using Objects.Global;
using Objects.Moon;
using Objects.Moon.Interface;
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
            WriteNewAppConfigFile();
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

        private static void WriteNewAppConfigFile()
        {
            ConfigSettings configSettings = new ConfigSettings();
            configSettings.AssetsDirectory = "C:\\Mud\\Assets";
            configSettings.BannedIps = "";
            configSettings.BugDirectory = "C:\\Mud\\Bugs";
            configSettings.DropBeingPlusOnePercent = 10;
            configSettings.ElemenatlSpawnPercent = .01;
            configSettings.LogDirectory = "C:\\Mud\\Logs";
            configSettings.LogStats = true;
            configSettings.LogStatsLocation = "C:\\Mud\\Stats";
            configSettings.Moons = GetMoons();
            configSettings.PlayerCharacterDirectory = "C:\\Mud\\Players";
            configSettings.Port = 52475;
            configSettings.RandomDropPercent = 10;
            configSettings.SendMapPosition = true;
            configSettings.VaultDirectory = "C:\\Mud\\Vaults";
            configSettings.ZoneDirectory = "C:\\Mud\\World";
            configSettings.AsciiArt = @"                                   ▒▓▓░░▒▒▓░                               
                           ▒▓▓██████████████▓▓▒░                           
                       ▒████████████████████████████▓░                     
                   ░▓███████████████████████████████████▓▒                 
                 ▓████████████████████▓▓██████████████████████▓▒           
              ░███████████████▓▒▒          ▒▒▓▓▓▒▒▓▓▓█████████████▒        
            ▒██████████▓▓░                        ░▓▓▓██████████████       
          ░██████▓▓██▒                                 ░▓████████████      
         ▓█████████░                                      ▓███████████░    
        ██████▓██▒                                         ▒████▒▒█████▓   
       █████▓▓█▒                                              ▓██  ░█████░ 
     ░████████                                                  ██    ▒▓▒  
    ░████████                                                    ▓▓        
    ████▓██▓                                                         ░     
   ▓███▓▓█▒                                                          ░░    
  ░████▓█▒                                                            ░    
  ████▓█▓                                                              ▒▒  
  ██████                                                               ▒▓  
 ▓█████▓                                                                ▓▒ 
 ██████                                                                 ▓█ 
▒█████▒                                                                 ▓█░
▓████▓░                                                                 ▓▓▒
▒████▒░                                                                 ▓▓▒
▒████▒                                                                  ▓▓▓
░████▓▒                                                                 ▓▓▓
 ████▓                                                                  ▓▒▒
 ▓████                                                                 ▒▓▒▒
 ▒████░                                                                █▓▒▓
  ████▓                                                               ▓█ ▓░
  ▓████▒                                                              █▓░█ 
   █████                                                             ██ █▒ 
   ▒█████                                                           ██ ▒█  
    ▓█████                                                         ▓█░ █   
     ▒█████                                                       ▓█░ █▒   
      ▒█████                                                     ██░ █▓    
       ▒█████░                                                 ▒██ ▒█▓     
        ░█████▓                                               ▓█▓ ▓█▓      
          ▓█████▓                                           ▓██▒░██▒       
            ██████▓                                       ▓██▓▒▓█▓         
             ▒██████▓░                                 ▒████▓▓██░          
               ░███████▓▒                           ▒████▒▒▓▓▒░            
                  ▓█████████▒░                 ░▓█████▓▓▓▒▒▒░              
                    ░▓███████████▓▓▓▓▓▓▓▓▓▓███████▓▒▒▒▒▓▒▒░                
                        ░▒▓███████████████████▓▒▒▒▒▒▒▒░                    
                                    ░░░░░░▒▒▒▒▒░░                          ";

            using (TextWriter tw = new StreamWriter(@"C:\Git\Mud\Server\AppConfig2.json"))
            {
                tw.Write(GlobalReference.GlobalValues.Serialization.Serialize(configSettings));
            }
        }

        private static List<IMoon> GetMoons()
        {
            List<IMoon> moons = new List<IMoon>();

            IMoon moon = new Moon();
            moon.MagicModifier = 1.29M;
            moon.MoonPhaseCycleDays = 13;
            moon.Name = "Luna";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.05M;
            moon.MoonPhaseCycleDays = 28;
            moon.Name = "Selene";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.18M;
            moon.MoonPhaseCycleDays = 80;
            moon.Name = "Callisto";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.22M;
            moon.MoonPhaseCycleDays = 7;
            moon.Name = "Dione";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.14M;
            moon.MoonPhaseCycleDays = 61;
            moon.Name = "Pasiphae";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.09M;
            moon.MoonPhaseCycleDays = 29;
            moon.Name = "Elara";
            moons.Add(moon);

            moon = new Moon();
            moon.MagicModifier = 1.03M;
            moon.MoonPhaseCycleDays = 16;
            moon.Name = "Lysithea";
            moons.Add(moon);

            return moons;
        }
    }
}