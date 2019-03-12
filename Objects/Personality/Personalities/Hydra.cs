using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
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
            IHydra hydra = npc as IHydra;
            if (hydra != null)
            {
                hydra.RegrowHeads();
            }

            return command;
        }
    }
}
