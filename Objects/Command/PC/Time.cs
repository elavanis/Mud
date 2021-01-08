using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Command.PC
{
    public class Time : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Time() : base(nameof(Time), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Time", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Time" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            return new Result(GlobalReference.GlobalValues.GameDateTime.GameDateTime.ToString(), true);
        }
    }
}
