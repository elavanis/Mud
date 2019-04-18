using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Settings.Interface;

namespace ObjectsUnitTest.Global.MultiClassBonus
{
    [TestClass]
    public class MultiClassBonusUnitTest
    {
        private Objects.Global.MultiClassBonus.MultiClassBonus multiClassBonus;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.MaxLevel).Returns(100);
            settings.Setup(e => e.Multiplier).Returns(1.1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            multiClassBonus = new Objects.Global.MultiClassBonus.MultiClassBonus();
        }


        [TestMethod]
        public void MultiClassBonus_CalculateBonus_1()
        {
            Assert.AreEqual(8, multiClassBonus.CalculateBonus(1, 100000));
        }

        [TestMethod]
        public void MultiClassBonus_CalculateBonus_100()
        {
            Assert.AreEqual(100000, multiClassBonus.CalculateBonus(100, 100000));
        }
    }
}
