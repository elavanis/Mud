using System;
using System.Collections.Generic;
using System.Text;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;

namespace Objects.Skill.Skills.Generic
{
    public class SingleTargetSkill : BaseSkill
    {
        public override IResult ProcessSkill(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 1)
            {
                IBaseObject target = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, command.Parameters[1].ParameterValue, 0, true, true, true, true);

                if (target != null)
                {
                    Parameter.Target = target;
                    return base.ProcessSkill(performer, command);
                }
                else
                {
                    return new Result($"Unable to find {command.Parameters[1].ParameterValue}.", true);
                }
            }
            return new Result($"The skill {command.Parameters[0].ParameterValue} requires a target.", true);
        }
    }
}
