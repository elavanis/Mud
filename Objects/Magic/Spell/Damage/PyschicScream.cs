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
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
