using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Spell.Heal.Health
{
    public class CureCriticalWounds : BaseCureSpell
    {
        [ExcludeFromCodeCoverage]
        public CureCriticalWounds() : base(nameof(CureCriticalWounds), 2, 1000000)
        {
        }
    }
}
