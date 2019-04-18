using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global.Random.Interface;
using Objects.Global;

namespace ObjectsUnitTest.LoadPercentage
{
    /// <summary>
    /// Summary description for LoadPercentageUnitTest
    /// </summary>
    [TestClass]
    public class LoadPercentageUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void LoadPercentage_Load()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            random.Setup(e => e.PercentDiceRoll(0)).Returns(false);
            GlobalReference.GlobalValues.Random = random.Object;

            Objects.LoadPercentage.LoadPercentage lp = new Objects.LoadPercentage.LoadPercentage();
            lp.PercentageLoad = 100;
            Assert.IsTrue(lp.Load);
            lp.PercentageLoad = 0;
            Assert.IsFalse(lp.Load);
        }
    }
}
