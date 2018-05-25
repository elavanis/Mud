using Objects.Effect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Global;
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
            IMobileObject mob = parameter.Target as IMobileObject;
            if (mob != null)
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
