using Objects.Mob.Interface;

namespace Objects.Global.LevelDifference.Interface
{
    public interface IEvaluateLevelDifference
    {
        string Evalute(IMobileObject performer, IMobileObject mobToConsiderAttacking);
    }
}