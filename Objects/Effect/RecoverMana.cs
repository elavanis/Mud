using Objects.Attribute.Effect;
using Objects.Effect.Interface;
using Objects.Mob.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    [DefenseEffect]
    public class RecoverMana : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                mob.Mana += parameter.Dice.RollDice();
                if (mob.Mana > mob.MaxMana)
                {
                    mob.Mana = mob.MaxMana;
                }
            }
        }
    }
}
