﻿using System;

namespace Objects.Global.PerformanceCounters.Interface
{
    public interface ICounters
    {
        DateTime CounterDateTime { get; set; }
        int ConnnectedPlayers { get; set; }
        int CPU { get; set; }
        int MaxTickTimeInMs { get; set; }
        int Memory { get; set; }
    }
}