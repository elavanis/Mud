using System.Collections.Generic;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities.Interface
{
    public interface ISpeaker : IPersonality
    {
        int SpeakPercent { get; set; }
        List<string> ThingsToSay { get; set; }
    }
}