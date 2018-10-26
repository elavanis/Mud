using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class SpittingCobra : BaseDamageSkill
    {
        public SpittingCobra() : base(nameof(Pierce),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(30).Sides,
            DamageType.Poison)
        {
            PerformerNotification = new TranslationMessage("You spit the poison on {target}'s face.");
            RoomNotification = new TranslationMessage("{performer} spits some type of purple liquid on to {target}'s face.");
            TargetNotification = new TranslationMessage("{performer} spits a purple liquid onto your face.");
        }

        public override string TeachMessage => "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";
    }
}