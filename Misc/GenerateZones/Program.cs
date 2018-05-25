using Objects.Global;
using Objects.World;
using Objects.World.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GenerateZones
{
    public class Program
    {
        private static string zoneFiles = @"C:\Mud\World";

        public static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            DeleteOldZoneFiles(Directory.GetCurrentDirectory());

            IWorld world = new World();
            GlobalReference.GlobalValues.World = world;

            Type zoneCodeInterface = typeof(IZoneCode);
            IEnumerable<Type> zones = Assembly.GetExecutingAssembly().GetTypes().Where(e => zoneCodeInterface.IsAssignableFrom(e) && e.IsClass);

            DeleteOldZoneFiles(zoneFiles);
            foreach (Type type in zones)
            {
                IZoneCode zone = (IZoneCode)Activator.CreateInstance(type);
                SaveZone(world, zone.Generate());
            }
        }

        private static void DeleteOldZoneFiles(string directory)
        {
            foreach (string file in Directory.GetFiles(directory, "*.zone"))
            {
                File.Delete(file);
            }
        }

        private static void SaveZone(IWorld world, IZone zone)
        {
            if (zone != null)
            {
                ZoneVerify.VerifyZone(zone);
                GlobalReference.GlobalValues.World.Zones.Add(zone.Id, zone);

                Console.WriteLine(string.Format("Starting serialization for {0}.", zone.Name));
                using (TextWriter tw = new StreamWriter(Path.Combine(zoneFiles, zone.Name + ".zone")))
                {
                    tw.Write(world.SerializeZone(zone));
                }
                Console.WriteLine(string.Format("Serialization for {0} complete.", zone.Name));
            }
        }
    }
}
