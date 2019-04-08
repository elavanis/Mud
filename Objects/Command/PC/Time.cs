using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Command.PC
{
    public class Time : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Time", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Time" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            return new Result(GlobalReference.GlobalValues.GameDateTime.GameDateTime.ToString(), true);
        }
    }
}
