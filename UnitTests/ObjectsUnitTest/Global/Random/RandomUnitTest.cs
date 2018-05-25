using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectsUnitTest.Global.Random
{
    [TestClass]
    public class RandomUnitTest
    {
        Objects.Global.Random.Random random;

        [TestInitialize]
        public void Setup()
        {
            random = new Objects.Global.Random.Random();
        }

        [TestMethod]
        public void RandomUnitTest_PercentDiceRoll()
        {
            Assert.IsTrue(random.PercentDiceRoll(100));
            Assert.IsFalse(random.PercentDiceRoll(0));
        }
    }
}
