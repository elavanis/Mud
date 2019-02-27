using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.EnchantersTower
{
    public class FollowPlayerAfterRescue : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            if (npc.FollowTarget == null)
            {
                bool guardFound = false;
                foreach (INonPlayerCharacter nonPlayerCharacter in npc.Room.NonPlayerCharacters)
                {
                    if (nonPlayerCharacter.Zone == npc.Zone && nonPlayerCharacter.Id == 7)
                    {
                        guardFound = true;
                        break;
                    }
                }

                if (!guardFound)
                {
                    if (npc.Room.PlayerCharacters.Count > 0)
                    {
                        npc.EnqueueCommand($"Follow {npc.Room.PlayerCharacters[0].KeyWords[0]}");
                    }
                }
            }

            return command;
        }
    }
}
