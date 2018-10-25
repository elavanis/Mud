using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class ShoulderBash : BaseDamageSkill
    {
        public ShoulderBash() : base(nameof(ShoulderBash),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(10).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(10).Sides,
            DamageType.Bludgeon)
        {
            PerformerNotification = new TranslationMessage("Lowering your shoulder you bash into {target} knocking them back slightly.");
            RoomNotification = new TranslationMessage("{performer} moves in close to {target} before lowering their shoulder and bashing into {target} knocking them back slightly.");
            TargetNotification = new TranslationMessage("{performer} knocks their shoulder into you knocking you back slightly.");
        }

        public override string TeachMessage => "The trick to a good shoulder bash it to get a firm stance before making contact with your opponent.";
    }
}