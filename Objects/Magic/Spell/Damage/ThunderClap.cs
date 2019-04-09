using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class ThunderClap : BaseDamageSpell
    {
        public ThunderClap() : base(nameof(ThunderClap),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(10).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(10).Sides,
                            DamageType.Thunder)
        {
            PerformerNotificationSuccess = new TranslationMessage("Clapping your hands together you make a sound like thunder that hits {target}");
            RoomNotificationSuccess = new TranslationMessage("{performer} test {target}");
            TargetNotificationSuccess = new TranslationMessage("{performer} test {target}");
        }
    }
}
