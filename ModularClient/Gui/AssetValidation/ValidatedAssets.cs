using Shared.FileIO;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.AssetValidation
{
    public static class ValidateAssets
    {
        public static Dictionary<string, string> AssetHashes = new Dictionary<string, string>();

        internal static void Validate(string message)
        {
            string messageWithOutTags = message.Replace("<FileValidation>", "").Replace("</FileValidation>", "");
            string[] messageSpit = messageWithOutTags.Split('|');
            string file = messageSpit[1];
            string hashedValue = messageSpit[2];

            if (hashedValue != "") //blank means something went wrong with the hashing on the server
            {
                MD5 mD5 = MD5.Create();
                IFileIO fileIO = new FileIO();
                byte[] byteHashValue = mD5.ComputeHash(fileIO.ReadBytes(file));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < byteHashValue.Length; i++)
                {
                    stringBuilder.Append(byteHashValue[i].ToString("X2"));
                }
                string existingHashedValue = stringBuilder.ToString();

                if (existingHashedValue != hashedValue)
                {
                    fileIO.Delete(file);
                }
                else
                {
                    try
                    {
                        AssetHashes.Add(file, hashedValue);
                    }
                    catch (Exception ex)
                    {
                        //We really shouldn't get this but I came across this error while holding key 
                        //so I guess it requested the map 2x before it had received it.
                        //Either way don't crash the app because of this.
                    }
                }
            }
        }
    }
}
