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
            if (elemental != null
                && GlobalReference.GlobalValues.TickCounter % 10 == 0) //only process every 10 ticks / 5 seconds
            {
                elemental.ProcessElementalTick();
            }

            return command;
        }
    }
}
