using Objects.Global;
using Objects.Language;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class MagicMissle : BaseDamageSpell
    {
        public MagicMissle() : base(nameof(MagicMissle),
                           GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(1).Die,
                           GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(1).Sides,
                           DamageType.Force,
                           0)
        {
            PerformerNotificationSuccess = new TranslationMessage("With a flick of your wrist three magical darts fly from your hand striking {target}.");
            RoomNotificationSuccess = new TranslationMessage("Three magical darts fly from {performer} and strike {target}.");
            TargetNotificationSuccess = new TranslationMessage("{performer} makes a gesture with their hand causing three magical darts to strike you in the chest.");
        }
    }
}
