using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using static Objects.Mob.MobileObject;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Relax : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Relax() : base(nameof(Relax), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Relax", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Relax" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            performer.Position = CharacterPosition.Relax;
            GlobalReference.GlobalValues.Engine.Event.Relax(performer);

            return new Result("You lay down and relax.", false);
        }
    }
}
