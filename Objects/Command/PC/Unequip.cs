using Objects.Command.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Command.PC
{
    public class Unequip : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Unequip() : base(nameof(Unequip), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Unequip [Item Name]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Unequip" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count <= 0)
            {
                return new Result("What would you like to unequip?", true);
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
                return new Result(message, false);
            }
            else
            {
                string message = string.Format("You do not appear to have {0} equipped.", parameter.ParameterValue);
                return new Result(message, true);
            }
        }
    }
}
