using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global.Stats;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Damage
{
    public class Damage : IDamage
    {
        [ExcludeFromCodeCoverage]
        public Damage()
        {

        }

        public Damage(IDice dice)
        {
            Dice = dice;
        }


        [ExcludeFromCodeCoverage]
        public IDice Dice { get; set; }

        [ExcludeFromCodeCoverage]
        public DamageType Type { get; set; }

        [ExcludeFromCodeCoverage]
        public Stats.Stat? BonusDamageStat { get; set; }

        [ExcludeFromCodeCoverage]
        public Stats.Stat? BonusDefenseStat { get; set; }

        public enum DamageType
        {
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
