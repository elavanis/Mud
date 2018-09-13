using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.GrandViewGraveYard
{
    public class DeathDuringDay : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            //if its 12am or then the mobs need to die since its daylight
            if (GlobalReference.GlobalValues.GameDateTime.InGameDateTime.Hour < 12)
            {
                npc.Die();
                IContainer corpse = npc.Room.Items[0] as IContainer;
                if (corpse != null)
                {
                    corpse.Items.Clear();
                }
                return "";
            }
            else
            {
                return command;
            }
        }
    }
}
