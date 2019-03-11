using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities
{
    public class Elemental : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            Mob.SpecificNPC.Elemental elemental = npc as Mob.SpecificNPC.Elemental;
            if (elemental != null)
            {
                elemental.ProcessElementalTick();
            }

            return command;
        }
    }
}
