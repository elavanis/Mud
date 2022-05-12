using Newtonsoft.Json;
using Shared.Sound.Interface;
using Shared.TagWrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelnetCommunication;
using WindowsClient.AssetValidation;

namespace WindowsClient.Sound
{
    public class SoundHandler
    {
        private TelnetHandler _telnetHandler;
        public SoundHandler(TelnetHandler telnetHandler)
        {
            _telnetHandler = telnetHandler;
        }
        public void HandleSounds(string message)
        {
            string newMessage = message.Remove(0, message.IndexOf("["));
            newMessage = newMessage.Substring(0, newMessage.LastIndexOf("]") + 1);

            List<ISound> sounds = JsonConvert.DeserializeObject<List<ISound>>(newMessage, JsonMudMessage.Settings);

            //engine.StopAllSounds();

            foreach (ISound sound in sounds)
            {
                string file = Path.Combine("Sounds", sound.SoundName);

                //request validation the file we have is the latest
                if (!ValidateAssets.AssetHashes.ContainsKey(file)
                    && File.Exists(file))
                {
                    _telnetHandler.OutQueue.Enqueue($"VALIDATEASSET|{file}");
                }

                if (!File.Exists(file))
                {
                    RequestSound(file);
                }
            }

            SoundManager.PlaySounds(sounds);
        }

        private void RequestSound(string file)
        {
            _telnetHandler.OutQueue.Enqueue(string.Format("RequestAsset|Sound|{0}", file));
        }

        internal void StopAll()
        {
            SoundManager.PlaySounds(new List<ISound>());
        }
    }
}
