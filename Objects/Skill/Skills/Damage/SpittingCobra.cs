using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Magic.Enchantment.DefeatbleInfo;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System;
using static Objects.Damage.Damage;
using static Objects.Global.Stats.Stats;

namespace Objects.Skill.Skills.Damage
{
    public class SpittingCobra : BaseDamageSkill
    {
        public SpittingCobra() : base(nameof(SpittingCobra),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Sides,
            DamageType.Poison)
        {
            PerformerNotificationSuccess = new TranslationMessage("You spit the poison on {target}'s face.");
            RoomNotificationSuccess = new TranslationMessage("{performer} spits some type of purple liquid on to {target}'s face.");
            TargetNotificationSuccess = new TranslationMessage("{performer} spits a purple liquid onto your face.");
        }

        public override string TeachMessage {get;} = "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";

        protected override void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {
            IEnchantment enchantment = new Magic.Enchantment.HeartbeatBigTickEnchantment();
            IEffect effect = new Effect.Damage();
            IEffectParameter effectParameter = new EffectParameter();
            IDefeatInfo defeatInfo = new DefeatInfo();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect;
            enchantment.Parameter = effectParameter;
            enchantment.EnchantmentEndingDateTime = new DateTime(9999, 1, 1);
            enchantment.DefeatInfo = defeatInfo;
            defeatInfo.CurrentEnchantmentPoints = performer.ConstitutionEffective;
            defeatInfo.MobStat = Stat.Constitution;

            effectParameter.TargetMessage = new TranslationMessage("Poison burns your face.");
            effectParameter.Damage = new Objects.Damage.Damage();
            effectParameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(28);
            effectParameter.Damage.Type = DamageType.Poison;

            target.Enchantments.Add(enchantment);
        }
    }
}