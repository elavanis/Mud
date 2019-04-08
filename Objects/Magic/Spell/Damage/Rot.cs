using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class Rot : BaseDamageSpell
    {
        public Rot() : base(nameof(Rot),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(90).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(90).Sides,
                            DamageType.Necrotic)
        {
            PerformerNotificationSuccess = new TranslationMessage("{performer} test {target}");
            RoomNotificationSuccess = new TranslationMessage("{performer} test {target}");
            TargetNotificationSuccess = new TranslationMessage("{performer} test {target}");
        }
    }
}
