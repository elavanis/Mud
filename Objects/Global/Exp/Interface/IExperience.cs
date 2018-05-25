namespace Objects.Global.Exp.Interface
{
    public interface IExperience
    {
        int GetDefaultNpcExpForLevel(int level);
        int GetExpForLevel(int level);
    }
}