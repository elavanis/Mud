using Objects.Effect.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Mob.Interface;
using Objects.Attribute.Effect;

namespace Objects.Effect
{
    [DefenseEffect]
    public class RecoverHealth : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                mob.Health += parameter.Dice.RollDice();
                if (mob.Health > mob.MaxHealth)
                {
                    mob.Health = mob.MaxHealth;
                }
            }
        }
    }
}
