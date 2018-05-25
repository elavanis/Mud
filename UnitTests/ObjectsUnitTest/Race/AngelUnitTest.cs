using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Mob;
using Objects.Race.Races;

namespace ObjectsUnitTest.Race
{
    [TestClass]
    public class AngelUnitTest
    {
        Angel angel;

        [TestInitialize]
        public void Setup()
        {
            angel = new Angel();
        }

        [TestMethod]
        public void Angel_RaceAttributes()
        {
            Assert.AreEqual(1, angel.RaceAttributes.Count);
            Assert.AreEqual(MobileObject.MobileAttribute.Fly, angel.RaceAttributes[0]);
        }
    }
}
