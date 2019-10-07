using Objects.Global;
using Objects.Zone.Interface;
using System.Collections.Generic;
using UndergroundChambers;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            // List<IZone> zones = GenerateZones.Program.GenerateZones();

            List<IZone> zones = new List<IZone>();
            UndergroundChamber undergroundChamber = new UndergroundChamber();
            undergroundChamber.Generate(10, 10, 3, 2);
            zones.Add(undergroundChamber.ConvertToZone(-2));

            GenerateZoneMaps.Program.GenerateMaps(zones);
        }
    }
}
