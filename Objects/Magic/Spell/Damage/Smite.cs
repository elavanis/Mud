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
            PerformerNotificationSuccess = new TranslationMessage("{performer} looks at you with anger in his eyes.  Suddenly you are knocked back by a great force.");
            RoomNotificationSuccess = new TranslationMessage("{performer} looks at {target} with a great anger in their eyes.  {target} is knocked back by some type of invisible force.");
            TargetNotificationSuccess = new TranslationMessage("{performer} looks at you with great anger.  Suddenly a great force knocks you back.");
        }
    }
}
