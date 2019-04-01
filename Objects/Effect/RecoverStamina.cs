using Objects.Attribute.Effect;
using Objects.Effect.Interface;
using Objects.Interface;
using Objects.Mob.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Effect
{
    [DefenseEffect]
    public class RecoverStamina : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                mob.Stamina += parameter.Dice.RollDice();
                if (mob.Stamina > mob.MaxStamina)
                {
                    mob.Stamina = mob.MaxStamina;
                }
            }
        }
    }
}
