using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.Stats;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Damage
{
    public class Damage : IDamage
    {
        public Damage(IDice dice, DamageType damageType)
        {
            Dice = dice;
            Type = damageType;
        }

        [ExcludeFromCodeCoverage]
        public IDice Dice { get; set; }

        [ExcludeFromCodeCoverage]
        public DamageType Type { get; set; } = DamageType.NotSet;

        [ExcludeFromCodeCoverage]
        public Stats.Stat? BonusDamageStat { get; set; }

        [ExcludeFromCodeCoverage]
        public Stats.Stat? BonusDefenseStat { get; set; }

        public enum DamageType
        {
            NotSet,
            Slash,
            Pierce,
            Bludgeon,
            Acid,
            Fire,
            Cold,
            Poison,
            Necrotic,
            Radiant,
            Lightning,
            Psychic,
            Thunder,
            Force
        }
    }
}
