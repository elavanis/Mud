using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Sound
{
    public class Sound : ISound
    {
        private static Random _random = new Random();
        private string _soundName = null;
        [ExcludeFromCodeCoverage]
        public string SoundName
        {
            get
            {
                if (_soundName == null)
                {
                    return RandomSounds[_random.Next(RandomSounds.Count)];
                }
                else
                {
                    return _soundName;
                }
            }
            set
            {
                _soundName = value;
            }
        }

        [ExcludeFromCodeCoverage]
        public bool Loop { get; set; }

        [ExcludeFromCodeCoverage]
        public List<string> RandomSounds { get; set; } = new List<string>();
    }
}
