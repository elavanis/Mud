using System.Diagnostics.CodeAnalysis;
using static Objects.Damage.Damage;

namespace Objects.Damage
{
    public class DamageDealt
    {
        [ExcludeFromCodeCoverage]
        public DamageType DamageType { get; set; }
        [ExcludeFromCodeCoverage]
        public int Amount { get; set; }

        public DamageDealt(DamageType damageType, int amount)
        {
            DamageType = damageType;
            Amount = amount;
        }
    }
}
