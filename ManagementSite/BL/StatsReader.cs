using Objects.Global.Serialization;
using Objects.Global.Serialization.Interface;
using Shared.PerformanceCounters;
using Shared.PerformanceCounters.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ManagementSite.BL
{
    public static class StatsReader
    {
        private static object padLock = new object();
        private static List<ICounters> stats = null;
        private static DateTime lastUpdate = new DateTime();
        private static ISerialization serialization = new JsonSerialization();
        private static int maxCounterSize = 100;
        private static PropertyInfo[] propertyInfos = typeof(ICounters).GetProperties();


        public static List<ICounters> Stats
        {
            get
            {
                lock (padLock)
                {
                    if (DateTime.Now.Subtract(lastUpdate).TotalSeconds > 60)
                    {
                        stats = ReloadStats();
                        lastUpdate = DateTime.Now;
                    }
                }

                return stats;
            }
        }

        private static List<ICounters> ReloadStats()
        {
            List<string> files = Directory.GetFiles(@"\\freenas\FreeNas\Mud\Stats\", "*.*", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
            {
                files = Directory.GetFiles(@"C:\Mud\Stats", "*.*", SearchOption.AllDirectories).ToList();
            }

            files = files.OrderByDescending(e => e).ToList();
            string fileContents = File.ReadAllText(files[0]);
            List<ICounters> counters = serialization.Deserialize<List<ICounters>>(fileContents);

            counters = LimitCountersCount(counters);



            return counters;
        }

        private static List<ICounters> LimitCountersCount(List<ICounters> counters)
        {
            if (counters.Count > maxCounterSize)
            {
                List<ICounters> newList = new List<ICounters>();

                int divisor = counters.Count / maxCounterSize + 1;

                List<ICounters> smallList = new List<ICounters>();
                foreach (ICounters counter in counters)
                {
                    if (smallList.Count < divisor)
                    {
                        smallList.Add(counter);
                    }
                    else
                    {
                        newList.Add(MedianCounterList(smallList));
                        smallList.Clear();
                    }
                }

                return newList;
            }
            else
            {
                return counters;
            }
        }

        private static ICounters MedianCounterList(List<ICounters> smallList)
        {
            int medianPos = smallList.Count / 2;
            ICounters medianCounter = new Counters();


            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object value = propertyInfo.GetValue(smallList.OrderBy(e => propertyInfo.GetValue(e, null)).ToList()[medianPos]);
                propertyInfo.SetValue(medianCounter, value);
            }

            return medianCounter;
        }
    }
}
