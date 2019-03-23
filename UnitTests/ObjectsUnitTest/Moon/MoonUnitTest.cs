using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Moon
{
    [TestClass]
    public class MoonUnitTest
    {
        Objects.Moon.Moon moon;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;

        [TestInitialize]
        public void Setup()
        {
            moon = new Objects.Moon.Moon();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();

            moon.MagicModifier = 2;
            moon.MoonPhaseCycleDays = 10;
            moon.Name = "moon";
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);

            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
        }

        [TestMethod]
        public void Moon_PropertyVerification()
        {
            Assert.AreEqual(2, moon.MagicModifier);
            Assert.AreEqual(5, moon.MoonPhaseCycleDays);
            Assert.AreEqual("moon", moon.Name);
        }

        [TestMethod]
        public void Moon_Full()
        {
            Assert.AreEqual(2, moon.CurrentMagicModifier);
        }

        [TestMethod]
        public void Moon_New()
        {
            gameDateTime.Setup(e => e.TotalDaysPastBegining).Returns(5);

            Assert.AreEqual(.5M, moon.CurrentMagicModifier);
        }

        [TestMethod]
        public void Moon_MoonModifer1()
        {
            moon.MagicModifier = 1;

            Assert.AreEqual(1M, moon.CurrentMagicModifier);
        }
    }
}
