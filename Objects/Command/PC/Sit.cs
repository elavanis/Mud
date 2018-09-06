using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Sit : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Sit", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sit" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            performer.Position = CharacterPosition.Sit;
            GlobalReference.GlobalValues.Engine.Event.Sit(performer);

            return new Result("You sit down.", false);
        }
    }
}
