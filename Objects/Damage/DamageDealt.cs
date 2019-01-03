using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Damage
{
    public class DamageDealt
    {
        public DamageType DamageType { get; set; }
        public int Amount { get; set; }

        public DamageDealt(DamageType damageType, int amount)
        {
            DamageType = damageType;
            Amount = amount;
        }
    }
}
