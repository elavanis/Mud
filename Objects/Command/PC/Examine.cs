using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Mob;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Item.Items;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Interface;

namespace Objects.Command.PC
{
    public class Examine : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Examine {Object Name}");


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
                    return new Result(true, baseObject.ExamineDescription, tagType);
                }

                return new Result(false, "You were unable to find that what you were looking for.");
            }
            else
            {
                return new Result(true, mob.Room.ExamineDescription, TagType.Room);
            }

        }

        private IResult VerifyCanExamine(IMobileObject performer)
        {
            if (!GlobalReference.GlobalValues.CanMobDoSomething.SeeDueToLight(performer))
            {
                return new Result(false, "You can not see here because it is to dark.");
            }

            if (performer.Position == MobileObject.CharacterPosition.Sleep)
            {
                return new Result(false, "You need to wake up before trying to examining things.");
            }

            return null;
        }

    }
}
