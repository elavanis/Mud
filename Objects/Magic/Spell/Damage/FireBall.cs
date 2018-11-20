using System;
using System.Collections.Generic;
using System.Text;
using Objects.Damage;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using static Objects.Damage.Damage;
using static Objects.Global.Language.Translator;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Magic.Spell.Damage
{
    public class FireBall : BaseDamageSpell
    {
        public FireBall() : base(nameof(FireBall),
                                GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(70).Die,
                                GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(70).Sides,
                                DamageType.Fire)
        {
            PerformerNotificationSuccess = new TranslationMessage("You thrust both hands toward {target} and say {SpellName}.  A fireball leaps through the air and grows until it lands on {target}.", TagType.Info, new List<ITranslationPair>() { new TranslationPair(Languages.Magic, "{SpellName}") });
            RoomNotificationSuccess = new TranslationMessage("{performer} thrust both hands toward {target} and says {SpellName}.  A giant ball of fire flies through the air towards {target} engulfing them in fire.", TagType.Info, new List<ITranslationPair>() { new TranslationPair(Languages.Magic, "{SpellName}") });
            TargetNotificationSuccess = new TranslationMessage("{performer} thrust both hands toward you and says {SpellName}.  A giant ball of fire flies through the air towards you.", TagType.Info, new List<ITranslationPair>() { new TranslationPair(Languages.Magic, "{SpellName}") });
        }
    }
}
