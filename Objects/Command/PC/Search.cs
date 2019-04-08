using Objects.Command.Interface;
using System.Collections.Generic;
using System.Text;
using Objects.Mob.Interface;
using Objects.Trap.Interface;

namespace Objects.Command.PC
{
    public class Search : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Search", true);

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
                return new Result("Nothing found was found.", false);
            }
            else
            {
                return new Result(strbBldr.ToString().Trim(), false);
            }
        }
    }
}
