using Objects.Global;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;
using static Objects.Global.Language.Translator;

namespace Objects.Language
{
    public class TranslationPair : ITranslationPair
    {
        [ExcludeFromCodeCoverage]
        public Languages Language { get; set; }
        [ExcludeFromCodeCoverage]
        public string TranslationPart { get; set; }


        public TranslationPair(Languages language, string translationPart)
        {
            Language = language;
            TranslationPart = translationPart;
        }

        public string GetTranslation(IMobileObject mob)
        {
            if (mob.KnownLanguages.Contains(Language))
            {
                return TranslationPart;
            }
            else
            {
                return GlobalReference.GlobalValues.Translator.Translate(Language, TranslationPart);
            }
        }
    }
}