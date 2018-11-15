using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class PummelStrike : BaseDamageSkill
    {
        public PummelStrike() : base(nameof(PummelStrike),
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(70).Die,
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(70).Sides,
          DamageType.Bludgeon)
        {
            PerformerNotification = new TranslationMessage("Taking pummel of your weapon you strike it against {target}.");
            RoomNotification = new TranslationMessage("{performer} strikes their pummel against {target}.");
            TargetNotification = new TranslationMessage("{performer} hits you with their pummel.");
        }
    }
}
