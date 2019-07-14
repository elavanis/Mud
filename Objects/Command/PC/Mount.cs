using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Mount : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(@"Mount {call}
Mount [pickup] [Mob Name]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Mount" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                if (performer.Mount == null)
                {
                    return new Result("You do not own a mount.", true);
                }

                if (performer.Mount.Room == performer.Room)
                {
                    return MountYourMount(performer);
                }
                else
                {
                    return SummonMount(performer);
                }
            }
            else if (command.Parameters[0].ParameterValue.ToUpper() == "CALL")
            {
                return SummonMount(performer);
            }
            else if (command.Parameters[0].ParameterValue.ToUpper() == "PICKUP"
                && command.Parameters.Count == 2)
            {
                return PickupRider(performer, command.Parameters[1]);
            }

            else return (Instructions);
        }



        private IResult PickupRider(IMobileObject performer, IParameter parameter)
        {
            if (performer.Mount == null)
            {
                return new Result("You do not own a mount.", true);
            }

            if (performer.Mount.Room != performer.Room)
            {
                return new Result("Your mount is not in the same room as you.", true);
            }

            if (performer.Mount.Riders.Count == 0)
            {
                return new Result("You need to mount your mount before picking up additional riders.", true);
            }

            if (performer.Mount.MaxRiders >= performer.Mount.Riders.Count)
            {
                return new Result("Your mount can not carry additional riders.", true);
            }

            IMobileObject mobToPickup = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameter.ParameterValue, parameter.ParameterNumber, false, true, true, true, false) as IMobileObject;

            if (mobToPickup != null)
            {
                performer.Mount.Riders.Add(mobToPickup);
                return new Result($"You pickup {mobToPickup.SentenceDescription}.", true);
            }
            else
            {
                return new Result($"Unable to find {parameter.ParameterValue}.", true);
            }

        }

        private IResult SummonMount(IMobileObject performer)
        {
            if (performer.Mount != null)
            {
                return new Result("You mount has been called.", true);
            }
            else
            {
                return new Result("You do not own a mount.", true);
            }
        }

        private IResult MountYourMount(IMobileObject performer)
        {
            if (performer.Mount == null)
            {
                return new Result("You do not own a mount.", true);
            }

            if (performer.Mount.Room != performer.Room)
            {
                return new Result("Your mount is not in the same room as you.", true);
            }

            if (performer.Mount.Riders.Count > 0
                && performer.Mount.Riders[0] == performer)
            {
                return new Result($"You are already mounted on your {performer.Mount.SentenceDescription}.", true);
            }

            performer.Mount.Riders.Add(performer);
            return new Result($"You mount your {performer.Mount.SentenceDescription}.", true);
        }
    }
}
