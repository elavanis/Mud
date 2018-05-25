using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Personalities.Interface
{
    public interface IWanderer : IPersonality
    {
        int MovementPercent { get; set; }
        List<IBaseObjectId> NavigableRooms { get; }
    }
}