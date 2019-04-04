using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class PoisonBreath : BaseDamageSpell
    {
        public PoisonBreath() : base(nameof(PoisonBreath),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(60).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(60).Sides,
                            DamageType.Poison)
        {
            PerformerNotificationSuccess = new TranslationMessage("Like a blur you rush {target} and  blow green gas in {target} face leaving them choking.");
            RoomNotificationSuccess = new TranslationMessage("Like a blur {performer} rushes toward {target} as they blow a cloud of green gas in their face.");
            TargetNotificationSuccess = new TranslationMessage("One second you are fighting {performer} at arms length and the next they are in your face.  with a slightly evil grin they blow a cloud of noxious gas in your face leaving you to choke on the fumes.");
        }
    }
}
