using System;

namespace Shared.PerformanceCounters.Interface
{
    public interface ICounters
    {
        DateTime CounterDateTime { get; set; }
        int ConnnectedPlayers { get; set; }
        decimal CPU { get; set; }
        int MaxTickTimeInMs { get; set; }
        decimal Memory { get; set; }
        int Elementals { get; set; }
    }
}