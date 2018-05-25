using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Spell.Heal.Health
{
    public class CureMinorWounds : BaseCureSpell
    {
        [ExcludeFromCodeCoverage]
        public CureMinorWounds() : base(nameof(CureMinorWounds), 2, 100)
        {
        }
    }
}
