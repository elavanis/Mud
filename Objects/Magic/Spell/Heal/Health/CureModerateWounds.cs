using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Spell.Heal.Health
{
    public class CureModerateWounds : BaseCureSpell
    {
        [ExcludeFromCodeCoverage]
        public CureModerateWounds() : base(nameof(CureModerateWounds), 2, 10000)
        {
        }
    }
}
