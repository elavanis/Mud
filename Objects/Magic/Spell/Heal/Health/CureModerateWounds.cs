using System.Diagnostics.CodeAnalysis;

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
