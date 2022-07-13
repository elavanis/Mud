using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.PerformanceCounters;
using System;

namespace SharedUnitTest.PerformanceCounters
{
    [TestClass]
    public class PerformanceCountersUnitTest
    {
        Counters counters = null!;
        [TestInitialize]
        public void Setup()
        {
            //We actually don't need to reference GlobalValues but I want this here to make sure I didn't miss it
            //GlobalReference.GlobalValues = new GlobalValues();

            counters = new Counters();
            counters.CounterDateTime = new DateTime(2000, 01, 02, 03, 04, 05);
            counters.ConnnectedPlayers = 1;
            counters.CPU = 2;
            counters.MaxTickTimeInMs = 3;
            counters.Memory = 4;
            counters.Elementals = 5;
        }

        [TestMethod]
        public void PerformanceCounters()
        {
            string result = counters.ToString();
            Assert.AreEqual("1/2/2000 3:04:05 AM|1|2|3|4|5", result);
        }
    }
}
