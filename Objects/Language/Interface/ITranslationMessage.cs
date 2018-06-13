using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Language.Interface
{
    public interface ITranslationMessage
    {
        string Message { get; set; }
        List<ITranslationPair> TranslationPair { get; set; }
        string GetTranslatedMessage(IMobileObject mob);
    }
}