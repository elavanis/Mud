using Objects.Global;
using Objects.Global.Language;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class LightningBolt : BaseDamageSpell
    {
        public LightningBolt() : base(nameof(LightningBolt),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(30).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(30).Sides,
                            DamageType.Cold)
        {
            string incantation = GlobalReference.GlobalValues.Translator.Translate(Translator.Languages.AncientMagic, nameof(LightningBolt));
            PerformerNotificationSuccess = new TranslationMessage("Speaking " + incantation + " a small storm cloud appears before you sending a bolt of lighting toward {target}.");
            RoomNotificationSuccess = new TranslationMessage("{performer} says " + incantation + " and a small storm cloud appears sending a bolt of lighting toward {target}.");
            TargetNotificationSuccess = new TranslationMessage("{performer} says " + incantation + " and a small storm cloud appears sending a bolt of lighting toward you.");
        }
    }
}
