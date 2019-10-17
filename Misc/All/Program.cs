using Objects.Global;
using Objects.Zone.Interface;
using RandomZone;
using RandomZone.Interface;
using System.Collections.Generic;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            // List<IZone> zones = GenerateZones.Program.GenerateZones();

            List<IZone> zones = new List<IZone>();
            IRandomZone randomZone = new Maze();
            randomZone.Generate(100, 100, 300, 2);
            zones.Add(randomZone.ConvertToZone(-2));


            GenerateZoneMaps.Program.GenerateMaps(zones);
        }
    }
}
