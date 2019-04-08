using System.Diagnostics.CodeAnalysis;

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
