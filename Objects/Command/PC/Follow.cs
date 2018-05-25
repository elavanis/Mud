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
    public class Follow : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Follow {Target}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Follow" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count == 0)
            {
                if (performer.FollowTarget == null)
                {
                    return new Result(false, "You are not following anyone.");
                }
                else
                {
                    performer.FollowTarget = null;
                    string message = string.Format("You stop following {0}.", performer.FollowTarget.SentenceDescription);
                    return new Result(true, message);
                }
            }
            else
            {
                IParameter paramerter = command.Parameters[0];
                IMobileObject mob = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, paramerter.ParameterValue, paramerter.ParameterNumber, false, false, true, true) as IMobileObject;

                if (mob != null)
                {
                    if (mob == performer)
                    {
                        string message = string.Format("You probably shouldn't follow yourself.  People might think your strange.", mob.SentenceDescription);
                        return new Result(true, message);
                    }
                    else
                    {
                        performer.FollowTarget = mob;
                        string message = string.Format("You start to follow {0}.", mob.SentenceDescription);
                        return new Result(true, message);
                    }
                }
                else
                {
                    string message = string.Format("Unable to find {0}.", paramerter.ParameterValue);
                    return new Result(false, message);
                }
            }
        }
    }
}
