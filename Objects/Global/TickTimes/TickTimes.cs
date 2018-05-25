using Objects.Global.TickTimes.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.TickTimes
{
    public class TickTimes : ITickTimes
    {
        private List<long> times { get; } = new List<long>();
        public void Enqueue(long ms)
        {
            lock (times)
            {
                times.Add(ms);
                while (times.Count() > 9)
                {
                    times.RemoveAt(0);
                }
            }
        }

        public string Times
        {
            get
            {
                if (times.Count < 9)
                {
                    return "Not enough data.";
                }

                List<long> copyList = null;
                lock (times)
                {
                    copyList = new List<long>(times);
                }

                copyList.Sort();



                return string.Format("Min: {0}%  Max: {1}%  Median: {2}%", CalculatePercentUsage(copyList[0]).ToString().PadLeft(6, ' ')
                                                                        , CalculatePercentUsage(copyList[8]).ToString().PadLeft(6, ' ')
                                                                        , CalculatePercentUsage(copyList[4]).ToString().PadLeft(6, ' ')
                                                                        );
            }
        }

        public decimal MaxTime
        {
            get
            {
                if (times.Count < 9)
                {
                    return 0;
                }
                else
                {
                    List<long> copyList = null;
                    lock (times)
                    {
                        copyList = new List<long>(times);
                    }

                    copyList.Sort();

                    //return CalculatePercentUsage(copyList[8]);
                    return CalculateMs(copyList[8]);
                }
            }
        }

        public decimal MedianTime
        {
            get
            {
                if (times.Count < 9)
                {
                    return 0;
                }
                else
                {
                    List<long> copyList = null;
                    lock (times)
                    {
                        copyList = new List<long>(times);
                    }

                    copyList.Sort();

                    return CalculatePercentUsage(copyList[4]);
                }
            }
        }

        private static decimal CalculatePercentUsage(long ticks)
        {
            Decimal stopWatchTicksPerHeartBeatTick = Convert.ToDecimal(ticks);
            long totalHeartBeatLength = Stopwatch.Frequency / 2; //divide by 2 because there are 2 ticks at 500ms each second

            decimal result = decimal.Round(stopWatchTicksPerHeartBeatTick / totalHeartBeatLength, 2);
            result *= 100; //convert to percent
            return result;
        }

        private decimal CalculateMs(long ticks)
        {
            Decimal stopWatchTicksPerHeartBeatTick = Convert.ToDecimal(ticks);
            long totalHeartBeatLength = Stopwatch.Frequency / 2; //divide by 2 because there are 2 ticks at 500ms each second

            long ms = Convert.ToInt64((stopWatchTicksPerHeartBeatTick / totalHeartBeatLength) * 500);

            return ms;
        }

    }
}
