using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Mob;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Interface;

namespace Objects.Command.PC
{
    public class Examine : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Examine() : base(nameof(Examine), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Examine {Object Name}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Examine" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            result = VerifyCanExamine(performer);
            //if we have a result it is because we can not examine
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count > 0)
            {
                IParameter param = command.Parameters[0];
                IBaseObject baseObject = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, param.ParameterValue, param.ParameterNumber);
                if (baseObject != null)
                {
                    TagType tagType = GlobalReference.GlobalValues.FindObjects.DetermineFoundObjectTagType(baseObject);
                    if (baseObject is IMobileObject mobileObject)
                    {
                        return new Result(baseObject.ExamineDescription + Environment.NewLine + mobileObject.HealthDescription, false, tagType);
                    }
                    else
                    {
                        return new Result(baseObject.ExamineDescription, false, tagType);
                    }
                }

                return new Result("You were unable to find that what you were looking for.", true);
            }
            else
            {
                return new Result(performer.Room.ExamineDescription, false, TagType.Room);
            }

        }

        private IResult VerifyCanExamine(IMobileObject performer)
        {
            if (!GlobalReference.GlobalValues.CanMobDoSomething.SeeDueToLight(performer))
            {
                return new Result("You can not see here because it is to dark.", true);
            }

            return null;
        }

    }
}
