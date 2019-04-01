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
        public decimal CPU { get; set; }
        [ExcludeFromCodeCoverage]
        public int MaxTickTimeInMs { get; set; }
        [ExcludeFromCodeCoverage]
        public decimal Memory { get; set; }
        [ExcludeFromCodeCoverage]
        public int Elementals { get; set; }

        public override string ToString()
        {
            return $"{CounterDateTime.ToString()}|{ConnnectedPlayers}|{CPU}|{MaxTickTimeInMs}|{Memory}|{Elementals}";
        }
    }
}
