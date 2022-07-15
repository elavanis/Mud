using Mud;
using Objects.Global;
using Objects.Global.FileIO;
using Objects.Moon;
using Objects.Moon.Interface;
using ServerTelnetCommunication;
using Shared.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using TelnetCommunication;

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

            ConnectionHandler connectionHandler = new ConnectionHandler(new JsonMudMessage("",""));
        }

        private static void LoadServerSettings()
        {
            //WriteNewAppConfigFile();
            ConfigSettings config = GlobalReference.GlobalValues.Serialization.Deserialize<ConfigSettings>(File.ReadAllText("AppConfig.json"));

            PropertyInfo[] propertyInfosConfig = config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertyInfosSettings = GlobalReference.GlobalValues.Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfoConfig in propertyInfosConfig)
            {
                if (propertyInfoConfig.Name == "BannedIps")
                {
                    continue;
                }

                foreach (var propertyInfoSetting in propertyInfosSettings)
                {
                    if (propertyInfoConfig.Name == propertyInfoSetting.Name)
                    {
                        propertyInfoSetting.SetValue(GlobalReference.GlobalValues.Settings, propertyInfoConfig.GetValue(config));
                    }
                }
            }

            //GlobalReference.GlobalValues.Settings.AsciiArt = config.AsciiArt;
            //GlobalReference.GlobalValues.Settings.AssetsDirectory = config.AssetsDirectory;
            //GlobalReference.GlobalValues.Settings.BugDirectory = config.BugDirectory;
            //GlobalReference.GlobalValues.Settings.BulletinBoardDirectory = config.BulletinBoardDirectory;
            //GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory = config.PlayerCharacterDirectory;
            //GlobalReference.GlobalValues.Settings.StatsDirectory = config.StatsDirectory;
            //GlobalReference.GlobalValues.Settings.VaultDirectory = config.VaultDirectory;
            //GlobalReference.GlobalValues.Settings.ZoneDirectory = config.ZoneDirectory;
            //GlobalReference.GlobalValues.Settings.UseCachingFileIO = config.UseCachingFileIO;
            //GlobalReference.GlobalValues.Settings.Port = config.Port;
            //GlobalReference.GlobalValues.Settings.SendMapPosition = config.SendMapPosition;
            //GlobalReference.GlobalValues.Settings.LogStats = config.LogStats;
            //GlobalReference.GlobalValues.Settings.ElementalSpawnPercent = config.ElemenatlSpawnPercent;
            //GlobalReference.GlobalValues.Settings.RandomDropPercent = config.RandomDropPercent;
            //GlobalReference.GlobalValues.Settings.DropBeingPlusOnePercent = config.DropBeingPlusOnePercent;

            if (GlobalReference.GlobalValues.Settings.UseCachingFileIO)
            {
                List<string> permanentDirectories = new List<string>();
                permanentDirectories.Add(GlobalReference.GlobalValues.Settings.AssetsDirectory);
                permanentDirectories.Add(GlobalReference.GlobalValues.Settings.BulletinBoardDirectory);
                permanentDirectories.Add(GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory);
                permanentDirectories.Add(GlobalReference.GlobalValues.Settings.VaultDirectory);
                permanentDirectories.Add(GlobalReference.GlobalValues.Settings.ZoneDirectory);
                GlobalReference.GlobalValues.FileIO = new CachedFileIO(permanentDirectories, GlobalReference.GlobalValues.FileIO);
            }

            string[] ips = config.BannedIps.Split(',');
            foreach (string ip in ips)
            {
                if (IPAddress.TryParse(ip, out IPAddress? address))
                {
                    GlobalReference.GlobalValues.Settings.BannedIps.Add(address);
                }
            }

            GlobalReference.GlobalValues.Logger.Settings.LogDirectory = config.LogDirectory;
            GlobalReference.GlobalValues.WeaponId.Initialize();
        }

        private static void WriteNewAppConfigFile()
        {
            string assetsDirectory = "C:\\Mud\\Assets";
            string bannedIps = "";
            string bugDirectory = "C:\\Mud\\Bugs";
            string weaponIdDirectory = "C:\\Mud\\DamgeId";
            double dropBeingPlusOnePercent = 10;
            double elemenatlSpawnPercent = .01;
            string logDirectory = "C:\\Mud\\Logs";
            bool logStats = true;
            string statsDirectory = "C:\\Mud\\Stats";
            List<IMoon> moons = GetMoons();
            string playerCharacterDirectory = "C:\\Mud\\Players";
            int port = 52475;
            double randomDropPercent = 10;
            bool sendMapPosition = true;
            bool useCachingFileIO = true;
            string bulletinBoardDirectory = "C:\\Mud\\BulletinBoard";
            string vaultDirectory = "C:\\Mud\\Vaults";
            string zoneDirectory = "C:\\Mud\\World";

            string asciiArt =

@"                                   ▒▓▓░░▒▒▓░                               
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


            ConfigSettings configSettings = new ConfigSettings(assetsDirectory, bugDirectory, bulletinBoardDirectory, logDirectory, playerCharacterDirectory, vaultDirectory, weaponIdDirectory, zoneDirectory, logStats, statsDirectory, useCachingFileIO, port, sendMapPosition, bannedIps, elemenatlSpawnPercent, randomDropPercent, dropBeingPlusOnePercent, asciiArt, moons);


            using (TextWriter tw = new StreamWriter(@"AppConfig2.json"))
            {
                tw.Write(GlobalReference.GlobalValues.Serialization.Serialize(configSettings));
            }
        }

        private static List<IMoon> GetMoons()
        {
            //total magnification  is 1.5
            //this means that magic will be affected by at most 50% yielding a 1.5x increase and a .6666x decrease
            //IE 100 damage becomes 150 at full bonus and 66 at full decrease
            List<IMoon> moons = new List<IMoon>();

            IMoon moon = new Moon();
            //moon.MagicModifier = 1.14M;
            moon.MoonPhaseCycleDays = 13;
            moon.Name = "Luna";
            moons.Add(moon);

            moon = new Moon();
            //moon.MagicModifier = 1.135M;
            moon.MoonPhaseCycleDays = 28;
            moon.Name = "Selene";
            moons.Add(moon);

            moon = new Moon();
            //moon.MagicModifier = 1.165M;
            moon.MoonPhaseCycleDays = 80;
            moon.Name = "Callisto";
            moons.Add(moon);

            moon = new Moon();
            //moon.MagicModifier = 1.06M;
            moon.MoonPhaseCycleDays = 7;
            moon.Name = "Dione";
            moons.Add(moon);

            return moons;
        }
    }
}