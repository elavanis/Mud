using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Manual : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Do you really need to Man the Manual?", false);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Man", "Manual" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count == 0)
            {
                return AllCommands(performer);
            }
            else
            {
                return ManualForCommand(command);
            }
        }

        private IResult AllCommands(IMobileObject performer)
        {
            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendLine("In Game Commands");
            if (performer is IPlayerCharacter pc
                && pc.God)
            {
                strBldr.AppendLine("God Commands");

                foreach (string str in GlobalReference.GlobalValues.CommandList.GodCommands.Keys)
                {
                    strBldr.AppendLine(str);
                }
                strBldr.AppendLine("");
                strBldr.AppendLine("Mortal Commands");
            }

            foreach (string str in GlobalReference.GlobalValues.CommandList.PcCommands.Keys)
            {
                strBldr.AppendLine(str);
            }
            return new Result(strBldr.ToString().Trim(), true);
        }

        private IResult ManualForCommand(ICommand command)
        {
            string commandToLookup = command.Parameters[0].ParameterValue.ToUpper();
            IMobileObjectCommand foundCommand = null;
            if (!GlobalReference.GlobalValues.CommandList.PcCommandsLookup.TryGetValue(commandToLookup, out foundCommand))
            {
                GlobalReference.GlobalValues.CommandList.GodCommandsLookup.TryGetValue(commandToLookup, out foundCommand);
            }

            if (foundCommand != null)
            {
                return foundCommand.Instructions;
            }
            else
            {
                return new Result("Unable to find that command.", true);
            }
        }
    }
}
