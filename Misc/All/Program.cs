using Objects.Global;
using Objects.Zone.Interface;
using RandomZone;
using System.Collections.Generic;
using System.IO;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            // List<IZone> zones = GenerateZones.Program.GenerateZones();

            List<IZone> zones = new List<IZone>();
            UndergroundChamber randomZone = new UndergroundChamber();
            randomZone.Generate(10, 10, 300, 2);
            zones.Add(randomZone.ConvertToZone(-2));

            using (TextWriter tw = new StreamWriter(@"C:\Git\Mud\Misc\GenerateZones\Zones\2.cs"))
            {
                tw.Write(zones[0].ToCsFile(2));
            }

            GenerateZoneMaps.Program.GenerateMaps(zones);
        }
    }
}
