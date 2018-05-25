using Shared.FileIO.Interface;
using Shared.TelnetItems.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [ExcludeFromCodeCoverage]
        public Data()
        {

        }

        public Data(DataType type, string fileLocation, IFileIO io)
        {
            Base64Encoding = io.ReadFileBase64(fileLocation);

            Type = type;
            AssetName = fileLocation;
        }

        public enum DataType
        {
            File
        }

    }
}
