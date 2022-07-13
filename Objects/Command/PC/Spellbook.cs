using Objects.Command.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Spellbook : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Spellbook() : base(nameof(Spellbook), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Spellbook", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Spellbook" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            IOrderedEnumerable<ISpell> spells = performer.SpellBook.Values.OrderBy(e => e.ManaCost);

            int nameLength = 0;
            foreach (ISpell spell in spells)
            {
                nameLength = Math.Max(nameLength, spell.AbilityName.Length);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Spell".PadRight(nameLength) + "  Mana Cost");
            foreach (ISpell spell in spells)
            {
                stringBuilder.AppendLine($"{spell.AbilityName.PadRight(nameLength)}  {spell.ManaCost}");
            }

            result = new Result(stringBuilder.ToString().Trim(), true, TagType.Info);

            return result;
        }
    }
}