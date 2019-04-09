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
            PerformerNotificationSuccess = new TranslationMessage("You look at {target} with great anger in your eyes.  A force radiates from you knocking them back.");
            RoomNotificationSuccess = new TranslationMessage("{performer} looks at {target} with a great anger in their eyes.  {target} is knocked back by some type of invisible force.");
            TargetNotificationSuccess = new TranslationMessage("{performer} looks at you with anger in his eyes.  Suddenly you are knocked back by a great force.");
        }
    }
}
