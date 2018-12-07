using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Give : IMobileObjectCommand
    {
        public IResult Instructions => throw new NotImplementedException();

        public IEnumerable<string> CommandTrigger => throw new NotImplementedException();

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
