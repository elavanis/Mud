using System.Diagnostics.CodeAnalysis;

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
