using Objects.Global.Engine.Engines.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Global.Direction;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Command.Interface;
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
            IMobileObject mob = parameter.Target as IMobileObject;
            if (mob != null)
            {
                IPlayerCharacter pc = mob as IPlayerCharacter;
                if (pc.God)
                {
                    //do not kill a god
                    return;
                }

                GlobalReference.GlobalValues.Notify.Mob(mob, parameter.TargetMessage);

                mob.Die();
            }
        }
    }
}
