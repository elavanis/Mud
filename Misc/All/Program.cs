﻿using Objects.Global;
using Objects.Global.FileIO;
using Objects.Zone.Interface;
using RandomZone;
using Shared.FileIO;
using System.Collections.Generic;
using System.IO;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            List<string> permanentDirectories = new List<string>();
            //permanentDirectories.Add(@"c:\temp");
            CachedFileIO cachedFileIO = new CachedFileIO(permanentDirectories, new FileIO());
            cachedFileIO.ReloadCache();


            List<IZone> zones = GenerateZones.Program.GenerateZones();

            //List<IZone> zones = new List<IZone>();
            //UndergroundChamber randomZone = new UndergroundChamber();

            //for (int i = 81; i < 82; i++)
            //{
            //    randomZone.Generate(20, 20, 10, i);
            //    zones.Add(randomZone.ConvertToZone(i * -1));
            //}



            //using (TextWriter tw = new StreamWriter(@"C:\Mud\Assets\Maps\2.cs"))
            //{
            //    tw.Write(zones[0].ToCsFile(2));
            //}

            GenerateZoneMaps.Program.GenerateMaps(zones);
        }
    }
}
