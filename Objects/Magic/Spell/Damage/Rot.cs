using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
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
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
