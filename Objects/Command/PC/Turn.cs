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
    public class Turn : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Turn() : base(nameof(Turn), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Turn {Item Keyword}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Turn" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
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
                    return new Result($"You could not find the {command.Parameters[0].ParameterValue} to turn it.", true);
                }
            }
            else
            {
                return Instructions;
            }
        }
    }
}
