using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.GameDateTime.Interface;
using Moq;

namespace ObjectsUnitTest.Global.GameDateTime
{
    [TestClass]
    public class GameDateTimeUnitTest
    {
        Objects.Global.GameDateTime.GameDateTime gameDateTime;

        [TestInitialize]
        public void Setup()
        {
            Mock<ITime> time = new Mock<ITime>();
            time.Setup(e => e.CurrentDateTime).Returns(new DateTime(2016, 11, 7));
            gameDateTime = new Objects.Global.GameDateTime.GameDateTime(time.Object);
        }

        [TestMethod]
        public void GameDateTime_InGameDateTime()
        {
            DateTime dt = new DateTime(61, 2, 15);
            Assert.AreEqual(dt, gameDateTime.InGameDateTime);
        }

        [TestMethod]
        public void GameDateTime_InGameFormatedDateTime()
        {
            string formatedDate = "02/15/0061 12:00 AM\r\nMonth: Vampire\r\nDay: Air\r\nYear: Calypso";

            Assert.AreEqual(formatedDate, gameDateTime.InGameFormatedDateTime);
        }
    }
}
