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
            IMobileObject mob = parameter.Target as IMobileObject;
            if (mob != null)
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
