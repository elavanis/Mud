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
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
