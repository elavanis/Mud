using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Sit : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Sit() : base(nameof(Sit), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Sit", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sit" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            performer.Position = CharacterPosition.Sit;
            GlobalReference.GlobalValues.Engine.Event.Sit(performer);

            return new Result("You sit down.", false);
        }
    }
}
