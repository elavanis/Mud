using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;

namespace Objects.Item.Items.EnchantersTower
{
    public class EnchantingTable : Enchantery
    {
        public EnchantingTable() : base()
        {

        }
        public override IResult Enchant(IItem item)
        {
            IRoom room = GlobalReference.GlobalValues.World.Zones[23].Rooms[8];
            Container container = (Container)GlobalReference.GlobalValues.FindObjects.FindItemsInRoom(room, "pedestal").FirstOrDefault();

            if (container != null)
            {
                foreach (IItem itemsInPedistal in container.Items)
                {
                    if (itemsInPedistal.Zone == 16
                        && itemsInPedistal.Id == 1)
                    {
                        return base.Enchant(item);
                    }
                }
            }

            return new Result("The pedestal doesn't seem to be getting enough energy.  Maybe there needs to be something to focus the energy coming down toward it.", true);
        }
    }
}
