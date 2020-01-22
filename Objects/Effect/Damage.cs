using System.Collections.Generic;
using Objects.Mob.Interface;
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
            if (parameter.Target is IMobileObject target)
            {
                GlobalReference.GlobalValues.Notify.Mob(parameter.Performer, parameter.Target, target, parameter.TargetMessage);

                int damageReceived = target.TakeDamage(parameter.Damage.Dice.RollDice(), parameter.Damage, parameter.Description);

                if (parameter.Performer is IMobileObject performer)
                {
                    GlobalReference.GlobalValues.Engine.Combat.AddCombatPair(performer, target);
                    GlobalReference.GlobalValues.Engine.Event.DamageAfterDefense(performer, target, damageReceived, parameter.Description);

                    if (Sound != null)
                    {
                        string serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { Sound });
                        GlobalReference.GlobalValues.Notify.Mob(target, new TranslationMessage(serializeSounds, TagType.Sound));
                    }
                }
            }
        }
    }
}
