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
            PerformerNotificationSuccess = new TranslationMessage("You clap your hands together at {target} and a large crack like thunder shakes them to the bone.");
            RoomNotificationSuccess = new TranslationMessage("{performer} claps their hands at {target} and a large crack of thunder fills the air.");
            TargetNotificationSuccess = new TranslationMessage("{performer} claps their hands in front of them the loud crack of thunder rattles your bones.");
        }
    }
}
