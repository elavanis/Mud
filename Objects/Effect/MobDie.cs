using Objects.Mob.Interface;
using Objects.Global;
using Objects.Effect.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    public class MobDie : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                IPlayerCharacter pc = mob as IPlayerCharacter;
                if (pc.God)
                {
                    //do not kill a god
                    return;
                }

                GlobalReference.GlobalValues.Notify.Mob(mob, parameter.TargetMessage);

                mob.Die(parameter.Attacker);
            }
        }
    }
}
