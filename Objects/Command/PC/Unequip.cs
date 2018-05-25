using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.PC
{
    public class Unequip : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Unequip [Item Name]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Unequip" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result(false, "What would you like to unequip?");
            }

            IEquipment equippedItem = null;
            IParameter parameter = command.Parameters[0];
            IList<IEquipment> foundEquipedItems = performer.EquipedEquipment.Where(e => e.KeyWords.Any(f => f.Equals(parameter.ParameterValue, StringComparison.CurrentCultureIgnoreCase))).ToList();

            if (foundEquipedItems.Count > parameter.ParameterNumber)
            {
                equippedItem = foundEquipedItems[parameter.ParameterNumber];
            }

            if (equippedItem != null)
            {
                performer.RemoveEquipment(equippedItem);
                performer.Items.Add(equippedItem);
                performer.ResetMaxStatValues();
                string message = string.Format("You removed {0}.", equippedItem.SentenceDescription);
                return new Result(true, message);
            }
            else
            {
                string message = string.Format("You do not appear to have {0} equipped.", parameter.ParameterValue);
                return new Result(false, message);
            }
        }
    }
}
