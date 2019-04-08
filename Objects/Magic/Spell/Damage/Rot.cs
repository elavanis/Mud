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
            PerformerNotificationSuccess = new TranslationMessage("Waving your hand across your arm spots of rotting flesh begin to form.  Going back and your arm is healthy.  Suddenly {target} cries out in pain.  A cursory glance shows them a pale color with spots of decay.");
            RoomNotificationSuccess = new TranslationMessage("{performer} waves his hand back and forth across his arm causing boils and rotting flesh to appear and disappear.  As the afflictions leave {performer} they appear on {target}.");
            TargetNotificationSuccess = new TranslationMessage("{performer} waves his hand back and forth across his arm causing decay to first appear on his arm then disappear and reappear on you.");
        }
    }
}
