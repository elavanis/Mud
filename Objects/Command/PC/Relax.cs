using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Mob;
using static Objects.Mob.MobileObject;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Relax : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Relax", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Relax" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            performer.Position = CharacterPosition.Relax;
            GlobalReference.GlobalValues.Engine.Event.Relax(performer);

            return new Result("You lay down and relax.", false);
        }
    }
}
