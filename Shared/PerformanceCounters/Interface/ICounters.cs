using System;

namespace Shared.PerformanceCounters.Interface
{
    public interface ICounters
    {
        DateTime CounterDateTime { get; set; }
        int ConnnectedPlayers { get; set; }
        int CPU { get; set; }
        int MaxTickTimeInMs { get; set; }
        decimal Memory { get; set; }
    }
}