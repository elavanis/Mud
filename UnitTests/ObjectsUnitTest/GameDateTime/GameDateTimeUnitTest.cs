using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.GameDateTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.GameDateTime
{
    [TestClass]
    public class GameDateTimeUnitTest
    {
        [TestMethod]
        public void GameDateTime_Attributes_Blank()
        {
            DateTime dateTime = new DateTime(2015, 11, 7, 0, 24, 0);

            Objects.GameDateTime.GameDateTime gameDateTime = new Objects.GameDateTime.GameDateTime(dateTime);

            Day day = gameDateTime.DayName;
            Month month = gameDateTime.MonthName;
            Year year = gameDateTime.YearName;


            Assert.AreEqual(1, 2);
        }
    }
}
