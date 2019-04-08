using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities
{
    public class Aggressive : IPersonality
    {
        /// <summary>
        /// Will attack pc on sight
        /// </summary>
        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                foreach (IPlayerCharacter pc in npc.Room.PlayerCharacters)
                {
                    if (GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(npc, pc))
                    {
                        command = "Kill " + pc.KeyWords[0];
                        break;
                    }
                }
            }

            return command;
        }
    }
}
