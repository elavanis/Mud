using Objects.Global.ValidateAsset.Interface;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Objects.Global.ValidateAsset
{
    public class ValidateAsset : IValidateAsset
    {
        private ConcurrentDictionary<string, string> assetHashes = new ConcurrentDictionary<string, string>();
        private MD5 md5 = MD5.Create();

        public string GetCheckSum(string message)
        {
            try
            {
                string[] splitMessage = message.Split('|');
                string asset = splitMessage[1];


                if (!assetHashes.TryGetValue(asset, out string hashedValue))
                {
                    lock (assetHashes)
                    {
                        if (!assetHashes.TryGetValue(asset, out hashedValue))
                        {
                            string fullPath = Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, asset);
                            byte[] fileBytes = GlobalReference.GlobalValues.FileIO.ReadBytes(fullPath);
                            byte[] byteHashValue = md5.ComputeHash(fileBytes);
                            StringBuilder stringBuilder = new StringBuilder();
                            for (int i = 0; i < byteHashValue.Length; i++)
                            {
                                stringBuilder.Append(byteHashValue[i].ToString("X2"));
                            }
                            hashedValue = stringBuilder.ToString();
                            assetHashes.TryAdd(asset, hashedValue);
                        }
                    }
                }

                return hashedValue;
            }
            catch
            {
                return null;
            }
        }

        public void ClearValidations()
        {
            assetHashes.Clear();
        }
    }
}