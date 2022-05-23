using Objects.Global;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality;
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
        public static void Main(string[] args)
        {

            GenerateZones();
        }

        public static List<IZone> GenerateZones()
        {
            List<IZone> compiledZones = new List<IZone>();
            GlobalReference.GlobalValues.Initilize();
            GlobalReference.GlobalValues.Settings.ZoneDirectory = @"C:\Mud\World";

            DeleteOldZoneFiles(GlobalReference.GlobalValues.Settings.ZoneDirectory);

            IWorld world = new World();
            GlobalReference.GlobalValues.World = world;

            Type zoneCodeInterface = typeof(IZoneCode);
            IEnumerable<Type> zones = Assembly.GetExecutingAssembly().GetTypes().Where(e => zoneCodeInterface.IsAssignableFrom(e) && e.IsClass);

            DeleteOldZoneFiles(GlobalReference.GlobalValues.Settings.ZoneDirectory);
            foreach (Type type in zones)
            {
                IZoneCode zone = (IZoneCode)Activator.CreateInstance(type);
                IZone builtZone = zone.Generate();
                SaveZone(world, builtZone);
                compiledZones.Add(builtZone);
            }

            using (TextWriter tw = new StreamWriter("..\\..\\..\\ZonesIds.txt"))
            {
                tw.WriteLine("Id\tLvl\tName");
                foreach (IZone zone in compiledZones.OrderBy(e => e.Id))
                {
                    List<int> mobLevel = new List<int>();
                    foreach (IRoom room in zone.Rooms.Values)
                    {
                        foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                        {
                            if (npc.Level == 0)
                            {
                                mobLevel.Add((npc.LevelRange.LowerLevel + npc.LevelRange.UpperLevel) / 2);
                            }
                            else
                            {
                                mobLevel.Add(npc.Level);
                            }
                        }
                    }

                    mobLevel.Sort();
                    int midLevel = 0;
                    if (mobLevel.Count > 0)
                    {
                        midLevel = mobLevel[mobLevel.Count / 2];
                    }

                    tw.WriteLine($"{zone.Id}\t{midLevel}\t{zone.Name}");
                }
            }

            return compiledZones;
        }

        private static IZone MassZone(int i)
        {
            IZone zone = new Zone();
            zone.Id = i;
            int roomId = 1;
            zone.Name = i.ToString();
           
            for (int x = 0; x < 100; x++)
            {
                IRoom room = new Room(x+1,i, "ExamineDescription", "LookDescription", "ShortDescription");
                room.Attributes.Add(Room.RoomAttribute.Indoor);
                zone.Rooms.Add(room.Id, room);

                INonPlayerCharacter npc = new NonPlayerCharacter(room, "corpseLookDescription", i, x, "ExamineDescription", "LookDescription", "SentenceDescription", "ShortDescription");
                npc.Personalities.Add(new Wanderer());
                npc.KeyWords.Add("npc");
                npc.TypeOfMob = NonPlayerCharacter.MobType.Humanoid;

                room.AddMobileObjectToRoom(npc);
            }

            return zone;
        }

        private static void DeleteOldZoneFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

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
                using (TextWriter tw = new StreamWriter(Path.Combine(GlobalReference.GlobalValues.Settings.ZoneDirectory, zone.Name + ".zone")))
                {
                    tw.Write(world.SerializeZone(zone));
                }
                Console.WriteLine(string.Format("Serialization for {0} complete.", zone.Name));
            }
        }
    }
}
