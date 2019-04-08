using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class Smite : BaseDamageSpell
    {
        public Smite() : base(nameof(Smite),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(100).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(100).Sides,
                            DamageType.Force)
        {
            PerformerNotificationSuccess = new TranslationMessage("{performer} test {target}");
            RoomNotificationSuccess = new TranslationMessage("{performer} test {target}");
            TargetNotificationSuccess = new TranslationMessage("{performer} test {target}");
        }
    }
}
