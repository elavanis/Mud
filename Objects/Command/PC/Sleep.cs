using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Sleep : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Sleep() : base(nameof(Sleep), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Sleep", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sleep" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            performer.Position = CharacterPosition.Sleep;
            GlobalReference.GlobalValues.Engine.Event.Sleep(performer);

            return new Result("You lay down and goto sleep.", false);
        }
    }
}
