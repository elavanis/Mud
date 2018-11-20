using Objects.Die;
using Objects.Effect;
using Objects.Global;
using Objects.Global.Language;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Spell.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Magic.Spell.Heal.Health
{
    public class BaseCureSpell : SingleTargetSpell
    {
        public BaseCureSpell(string spellName, int die, int sides, int manaCost = -1)
        {
            Effect = new RecoverHealth();
            Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.ReduceValues(die, sides);

            SpellName = spellName;

            SpellName = spellName;
            if (manaCost == -1)
            {
                ManaCost = sides * die / 20;
            }
            else
            {
                ManaCost = manaCost;
            }

            string roomMessage = "{performer} says {0} and their hands begin to glow causing {target} to look better.";
            string targetMessage = "{performer} says {0} and heals you.";
            string performerMessage = "You say {0} and heal {target}.";

            List<ITranslationPair> translate = new List<ITranslationPair>();
            ITranslationPair pair = new TranslationPair(Translator.Languages.Magic, spellName);
            translate.Add(pair);

            RoomNotificationSuccess = new TranslationMessage(roomMessage, TagType.Info, translate);
            TargetNotificationSuccess = new TranslationMessage(targetMessage, TagType.Info, translate);
            PerformerNotificationSuccess = new TranslationMessage(performerMessage, TagType.Info, translate);
        }
    }
}
