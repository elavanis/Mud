using System.Diagnostics.CodeAnalysis;

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
