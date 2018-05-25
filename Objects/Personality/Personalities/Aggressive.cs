using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
