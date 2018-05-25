using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class Freeze : BaseDamageSpell
    {
        public Freeze() : base(nameof(Freeze),
                              GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(40).Die,
                              GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(40).Sides,
                              DamageType.Cold)
        {
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
