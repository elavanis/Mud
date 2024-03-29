﻿using System.Linq;
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
        public EnchantingTable(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        public override IResult Enchant(IItem item)
        {
            IRoom room = GlobalReference.GlobalValues.World.Zones[23].Rooms[8];
            IContainer container = GlobalReference.GlobalValues.FindObjects.FindItemsInRoom(room, "pedestal").FirstOrDefault() as IContainer;

            if (container != null)
            {
                foreach (IItem itemsInPedistal in container.Items)
                {
                    if (itemsInPedistal.ZoneId == 16
                        && itemsInPedistal.Id == 1)
                    {
                        return base.Enchant(item);
                    }
                }
            }

            return new Result("The pedestal doesn't seem to be getting enough energy.  Maybe there needs to be something to focus the energy coming down from the top of the tower.", true);
        }
    }
}
