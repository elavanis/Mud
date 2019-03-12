using Objects.Global;
using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
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
            IElemental elemental = npc as IElemental;
            if (elemental != null)
            {
                elemental.ProcessElementalTick();
            }

            return command;
        }
    }
}
