using System.Collections.Generic;

namespace Shared.Sound.Interface
{
    public interface ISound
    {
        bool Loop { get; set; }
        string SoundName { get; set; }
        List<string> RandomSounds { get; set; }
    }
}