using Objects.Global.Serialization;
using Objects.Global.Serialization.Interface;
using Shared.PerformanceCounters.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementSite.BL
{
    public static class StatsReader
    {
        private static object padLock = new object();
        private static List<ICounters> stats = null;
        private static DateTime lastUpdate = new DateTime();
        private static ISerialization serialization = new JsonSerialization();

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
            List<string> files = Directory.GetFiles(@"C:\Mud\Stats", "*.*", SearchOption.AllDirectories).ToList();
            files = files.OrderByDescending(e => e).ToList();
            string fileContents = File.ReadAllText(files[0]);
            List<ICounters> counters = serialization.Deserialize<List<ICounters>>(fileContents);
            return counters;
        }
    }
}
