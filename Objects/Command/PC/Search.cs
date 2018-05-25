using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Trap.Interface;

namespace Objects.Command.PC
{
    public class Search : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Search");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Search" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            StringBuilder strbBldr = new StringBuilder();

            foreach (ITrap trap in performer.Room.Traps)
            {
                strbBldr.AppendLine($"A trap was found in the {trap.DisarmWord[0]}.");
            }

            if (strbBldr.Length == 0)
            {
                return new Result(true, "Nothing found was found.");
            }
            else
            {
                return new Result(true, strbBldr.ToString().Trim());
            }
        }
    }
}
