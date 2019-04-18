using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using Objects.Global;

namespace ObjectsUnitTest.Global.TickTimes
{
    [TestClass]
    public class TickTimesUnitTest
    {
        Objects.Global.TickTimes.TickTimes tickTimes;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tickTimes = new Objects.Global.TickTimes.TickTimes();
        }

        [TestMethod]
        public void TickTimes_Enqueue()
        {
            PropertyInfo propertyInfo = tickTimes.GetType().GetProperty("times", BindingFlags.Instance | BindingFlags.NonPublic);
            List<long> times = (List<long>)propertyInfo.GetValue(tickTimes);

            Assert.IsNotNull(times);
            Assert.AreEqual(0, times.Count);

            for (int i = 1; i < 11; i++)
            {
                tickTimes.Enqueue(i);
                int expected = Math.Min(9, i);
                Assert.AreEqual(expected, times.Count);
            }
        }

        [TestMethod]
        public void TickTimes_Times_NotEnoughData()
        {
            Assert.AreEqual("Not enough data.", tickTimes.Times);
        }

        //not a very good test since the stopwatch frequency changes 
        //with computers but it does allow the formating to be tested.
        [TestMethod]
        public void TickTimes_Times()
        {
            for (int i = 1; i < 10; i++)
            {
                tickTimes.Enqueue(i);
            }

            string expected = "Min:   0.00%  Max:   0.00%  Median:   0.00%";
            string result = tickTimes.Times;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TickTimes_MaxTime_NoEntries()
        {
            Assert.AreEqual(0, tickTimes.MaxTime);
        }

        [TestMethod]
        public void TickTimes_MaxTime_9Entries()
        {
            for (int i = 1000000000; i < 1000000010; i++)
            {
                tickTimes.Enqueue(i);
            }

            Assert.IsTrue(tickTimes.MaxTime > 0);
        }

        [TestMethod]
        public void TickTimes_MedianTime_NoEntries()
        {
            Assert.AreEqual(0, tickTimes.MaxTime);
        }

        [TestMethod]
        public void TickTimes_MedianTime_9Entries()
        {
            for (int i = 1000000000; i < 1000000010; i++)
            {
                tickTimes.Enqueue(i);
            }

            Assert.IsTrue(tickTimes.MedianTime > 0);
        }
    }
}
