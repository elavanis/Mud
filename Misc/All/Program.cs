using Objects.Global;

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
