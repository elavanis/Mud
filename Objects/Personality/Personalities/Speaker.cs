using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Personality.Personalities
{
    public class Speaker : ISpeaker
    {
        [ExcludeFromCodeCoverage]
        public int SpeakPercent { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public List<string> ThingsToSay { get; set; } = new List<string>();

        public string Process(INonPlayerCharacter npc, string command)
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
