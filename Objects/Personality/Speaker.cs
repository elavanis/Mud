using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Personality
{
    public class Speaker : ISpeaker
    {
        [ExcludeFromCodeCoverage]
        public int SpeakPercent { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public List<string> ThingsToSay { get; set; } = new List<string>();

        public string? Process(INonPlayerCharacter npc, string? command)
        {
            if (command == null)
            {
                if (GlobalReference.GlobalValues.Random.PercentDiceRoll(SpeakPercent))
                {
                    return SayMessage(npc);
                }
            }

            return command;
        }

        private string SayMessage(INonPlayerCharacter npc)
        {
            string message = ThingsToSay[GlobalReference.GlobalValues.Random.Next(ThingsToSay.Count)];
            return "Say " + message;
        }
    }
}
