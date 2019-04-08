using Objects.Interface;
using Objects.Personality.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Personalities.Interface
{
    public interface IPhase : IPersonality
    {
        int PhasePercent { get; set; }
        List<IBaseObjectId> RoomsToPhaseTo { get; set; }
    }
}
