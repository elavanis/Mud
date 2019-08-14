using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality
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
