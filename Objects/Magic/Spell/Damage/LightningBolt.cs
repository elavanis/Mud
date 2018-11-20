using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class LightningBolt : BaseDamageSpell
    {
        public LightningBolt() : base(nameof(LightningBolt),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(30).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(30).Sides,
                            DamageType.Cold)
        {
            PerformerNotificationSuccess = new TranslationMessage("{performer} test {target}");
            RoomNotificationSuccess = new TranslationMessage("{performer} test {target}");
            TargetNotificationSuccess = new TranslationMessage("{performer} test {target}");
        }
    }
}
