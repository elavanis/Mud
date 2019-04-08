using Objects.Global.Direction;
using Objects.Personality.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Personalities.Interface
{
    public interface IGuard : IPersonality
    {
        HashSet<Directions.Direction> GuardDirections { get; set; }

        string BlockLeaveMessage { get; set; }
    }
}
