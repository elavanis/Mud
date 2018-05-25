using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.Interface
{
    public interface IPhase : IPersonality
    {
        int PhasePercent { get; set; }
        List<IBaseObjectId> RoomsToPhaseTo { get; set; }
    }
}
