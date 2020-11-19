using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Follow : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Follow() : base(nameof(Follow), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Follow {Target}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Follow" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count == 0)
            {
                if (performer.FollowTarget == null)
                {
                    return new Result("You are not following anyone.", true);
                }
                else
                {
                    performer.FollowTarget = null;
                    string message = string.Format("You stop following {0}.", performer.FollowTarget.SentenceDescription);
                    return new Result(message, false);
                }
            }
            else
            {
                IParameter paramerter = command.Parameters[0];
                if (GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, paramerter.ParameterValue, paramerter.ParameterNumber, false, false, true, true) is IMobileObject mob)
                {
                    if (mob == performer)
                    {
                        string message = string.Format("You probably shouldn't follow yourself.  People might think your strange.", mob.SentenceDescription);
                        return new Result(message, true);
                    }
                    else
                    {
                        performer.FollowTarget = mob;
                        string message = string.Format("You start to follow {0}.", mob.SentenceDescription);
                        return new Result(message, false);
                    }
                }
                else
                {
                    string message = string.Format("Unable to find {0}.", paramerter.ParameterValue);
                    return new Result(message, true);
                }
            }
        }
    }
}
