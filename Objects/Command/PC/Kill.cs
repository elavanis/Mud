using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Global;
using Objects.Interface;
using static Objects.Room.Room;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Kill : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "(K)ill {Target}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "K", "Kill" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IMobileObject target = null;

            if (performer.Position == CharacterPosition.Sleep)
            {
                return new Result(false, "You can not kill someone while you are asleep.");
            }

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
                    return new Result(true, message);
                }
                else
                {
                    return new Result(false, "You were ready to attack but then you sense of peace rush over you and you decided not to attack.");
                }
            }
            else
            {
                if (command.Parameters.Count == 0)
                {
                    return new Result(false, "Unable to find anything to kill.");
                }
                else
                {
                    return new Result(false, "Unable to find anything that matches that description to kill.");
                }
            }
        }
    }
}
