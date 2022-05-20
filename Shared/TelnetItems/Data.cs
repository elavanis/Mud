using Shared.FileIO.Interface;
using Shared.TelnetItems.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Shared.TelnetItems
{
    public class Data : IData
    {
        [ExcludeFromCodeCoverage]
        public string AssetName { get; set; } 

        [ExcludeFromCodeCoverage]
        public DataType Type { get; set; } 

        [ExcludeFromCodeCoverage]
        public string Base64Encoding { get; set; } 


        public Data(DataType type, string fileLocation, IFileIO io, string assetName)
        {
            Base64Encoding = io.ReadFileBase64(fileLocation);

            Type = type;
            AssetName = assetName;
        }

        public enum DataType
        {
            File
        }

    }
}
