using Objects.Die;
using Objects.Effect;
using Objects.Global;
using Objects.Global.Language;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Interface;
using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using static Shared.TagWrapper.TagWrapper;

namespace All
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalReference.GlobalValues.Initilize();

            GenerateZones.Program.Main(null);

            GlobalReference.GlobalValues.Settings.AssetsDirectory = @"C:\Mud\Assets";
            GenerateZoneMaps.Program.Main(null);
        }
    }
}
