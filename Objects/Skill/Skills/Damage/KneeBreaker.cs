using Objects.Global;
using Objects.Language;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class KneeBreaker : BaseDamageSkill
    {
        public KneeBreaker() : base(nameof(KneeBreaker),
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(80).Die,
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(80).Sides,
          DamageType.Bludgeon)
        {
            PerformerNotificationSuccess = new TranslationMessage("Lift your foot high in the air you expertly bring it down on the knee of {target}.");
            RoomNotificationSuccess = new TranslationMessage("{performer} lifts their foot high and brings it down on the knee of {target}.");
            TargetNotificationSuccess = new TranslationMessage("{performer} lifts their foot and brings it down on your knee.");
        }

        public override void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {
            target.Stamina = target.Stamina / 2;
        }
    }
}
