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
    public class Stand : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Stand");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Stand" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            performer.Position = CharacterPosition.Stand;
            GlobalReference.GlobalValues.Engine.Event.Stand(performer);

            return new Result(true, "You stand up.");
        }
    }
}
