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
                //check if it is night
                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour >= 12)
                {
                    //check to see if they are not in the house
                    if (npc.Room.Id != 26)
                    {
                        if (npc.Room.South != null)
                        {
                            npc.EnqueueCommand("South");
                        }
                        else if (npc.Room.East != null)
                        {
                            npc.EnqueueCommand("East");
                        }
                    }
                }
                else
                {
                    foreach (IItem item in npc.Room.Items)
                    {
                        if (item is ICorpse)
                        {
                            npc.EnqueueCommand($"Get {item.KeyWords[0]}");
                            npc.EnqueueCommand("Emote The groundskeeper starts digging a grave for the corpse.");
                            npc.EnqueueCommand("Wait");
                            npc.EnqueueCommand("Emote The groundskeeper places the body in the grave.");
                            npc.EnqueueCommand("Wait");
                            npc.EnqueueCommand("Say And stay there this time.");
                            return "";
                        }
                    }
                }
            }

            return command;
        }
    }
}
