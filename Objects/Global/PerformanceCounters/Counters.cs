//using Objects.Global.PerformanceCounters.Interface;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Objects.Global.PerformanceCounters
//{
//    [ExcludeFromCodeCoverage]
//    public class Counters : ICounters
//    {
//        private bool usePerformanceCounters = false;

//        private PerformanceCounter connectedPlayers { get; set; }
//        private PerformanceCounter cpu { get; set; }
//        private PerformanceCounter maxTickMs { get; set; }
//        private PerformanceCounter memory { get; set; }


//        public Counters()
//        {
//            //if (PerformanceCounterCategory.Exists("Mud"))
//            //{
//            //    PerformanceCounterCategory.Delete("Mud");
//            //}

//            //if (!PerformanceCounterCategory.Exists("Mud"))
//            //{
//            //    CounterCreationDataCollection counters = new CounterCreationDataCollection();
//            //    counters.Add(new CounterCreationData("Connected Players", "Players connected at this moment.", PerformanceCounterType.NumberOfItems32));
//            //    counters.Add(new CounterCreationData("CPU%", "Median of CPU time for last 9 ticks", PerformanceCounterType.NumberOfItems32));
//            //    counters.Add(new CounterCreationData("MaxTickMs", "Max of time in ms for last 9 ticks", PerformanceCounterType.NumberOfItems32));
//            //    counters.Add(new CounterCreationData("Memory", "Memory Consumption in MB", PerformanceCounterType.NumberOfItems32));

//            //    PerformanceCounterCategory.Create("Mud", "Mud Performance", PerformanceCounterCategoryType.SingleInstance, counters);
//            //}

//            //connectedPlayers = new PerformanceCounter("Mud", "Connected Players", false);
//            //connectedPlayers.RawValue = 0;

//            //cpu = new PerformanceCounter("Mud", "CPU%", false);
//            //cpu.RawValue = 0;

//            //maxCpu = new PerformanceCounter("Mud", "MaxCPU", false);
//            //maxCpu.RawValue = 0;

//            //memory = new PerformanceCounter("Mud", "Memory", false);
//            //memory.RawValue = 0;


//            if (PerformanceCounterCategory.Exists("Mud"))
//            {
//                usePerformanceCounters = true;

//                connectedPlayers = new PerformanceCounter("Mud", "Connected Players", false);
//                connectedPlayers.RawValue = 10;

//                cpu = new PerformanceCounter("Mud", "CPU%", false);
//                cpu.RawValue = 20;

//                maxTickMs = new PerformanceCounter("Mud", "MaxTickMs", false);
//                maxTickMs.RawValue = 30;

//                memory = new PerformanceCounter("Mud", "Memory", false);
//                memory.RawValue = 40;
//            }
//        }

//        public int ConnnectedPlayers
//        {
//            set
//            {
//                if (usePerformanceCounters)
//                {
//                    connectedPlayers.RawValue = value;
//                }
//            }
//        }

//        public int CPU
//        {
//            set
//            {
//                if (usePerformanceCounters)
//                {
//                    cpu.RawValue = value;
//                }
//            }
//        }

//        public int MaxTickTimeInMs
//        {
//            set
//            {
//                if (usePerformanceCounters)
//                {
//                    maxTickMs.RawValue = value;
//                }
//            }
//        }

//        public int Memory
//        {
//            set
//            {
//                if (usePerformanceCounters)
//                {
//                    memory.RawValue = value;
//                }
//            }
//        }
//    }
//}
