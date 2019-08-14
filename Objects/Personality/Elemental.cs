using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality
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
