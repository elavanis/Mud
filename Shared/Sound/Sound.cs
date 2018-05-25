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
        [ExcludeFromCodeCoverage]
        public string SoundName { get; set; }

        [ExcludeFromCodeCoverage]
        public bool Loop { get; set; }

        [ExcludeFromCodeCoverage]
        public List<string> RandomSounds { get; set; } = new List<string>();
    }
}
