using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Command.Interface;
using Objects.Interface;
using Objects.Mob.Interface;
using static Objects.Damage.Damage;
using Objects.Global;
using Objects.Effect.Interface;
using Shared.Sound.Interface;
using static Shared.TagWrapper.TagWrapper;
using System.Diagnostics.CodeAnalysis;
using Objects.Language;

namespace Objects.Effect
{
    public class Damage : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        [ExcludeFromCodeCoverage]
        public Damage()
        {

        }

        public Damage(ISound sound)
        {
            Sound = sound;
        }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                GlobalReference.GlobalValues.Notify.Mob(parameter.Performer, parameter.Target, mob, parameter.TargetMessage);

                mob.TakeDamage(parameter.Damage.Dice.RollDice(), parameter.Damage, null);

                if (Sound != null)
                {
                    string serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { Sound });
                    GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(serializeSounds, TagType.Sound));
                }
            }
        }
    }
}
