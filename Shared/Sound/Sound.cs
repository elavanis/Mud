using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Sound
{
    public class Sound : ISound
    {
        private static Random _random = new Random();
        private string? _soundName = null;
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
