using Objects.Command.Interface;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Global;
using static Objects.Room.Room;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Kill : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Kill() : base(nameof(Kill), ShortCutCharPositions.Standing) { }

        public IResult Instructions { get; } = new Result("(K)ill {Target}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "K", "Kill" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            IMobileObject target = null;


            if (command.Parameters.Count == 0)
            {
                //attack the first npc in the room.
                target = performer.Room.NonPlayerCharacters.FirstOrDefault();
            }
            else
            {
                string targetKeyword = command.Parameters[0].ParameterValue;
                int targetNumber = command.Parameters[0].ParameterNumber;
                target = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, targetKeyword, targetNumber, false, false, true, true) as IMobileObject;
            }

            if (target != null)
            {
                if (!performer.Room.Attributes.Contains(RoomAttribute.Peaceful))
                {
                    GlobalReference.GlobalValues.Engine.Combat.AddCombatPair(performer, target);
                    string message = string.Format("You begin to attack {0}.", target.SentenceDescription);
                    return new Result(message, false);
                }
                else
                {
                    return new Result("You were ready to attack but then you sense of peace rush over you and you decided not to attack.", true);
                }
            }
            else
            {
                if (command.Parameters.Count == 0)
                {
                    return new Result("Unable to find anything to kill.", true);
                }
                else
                {
                    return new Result("Unable to find anything that matches that description to kill.", true);
                }
            }
        }
    }
}
