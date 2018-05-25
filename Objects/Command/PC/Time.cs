using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.PC
{
    public class Time : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Time");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Time" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            return new Result(true, GlobalReference.GlobalValues.GameDateTime.InGameFormatedDateTime);
        }
    }
}
