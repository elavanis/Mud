using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;

namespace ObjectsUnitTest.Global.UpTime
{
    [TestClass]
    public class UpTimeUnitTest
    {
        Objects.Global.UpTime.UpTime updtime;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            updtime = new Objects.Global.UpTime.UpTime();
        }

        [TestMethod]
        public void UpTimeUnitTest_FormatedUpTime()
        {
            DateTime past = DateTime.Now.AddDays(-1).AddHours(-2).AddMinutes(-3).AddSeconds(-4);

            string result = updtime.FormatedUpTime(past);
            Assert.AreEqual(" 1 days,  2 hours,  3 minutes,  4 seconds", result);
        }
    }
}
