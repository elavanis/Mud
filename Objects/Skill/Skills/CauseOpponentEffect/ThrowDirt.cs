using Objects.Command;
using Objects.Command.Interface;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Enchantment.DefeatbleInfo;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Global.Stats.Stats;

namespace Objects.Skill.Skills.CauseOpponentEffect
{
    public class ThrowDirt : BaseCauseOpponentEffect
    {
        public ThrowDirt() : base(nameof(ThrowDirt), 200)
        {
            Effect = new Objects.Effect.Blindness();
            PerformerNotificationSuccess = new TranslationMessage("You grab a hand full of dirt and throw it into {target} eyes.");
            RoomNotificationSuccess = new TranslationMessage("{performer} throws a hand full of dirt into {target} eyes temporarily blinding them.");
            TargetNotificationSuccess = new TranslationMessage("{performer} throws dirt into your eyes blinding you.");
        }

        public override void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {
            //notify the target what is happening since it doesn't happen earlier;
            ITranslationMessage message = new TranslationMessage(GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performer.SentenceDescription, target.SentenceDescription, TargetNotificationSuccess.Message));

            GlobalReference.GlobalValues.Notify.Mob(target, message);

            IEnchantment enchantment = new HeartbeatBigTickEnchantment();
            IEffect effect = new Blindness();
            IEffectParameter effectParameter = new EffectParameter();
            IDefeatInfo defeatInfo = new DefeatInfo();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect;
            enchantment.Parameter = effectParameter;
            enchantment.EnchantmentEndingDateTime = new DateTime(9999, 1, 1);
            enchantment.DefeatInfo = defeatInfo;
            defeatInfo.CurrentEnchantmentPoints = performer.StrengthEffective;
            defeatInfo.MobStat = Stat.Constitution;

            target.Enchantments.Add(enchantment);
        }
    }
}
