using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Push : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Push {Item Keyword}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Push" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (performer.Position == CharacterPosition.Sleep)
            {
                return new Result("You can not push things while asleep.", true);
            }

            if (command.Parameters.Count > 0)
            {
                IParameter parameter = command.Parameters[0];
                IBaseObject foundItem = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameter.ParameterValue, parameter.ParameterNumber, true, true, false, false, true);

                if (foundItem != null)
                {
                    return foundItem.Turn(performer, command);
                }
                else
                {
                    return new Result($"You could not find the {command.Parameters[0].ParameterValue} to push it.", true);
                }
            }
            else
            {
                return Instructions;
            }
        }
    }
}
