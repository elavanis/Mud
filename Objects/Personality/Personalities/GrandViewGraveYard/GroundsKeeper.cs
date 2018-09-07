using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.GrandViewGraveYard
{
    public class GroundsKeeper : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {

            if (command == null)
            {
                foreach (IItem item in npc.Room.Items)
                {
                    if (item is ICorpse)
                    {
                        npc.EnqueueCommand($"Get {item.KeyWords[0]}");
                        npc.EnqueueCommand("Emote The groudskeeper starts digging a grave for the corpse.");
                        npc.EnqueueCommand("Emote The groudskeeper places the body in the grave.");
                        npc.EnqueueCommand("Say And stay there this time.");
                        break;
                    }
                }
            }

            return command;
        }
    }
}
