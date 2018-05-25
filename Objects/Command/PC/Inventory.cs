using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Inventory : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "(Inv)entory");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Inv", "Inventory" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            StringBuilder strBldr = new StringBuilder();
            foreach (IItem item in performer.Items)
            {
                strBldr.AppendLine(item.ShortDescription);
            }

            string message = strBldr.ToString().Trim();
            if (message != "")
            {
                return new Result(true, message, TagType.Item);
            }
            else
            {
                return new Result(true, "You are not carrying anything.");
            }
        }
    }
}
