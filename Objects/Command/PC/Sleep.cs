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
    public class Sleep : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Sleep");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sleep" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            performer.Position = CharacterPosition.Sleep;
            GlobalReference.GlobalValues.Engine.Event.Sleep(performer);

            return new Result(true, "You lay down and goto sleep.");
        }
    }
}
