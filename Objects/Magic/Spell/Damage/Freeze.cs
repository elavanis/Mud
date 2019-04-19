using Objects.Global;
using Objects.Global.Language;
using Objects.Language;
using static Objects.Damage.Damage;
using static Objects.Global.Language.Translator;

namespace Objects.Magic.Spell.Damage
{
    public class Freeze : BaseDamageSpell
    {
        public Freeze() : base(nameof(Freeze),
                              GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(40).Die,
                              GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(40).Sides,
                              DamageType.Cold)
        {
            string incantation = GlobalReference.GlobalValues.Translator.Translate(Languages.AncientMagic, nameof(Freeze));
            PerformerNotificationSuccess = new TranslationMessage("Speaking " + incantation + " your hands become cold and numb.  You blow across your hand causing {target} to be engulfed in a blizzard.");
            RoomNotificationSuccess = new TranslationMessage("{performer} speaks the words " + incantation + " before a blizzard at {target}.");
            TargetNotificationSuccess = new TranslationMessage("{performer} speaks " + incantation + " and blows a blizzard at you.");
        }
    }
}
