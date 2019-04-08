using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class RagingBull : BaseDamageSkill
    {
        public RagingBull() : base(nameof(RagingBull),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(1).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(1).Sides,
            DamageType.Bludgeon)
        {
            PerformerNotificationSuccess = new TranslationMessage("You lower your head and charge towards {target} hitting them with all your might.");
            RoomNotificationSuccess = new TranslationMessage("{performer} lowers their head and charges toward {target} hitting them with all their might.");
            TargetNotificationSuccess = new TranslationMessage("{performer} briefly lowered their head before crashing into you.");
        }

        public override string TeachMessage => "Lower your head and charge your enemy like a raging bull.  Finger horns are optional but encouraged.";
    }
}
