using Objects.Personality.Interface;

namespace Objects.Personality.Interface
{
    public interface IHealer : IPersonality
    {
        int CastPercent { get; set; }
    }
}