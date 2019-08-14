using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Custom.GrandViewGraveYard
{
    public class DeathDuringDay : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            //if its 12am or then the mobs need to die since its daylight
            if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour < 12)
            {
                npc.Die(null);

                IContainer corpse = npc.Room.Items[0] as IContainer;
                if (corpse != null)
                {
                    if (npc.Room.Items.Count > 5)
                    {
                        npc.Room.RemoveItemFromRoom(npc.Room.Items[0]);
                    }
                    else
                    {
                        corpse.Items.Clear();
                    }
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
