using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Enchantment
{
    public class DamageDealtBeforeDefenseEnchantment : BaseEnchantment
    {
        [ExcludeFromCodeCoverage]
        public bool TargetIsDefender { get; set; }

        public DamageDealtBeforeDefenseEnchantment(bool targetIsDefender = true)
        {
            TargetIsDefender = targetIsDefender;
        }

        public override void DamageDealtBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.ObjectRoom = attacker?.Room ?? defender?.Room;
                Parameter.Defender = defender;
                Parameter.Attacker = attacker;
                if (TargetIsDefender)
                {
                    Parameter.Target = defender;
                }
                else
                {
                    Parameter.Target = attacker;
                }
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
