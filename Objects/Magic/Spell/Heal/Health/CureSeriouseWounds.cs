using System.Diagnostics.CodeAnalysis;

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
