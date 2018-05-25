using Objects.Mob.Interface;

namespace Objects.Language.Interface
{
    public interface ITranslationMessage
    {
        string GetTranslatedMessage(IMobileObject mob);
    }
}