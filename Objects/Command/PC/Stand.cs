using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Stand : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Stand() : base(nameof(Stand), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Stand", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Stand" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            performer.Position = CharacterPosition.Stand;
            GlobalReference.GlobalValues.Engine.Event.Stand(performer);

            return new Result("You stand up.", false);
        }
    }
}
