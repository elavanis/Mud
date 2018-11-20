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
            PerformerNotificationSuccess = new TranslationMessage("{performer} test {target}");
            RoomNotificationSuccess = new TranslationMessage("{performer} test {target}");
            TargetNotificationSuccess = new TranslationMessage("{performer} test {target}");
        }
    }
}
