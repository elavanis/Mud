using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Spell.Heal.Health
{
    public class CureLightWounds : BaseCureSpell
    {
        [ExcludeFromCodeCoverage]
        public CureLightWounds() : base(nameof(CureLightWounds), 2, 1000)
        {
        }
    }
}
