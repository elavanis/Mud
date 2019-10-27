using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.Custom.MountainGoblinCamp
{
    [TestClass]
    public class ChiefDaughterPresentUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

        }

        [TestMethod]
        public void ChiefDaughterPresenUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }
    }
}