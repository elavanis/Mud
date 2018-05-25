using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Spell.Heal.Health
{
    public class CureSeriouseWounds : BaseCureSpell
    {
        [ExcludeFromCodeCoverage]
        public CureSeriouseWounds() : base(nameof(CureSeriouseWounds), 2, 100000)
        {
        }
    }
}
