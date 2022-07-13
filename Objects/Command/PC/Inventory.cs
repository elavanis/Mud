using Objects.Command.Interface;
using System.Collections.Generic;
using System.Text;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Inventory : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Inventory() : base(nameof(Inventory), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("(Inv)entory", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Inv", "Inventory" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            StringBuilder strBldr = new StringBuilder();
            foreach (IItem item in performer.Items)
            {
                strBldr.AppendLine(item.ShortDescription);
            }

            string message = strBldr.ToString().Trim();
            if (message != "")
            {
                return new Result(message, true, TagType.Item);
            }
            else
            {
                return new Result("You are not carrying anything.", true);
            }
        }
    }
}
