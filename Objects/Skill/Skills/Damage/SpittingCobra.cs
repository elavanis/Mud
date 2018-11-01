using Objects.Damage.Interface;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class SpittingCobra : BaseDamageSkill
    {
        public SpittingCobra() : base(nameof(SpittingCobra),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Sides,
            DamageType.Poison)
        {
            PerformerNotification = new TranslationMessage("You spit the poison on {target}'s face.");
            RoomNotification = new TranslationMessage("{performer} spits some type of purple liquid on to {target}'s face.");
            TargetNotification = new TranslationMessage("{performer} spits a purple liquid onto your face.");
        }

        public override string TeachMessage => "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";

        public override void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {
            IEnchantment enchantment = new Magic.Enchantment.HeartbeatBigTickEnchantment();
            IEffect effect = new Objects.Effect.Damage();
            IEffectParameter effectParameter = new EffectParameter();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect;
            enchantment.Parameter = effectParameter;
            enchantment.EnchantmentEndingDateTime = DateTime.UtcNow.AddSeconds(10);

            effectParameter.TargetMessage = new TranslationMessage("Acid burns your face.");
            effectParameter.Damage = new Objects.Damage.Damage();
            effectParameter.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(28);
            effectParameter.Damage.Type = DamageType.Poison;

            target.Enchantments.Add(enchantment);
        }
    }
}