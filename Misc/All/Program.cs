using Objects.Global;
using Objects.Zone.Interface;
using System.Collections.Generic;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            List<IZone> zones = GenerateZones.Program.GenerateZones();

            //List<IZone> zones = new List<IZone>();
            //Maze.Maze maze = new Maze.Maze();
            //maze.Generate(10, 10, 50, 2);
            //zones.Add(maze.ConvertToZone(-2));

            GenerateZoneMaps.Program.GenerateMaps(zones);
        }
    }
}
