using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Magic.Spell.Generic
{
    public class SingleTargetSpell : BaseSpell
    {
        public override IResult ProcessSpell(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 1)
            {
                IBaseObject target = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, command.Parameters[1].ParameterValue, 0, true, true, true, true, true);

                if (target != null)
                {
                    SetParameters(performer, target);
                    return base.ProcessSpell(performer, command);
                }
                else
                {
                    return new Result($"Unable to find {command.Parameters[1].ParameterValue}.", true);
                }
            }
            else if (performer.IsInCombat)
            {
                IBaseObject target = performer.Opponent;

                if (target != null)
                {
                    SetParameters(performer, target);
                    return base.ProcessSpell(performer, command);
                }
                else
                {
                    return new Result("Unable to find an opponent to cast the spell on.", true);
                }
            }

            return new Result($"The spell {command.Parameters[0].ParameterValue} requires a target.", true);
        }

        private void SetParameters(IMobileObject performer, IBaseObject target)
        {
            Parameter.Performer = performer;
            Parameter.Target = target;
            Parameter.PerformerMessage = PerformerNotification;
            Parameter.RoomMessage = RoomNotification;
            Parameter.TargetMessage = TargetNotification;
        }
    }
}