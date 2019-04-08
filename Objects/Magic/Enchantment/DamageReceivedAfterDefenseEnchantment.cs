using Objects.Global;
using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Magic.Enchantment
{
    public class DamageReceivedAfterDefenseEnchantment : BaseEnchantment
    {
        [ExcludeFromCodeCoverage]
        public bool TargetIsDefender { get; set; }

        public DamageReceivedAfterDefenseEnchantment(bool targetIsDefender = false)
        {
            TargetIsDefender = targetIsDefender;
        }

        public override void DamageReceivedAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
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
