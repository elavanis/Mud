using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Dismount : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Dismount", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Dismount" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
