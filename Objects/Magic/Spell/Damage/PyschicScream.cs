using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class PyschicScream : BaseDamageSpell
    {
        public PyschicScream() : base(nameof(PyschicScream),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(50).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(50).Sides,
                            DamageType.Cold)
        {
            PerformerNotificationSuccess = new TranslationMessage("Closing your eyes you and using your minds voice you scream at {target}.");
            RoomNotificationSuccess = new TranslationMessage("{performer} closes their eyes and {target} begins to scream in terror covering their ears.");
            TargetNotificationSuccess = new TranslationMessage("{performer} closes their eyes.  Suddenly you hear the sound screaming as if from a banshee.");
        }
    }
}
