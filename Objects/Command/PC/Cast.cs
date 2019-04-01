using Objects.Command.Interface;
using Objects.Global;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.PC
{
    public class Cast : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("(C)ast [Spell Name] {Parameter(s)}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "C", "Cast" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("What spell would you like to cast?", true);
            }

            performer.SpellBook.TryGetValue(command.Parameters[0].ParameterValue.ToUpper(), out ISpell spell);
            if (spell != null)
            {
                GlobalReference.GlobalValues.Engine.Event.Cast(performer, spell.SpellName);
                return spell.ProcessSpell(performer, command);
            }
            else
            {
                return new Result("You do not know that spell.", true);
            }
        }
    }
}