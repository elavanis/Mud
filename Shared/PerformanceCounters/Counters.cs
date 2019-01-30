using Shared.PerformanceCounters.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Shared.PerformanceCounters
{
    public class Counters : ICounters
    {
        [ExcludeFromCodeCoverage]
        public DateTime CounterDateTime { get; set; } = DateTime.UtcNow;
        [ExcludeFromCodeCoverage]
        public int ConnnectedPlayers { get; set; }
        [ExcludeFromCodeCoverage]
        public int CPU { get; set; }
        [ExcludeFromCodeCoverage]
        public int MaxTickTimeInMs { get; set; }
        [ExcludeFromCodeCoverage]
        public int Memory { get; set; }

        public override string ToString()
        {
            return $"{CounterDateTime.ToString()}|{ConnnectedPlayers}|{CPU}|{MaxTickTimeInMs}|{Memory}";
        }
    }
}
