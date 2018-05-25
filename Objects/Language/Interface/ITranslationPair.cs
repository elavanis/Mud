using Objects.Mob.Interface;

namespace Objects.Language.Interface
{
    public interface ITranslationPair
    {
        string GetTranslation(IMobileObject mob);
    }
}