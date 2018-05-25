using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Item.Interface;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Equip : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Equip [Item Name]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Equip" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                StringBuilder strBldr = new StringBuilder();
                foreach (Equipment.AvalableItemPosition position in Enum.GetValues(typeof(Equipment.AvalableItemPosition)))
                {
                    if (position != Equipment.AvalableItemPosition.NotWorn)
                    {
                        bool found = false;
                        foreach (IEquipment equipment in performer.EquipedEquipment)
                        {
                            if (equipment.ItemPosition == position)
                            {
                                strBldr.AppendLine(position.ToString() + " " + equipment.ShortDescription);
                            }
                        }

                        if (!found)
                        {
                            strBldr.AppendLine(position.ToString() + " <Nothing>");
                        }
                    }
                }
                return new Result(false, strBldr.ToString().Trim());
            }

            IParameter parm = command.Parameters[0];
            IItem item = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, parm.ParameterValue, parm.ParameterNumber);

            if (item != null)
            {
                IEquipment equipment = item as IEquipment;
                if (equipment == null
                    || equipment.ItemPosition == Equipment.AvalableItemPosition.NotWorn)
                {
                    string message = string.Format("You can not equip the {0}.", item.SentenceDescription);
                    return new Result(false, message);
                }

                IEquipment equipedItem = performer.EquipedEquipment.Where(i => i.ItemPosition == equipment.ItemPosition).FirstOrDefault();
                if (equipedItem != null)
                {
                    string message = string.Format("You already have {0} in the {1} position.", equipedItem.SentenceDescription, equipedItem.ItemPosition);
                    return new Result(false, message);
                }
                else
                {
                    performer.Items.Remove(equipment);
                    performer.AddEquipment(equipment);
                    string message = string.Format("You equipped the {0}.", equipment.SentenceDescription);
                    return new Result(true, message);
                }
            }
            else
            {
                string message = string.Format("You were unable to find {0}.", parm.ParameterValue);
                return new Result(false, message);
            }
        }
    }
}
