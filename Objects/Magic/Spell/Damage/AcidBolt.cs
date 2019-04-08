using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class AcidBolt : BaseDamageSpell
    {
        public AcidBolt() : base(nameof(AcidBolt),
                                GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(20).Die,
                                GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(20).Sides,
                                DamageType.Acid)
        {
            PerformerNotificationSuccess = new TranslationMessage("Acid leaps from you hands and lands on {target} burning their skin.");
            RoomNotificationSuccess = new TranslationMessage("Acid leaps from {performer} arms and lands {target} burning their skin.");
            TargetNotificationSuccess = new TranslationMessage("Acid leaps from {performer} arms and lands on you burning your skin.");
        }
    }
}
