using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Race;

namespace ObjectsUnitTest.Race
{
    [TestClass]
    public class BaseRaceUnitTest
    {
        UnitTestRace race;

        [TestInitialize]
        public void Setup()
        {
            race = new UnitTestRace();
        }

        [TestMethod]
        public void BaseRace_RaceAttributes()
        {
            Assert.IsNotNull(race.RaceAttributes);
            Assert.AreEqual(0, race.RaceAttributes.Count);
        }

        [TestMethod]
        public void BaseRace_ToString()
        {
            Assert.AreEqual("UnitTestRace", race.ToString());
        }

        private class UnitTestRace : BaseRace
        {

        }
    }
}
