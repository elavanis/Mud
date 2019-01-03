using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities
{
    public class Hydra : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            Mob.SpecificNPC.Hydra hydra = npc as Mob.SpecificNPC.Hydra;
            if (hydra != null)
            {

            }

            return command;
        }
    }
}
