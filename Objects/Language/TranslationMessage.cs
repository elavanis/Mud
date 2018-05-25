using Objects.Global;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Language
{
    public class TranslationMessage : ITranslationMessage
    {
        [ExcludeFromCodeCoverage]
        public string Message { get; set; }
        [ExcludeFromCodeCoverage]
        public List<ITranslationPair> TranslationPair { get; set; }

        public TranslationMessage(string message, TagType tagType = TagType.Info, List<ITranslationPair> translationPair = null)
        {
            this.Message = GlobalReference.GlobalValues.TagWrapper.WrapInTag(message, tagType);
            this.TranslationPair = translationPair;
        }

        public string GetTranslatedMessage(IMobileObject mob)
        {
            if (TranslationPair == null
                || mob == null)
            {
                return Message;
            }
            else
            {
                List<string> translatedParts = new List<string>();
                foreach (ITranslationPair pair in TranslationPair)
                {
                    translatedParts.Add(pair.GetTranslation(mob));
                }

                return string.Format(Message, translatedParts.ToArray());
            }
        }
    }
}
