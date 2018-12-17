using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Give : IMobileObjectCommand
    {
        public IResult Instructions => new Result("Give [Item Name]", true);

        public IEnumerable<string> CommandTrigger => new List<string>() { "Give" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
