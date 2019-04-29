using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities.EnchantersTower
{
    public class FollowPlayerAfterRescue : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                if (npc.FollowTarget == null)
                {
                    if (npc.Room.Zone == 16 && npc.Room.Id == 7) //don't start following in the room with the goblin chief
                    {
                        return command;
                    }

                    bool guardFound = false;
                    foreach (INonPlayerCharacter nonPlayerCharacter in npc.Room.NonPlayerCharacters)
                    {
                        if (nonPlayerCharacter.Zone == npc.Zone && nonPlayerCharacter.Id == 11)
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
            }

            return command;
        }
    }
}
