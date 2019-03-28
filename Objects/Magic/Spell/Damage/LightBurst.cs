using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class LightBurst : BaseDamageSpell
    {
        public LightBurst() : base(nameof(LightBurst),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(80).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(80).Sides,
                            DamageType.Radiant)
        {
            PerformerNotificationSuccess = new TranslationMessage("Circling your hands around an imaginary sphere you push out.  Turning away the imaginary sphere burst into light blinding {target}.");
            RoomNotificationSuccess = new TranslationMessage("{performer} swirls their hands around an imaginary sphere.  They push the sphere towards {target} and quickly before light white blinding burst forth filling the room.");
            TargetNotificationSuccess = new TranslationMessage("{performer} swirls their hands around an imaginary sphere and pushes it towards you while quickly turning away.  A moment later bright light burst forth.");
        }
    }
}
