using System.Collections.Generic;
using Objects.Personality.Interface;

namespace Objects.Personality.Interface
{
    public interface ISpeaker : IPersonality
    {
        int SpeakPercent { get; set; }
        List<string> ThingsToSay { get; set; }
    }
}