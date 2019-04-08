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
    public class Examine : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Examine {Object Name}", true);


        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Examine" };

        public IResult PerformCommand(IMobileObject mob, ICommand command)
        {
            IResult result = VerifyCanExamine(mob);
            //if we have a result it is because we can not examine
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count > 0)
            {
                IParameter param = command.Parameters[0];
                IBaseObject baseObject = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(mob, param.ParameterValue, param.ParameterNumber);
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
                return new Result(mob.Room.ExamineDescription, false, TagType.Room);
            }

        }

        private IResult VerifyCanExamine(IMobileObject performer)
        {
            if (!GlobalReference.GlobalValues.CanMobDoSomething.SeeDueToLight(performer))
            {
                return new Result("You can not see here because it is to dark.", true);
            }

            if (performer.Position == MobileObject.CharacterPosition.Sleep)
            {
                return new Result("You need to wake up before trying to examining things.", true);
            }

            return null;
        }

    }
}
