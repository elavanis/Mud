using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.GameDateTime.Interface;
using Moq;
using Objects.GameDateTime;

namespace ObjectsUnitTest.Global.GameDateTime
{
    [TestClass]
    public class GameDateTimeUnitTest
    {
        Objects.Global.GameDateTime.InGameDateTime gameDateTime;

        [TestInitialize]
        public void Setup()
        {
            Mock<ITime> time = new Mock<ITime>();
            time.Setup(e => e.CurrentDateTime).Returns(new DateTime(2015, 11, 7, 16, 43, 0, DateTimeKind.Utc));
            gameDateTime = new Objects.Global.GameDateTime.InGameDateTime(time.Object);
        }

        [TestMethod]
        public void GameDateTime_InGameDateTime()
        {
            Assert.AreEqual(1, gameDateTime.GameDateTime.Year);
            Assert.AreEqual(1, gameDateTime.GameDateTime.Month);
            Assert.AreEqual(1, gameDateTime.GameDateTime.Day);
            Assert.AreEqual(0, gameDateTime.GameDateTime.Hour);
            Assert.AreEqual(0, gameDateTime.GameDateTime.Minute);
            Assert.AreEqual(Days.Life, gameDateTime.GameDateTime.DayName);
            Assert.AreEqual(Months.Griffin, gameDateTime.GameDateTime.MonthName);
            Assert.AreEqual(Years.Charon, gameDateTime.GameDateTime.YearName);
        }

        [TestMethod]
        public void GameDateTime_InGameFormatedDateTime()
        {
            string formatedDate = "01/01/01 00:00:00\r\nMonth: Griffin\r\nDay: Life\r\nYear: Charon";

            Assert.AreEqual(formatedDate, gameDateTime.GameDateTime.ToString());
        }
    }
}
