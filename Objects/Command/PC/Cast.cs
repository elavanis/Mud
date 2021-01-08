using Objects.Command.Interface;
using Objects.Global;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Command.PC
{
    public class Cast : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Cast() : base(nameof(Cast), ShortCutCharPositions.Standing) { }

        public IResult Instructions { get; } = new Result("(C)ast [Spell Name] {Parameter(s)}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "C", "Cast" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

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