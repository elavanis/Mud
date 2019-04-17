using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Moon;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Moon
{
    [TestClass]

    public class MoonsUnitTest
    {
        Moons moons;
        [TestInitialize]
        public void Setup()
        {
            moons = new Moons();
        }

        [TestMethod]
        public void Moons_MoonListIsNotNull()
        {
            Assert.IsNotNull(moons.MoonList);
        }
    }
}
