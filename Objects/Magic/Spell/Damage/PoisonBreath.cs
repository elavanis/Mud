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
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
