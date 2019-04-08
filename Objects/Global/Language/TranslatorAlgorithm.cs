using Objects.Global.Language.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Objects.Global.Language
{
    public class TranslatorAlgorithm : ITranslatorAlgorithm
    {
        public string CalculateHash(string inputString)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(inputString);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
