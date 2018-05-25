using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities.Interface
{
    public interface IHealer : IPersonality
    {
        int CastPercent { get; set; }
    }
}