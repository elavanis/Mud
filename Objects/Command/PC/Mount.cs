using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Mount : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Mount() : base(nameof(Mount), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result(@"Mount {call}
Mount {mount}
Mount [pickup] [Mob Name]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Mount" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count <= 0)
            {
                return MountOrCall(performer);
            }
            else if (command.Parameters[0].ParameterValue.ToUpper() == "CALL"
                    || (command.Parameters[0].ParameterValue.ToUpper() == "MOUNT"))
            {
                return MountOrCall(performer);
            }
            else if (command.Parameters[0].ParameterValue.ToUpper() == "PICKUP"
                && command.Parameters.Count == 2)
            {
                return PickupRider(performer, command.Parameters[1]);
            }

            else return (Instructions);
        }

        private IResult MountOrCall(IMobileObject performer)
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

        private IResult PickupRider(IMobileObject performer, IParameter parameter)
        {
            IMount? mount = performer.Mount;

            if (mount == null)
            {
                return new Result("You do not own a mount.", true);
            }

            if (mount.Room != performer.Room)
            {
                return new Result("Your mount is not in the same room as you.", true);
            }

            if (mount.Riders.Count == 0
                || mount.Riders[0] != performer)
            {
                return new Result("You need to mount your mount before picking up additional riders.", true);
            }

            if (mount.MaxRiders <= mount.Riders.Count)
            {
                return new Result("Your mount can not carry additional riders.", true);
            }

            IMobileObject? mobToPickup = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameter.ParameterValue, parameter.ParameterNumber, false, false, true, true, false) as IMobileObject;

            if (mobToPickup != null)
            {
                if (mount.Riders.Contains(mobToPickup))
                {
                    return new Result($"{mobToPickup.SentenceDescription} is already riding with you.", true);
                }

                mount.Riders.Add(mobToPickup);
                return new Result($"You pickup {mobToPickup.SentenceDescription}.", true);
            }
            else
            {
                return new Result($"Unable to find {parameter.ParameterValue}.", true);
            }
        }

        private IResult SummonMount(IMobileObject performer)
        {
            IMount? mount = performer.Mount;

            if (mount != null)
            {
                GlobalReference.GlobalValues.World.AddMountToWorld(mount);
                mount.Riders.Clear();
                mount.Room?.RemoveMobileObjectFromRoom(mount);
                performer.Room.AddMobileObjectToRoom(mount);
                mount.Room = performer.Room;

                return new Result("Your mount has been called.", true);
            }
            else
            {
                return new Result("You have no mount.", true);
            }
        }

        private IResult MountYourMount(IMobileObject performer)
        {
            IMount? mount = performer.Mount;

            if (mount != null)
            {
                if (mount.Riders.Count > 0
                    && mount.Riders[0] == performer)
                {
                    return new Result($"You are already mounted on your {mount.SentenceDescription}.", true);
                }

                mount.Riders.Insert(0, performer);

                while (mount.Riders.Count > mount.MaxRiders)
                {
                    mount.Riders.RemoveAt(mount.Riders.Count - 1);
                }


                return new Result($"You mount your {mount.SentenceDescription}.", false);
            }
            else
            {
                return new Result("You have no mount.", true);
            }
        }
    }
}
