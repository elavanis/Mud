using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class MagicMissle : BaseDamageSpell
    {
        public MagicMissle() : base(nameof(MagicMissle),
                           GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(1).Die,
                           GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(1).Sides,
                           DamageType.Force,
                           0)
        {
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
