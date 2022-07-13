using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Consider : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Consider() : base(nameof(Consider), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Consider [Mob]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Consider" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count <= 0)
            {
                return new Result("Who would you like to consider attacking?", true);
            }

            IParameter parm = command.Parameters[0];
            IMobileObject mob = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parm.ParameterValue, parm.ParameterNumber, false, false, true, true, false) as IMobileObject;

            if (mob != null)
            {
                return new Result(GlobalReference.GlobalValues.EvaluateLevelDifference.Evalute(performer, mob), true);
            }
            else
            {
                string message = string.Format("You were unable to find {0}.", command.Parameters[0].ParameterValue);
                return new Result(message, true);
            }
        }
    }
}
