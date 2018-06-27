using Objects.Global;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Room;
using Objects.Room.Interface;
using Objects.World;
using Objects.World.Interface;
using Objects.Zone;
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



            //for (int i = 100; i < 1000; i++)
            //{
            //    SaveZone(world, MassZone(i));
            //}
        }

        private static IZone MassZone(int i)
        {
            IZone zone = new Zone();
            zone.Id = i;
            int roomId = 1;
            zone.Name = i.ToString();

            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Wanderer());
            npc.Zone = i;
            npc.Id = 1;
            npc.ExamineDescription = "ExamineDescription";
            npc.LongDescription = "LongDescription";
            npc.ShortDescription = "ShortDescription";
            npc.SentenceDescription = "SentenceDescription";
            npc.KeyWords.Add("npc");
            npc.TypeOfMob = NonPlayerCharacter.MobType.Humanoid;
            for (int x = 0; x < 100; x++)
            {
                IRoom room = new Room();
                room.Id = roomId++;
                room.Zone = i;
                room.ExamineDescription = "ExamineDescription";
                room.LongDescription = "LongDescription";
                room.ShortDescription = "ShortDescription";
                room.SentenceDescription = "SentenceDescription";
                zone.Rooms.Add(room.Id, room);
                room.AddMobileObjectToRoom(npc);
            }

            return zone;
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
