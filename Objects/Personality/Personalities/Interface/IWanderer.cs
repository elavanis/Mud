﻿using Objects.Interface;
using Objects.Personality.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Personalities.Interface
{
    public interface IWanderer : IPersonality
    {
        int MovementPercent { get; set; }
        List<IBaseObjectId> NavigableRooms { get; }
    }
}