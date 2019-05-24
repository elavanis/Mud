using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Consider : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Consider [Mob]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Consider" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
