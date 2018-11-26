using GenerateZones;
using Maps;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static GenerateZones.RandomZoneGeneration;

namespace GenerateZoneMaps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            GlobalReference.GlobalValues.Settings.AssetsDirectory = @"C:\Mud\Assets";
            Directory.CreateDirectory(Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, "Maps"));

            Type zoneCodeInterface = typeof(IZoneCode);
            IEnumerable<Type> zones = Assembly.GetAssembly(typeof(IZoneCode)).GetTypes().Where(e => zoneCodeInterface.IsAssignableFrom(e) && e.IsClass);

            foreach (Type type in zones)
            {
                IZoneCode zoneCode = (IZoneCode)Activator.CreateInstance(type);
                IZone zone = zoneCode.Generate();

                if (zone.Rooms.Count > 0)
                {
                    if (!ManualZones(zone))
                    {
                        Map map = new Map(zone);
                        map.GenerateMap();
                    }
                }
            }
        }

        #region Manual Zones
        private static bool ManualZones(IZone zone)
        {
            switch (zone.Id)
            {
                case 5:
                    GenerateZone5(zone);
                    break;
                case 11:
                    GenerateZone11(zone);
                    break;
                case 18:
                    GenerateZone18(zone);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static void GenerateZone18(IZone zone)
        {
            //RandomZoneGeneration randomZoneGeneration = new RandomZoneGeneration(10, 10, zone.Id);
            //RoomDescription description = new RoomDescription();
            //description.LongDescription = "";
            //description.ExamineDescription = "";
            //description.ShortDescription = "";
            //randomZoneGeneration.RoomDescriptions.Add(description);

            //IZone zone2 = randomZoneGeneration.Generate();

            //Map map = new Map(zone2);
            //map.GenerateMap();
        }

        private static void GenerateZone11(IZone zone)
        {
            Map map = new Map(null);
            MapGrid mapGrid = new MapGrid();
            mapGrid.Grid = new Dictionary<IRoom, MapRoom>();

            mapGrid.Grid.Add(zone.Rooms[1], new MapRoom(zone, zone.Rooms[1], new Position(1, 1, 1)));
            mapGrid.Grid.Add(zone.Rooms[2], new MapRoom(zone, zone.Rooms[2], new Position(1, 2, 1)));
            mapGrid.Grid.Add(zone.Rooms[3], new MapRoom(zone, zone.Rooms[3], new Position(2, 2, 1)));
            mapGrid.Grid.Add(zone.Rooms[4], new MapRoom(zone, zone.Rooms[4], new Position(3, 2, 1)));
            mapGrid.Grid.Add(zone.Rooms[5], new MapRoom(zone, zone.Rooms[5], new Position(3, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[6], new MapRoom(zone, zone.Rooms[6], new Position(2, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[7], new MapRoom(zone, zone.Rooms[7], new Position(2, 1, 1)));
            mapGrid.Grid.Add(zone.Rooms[8], new MapRoom(zone, zone.Rooms[8], new Position(3, 1, 1)));
            mapGrid.Grid.Add(zone.Rooms[9], new MapRoom(zone, zone.Rooms[9], new Position(1, 3, 1)));

            mapGrid.Grid.Add(zone.Rooms[11], new MapRoom(zone, zone.Rooms[11], new Position(1, 1, 2)));
            mapGrid.Grid.Add(zone.Rooms[12], new MapRoom(zone, zone.Rooms[12], new Position(1, 2, 2)));
            mapGrid.Grid.Add(zone.Rooms[13], new MapRoom(zone, zone.Rooms[13], new Position(2, 2, 2)));
            mapGrid.Grid.Add(zone.Rooms[14], new MapRoom(zone, zone.Rooms[14], new Position(3, 2, 2)));
            mapGrid.Grid.Add(zone.Rooms[15], new MapRoom(zone, zone.Rooms[15], new Position(3, 3, 2)));
            mapGrid.Grid.Add(zone.Rooms[16], new MapRoom(zone, zone.Rooms[16], new Position(2, 3, 2)));
            mapGrid.Grid.Add(zone.Rooms[17], new MapRoom(zone, zone.Rooms[17], new Position(2, 1, 2)));
            mapGrid.Grid.Add(zone.Rooms[18], new MapRoom(zone, zone.Rooms[18], new Position(3, 1, 2)));
            mapGrid.Grid.Add(zone.Rooms[19], new MapRoom(zone, zone.Rooms[19], new Position(1, 3, 2)));

            map.DrawGrid(mapGrid);
            map.BuildRoomToPositionConversion(mapGrid, zone.Id);
        }

        private static void GenerateZone5(IZone zone)
        {
            Map map = new Map(null);
            MapGrid mapGrid = new MapGrid();
            mapGrid.Grid = new Dictionary<IRoom, MapRoom>();

            mapGrid.Grid.Add(zone.Rooms[1], new MapRoom(zone, zone.Rooms[1], new Position(5, 1, 1)));
            mapGrid.Grid.Add(zone.Rooms[2], new MapRoom(zone, zone.Rooms[2], new Position(5, 2, 1)));
            mapGrid.Grid.Add(zone.Rooms[3], new MapRoom(zone, zone.Rooms[3], new Position(5, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[4], new MapRoom(zone, zone.Rooms[4], new Position(8, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[5], new MapRoom(zone, zone.Rooms[5], new Position(8, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[6], new MapRoom(zone, zone.Rooms[6], new Position(9, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[7], new MapRoom(zone, zone.Rooms[7], new Position(9, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[8], new MapRoom(zone, zone.Rooms[8], new Position(9, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[9], new MapRoom(zone, zone.Rooms[9], new Position(8, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[10], new MapRoom(zone, zone.Rooms[10], new Position(7, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[11], new MapRoom(zone, zone.Rooms[11], new Position(7, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[12], new MapRoom(zone, zone.Rooms[12], new Position(7, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[13], new MapRoom(zone, zone.Rooms[13], new Position(8, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[14], new MapRoom(zone, zone.Rooms[14], new Position(8, 7, 1)));
            mapGrid.Grid.Add(zone.Rooms[15], new MapRoom(zone, zone.Rooms[15], new Position(8, 8, 1)));
            mapGrid.Grid.Add(zone.Rooms[16], new MapRoom(zone, zone.Rooms[16], new Position(8, 9, 1)));
            mapGrid.Grid.Add(zone.Rooms[17], new MapRoom(zone, zone.Rooms[17], new Position(8, 10, 1)));
            mapGrid.Grid.Add(zone.Rooms[18], new MapRoom(zone, zone.Rooms[18], new Position(10, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[19], new MapRoom(zone, zone.Rooms[19], new Position(10, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[20], new MapRoom(zone, zone.Rooms[20], new Position(5, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[21], new MapRoom(zone, zone.Rooms[21], new Position(4, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[22], new MapRoom(zone, zone.Rooms[22], new Position(4, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[23], new MapRoom(zone, zone.Rooms[23], new Position(4, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[24], new MapRoom(zone, zone.Rooms[24], new Position(5, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[25], new MapRoom(zone, zone.Rooms[25], new Position(6, 6, 1)));
            mapGrid.Grid.Add(zone.Rooms[26], new MapRoom(zone, zone.Rooms[26], new Position(6, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[27], new MapRoom(zone, zone.Rooms[27], new Position(6, 4, 1)));
            mapGrid.Grid.Add(zone.Rooms[28], new MapRoom(zone, zone.Rooms[28], new Position(5, 5, 1)));
            mapGrid.Grid.Add(zone.Rooms[29], new MapRoom(zone, zone.Rooms[29], new Position(4, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[30], new MapRoom(zone, zone.Rooms[30], new Position(3, 3, 1)));
            mapGrid.Grid.Add(zone.Rooms[31], new MapRoom(zone, zone.Rooms[31], new Position(2, 3, 1)));

            map.DrawGrid(mapGrid);
            map.BuildRoomToPositionConversion(mapGrid, zone.Id);
        }

        #endregion Manual Zones
    }
}
