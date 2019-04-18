using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic;
using Moq;
using Objects.Global.Random.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Magic
{
    [TestClass]
    public class BaseEnchantmentUnitTest
    {
        UnitTestBaseEnchantment enchantment;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            enchantment = new UnitTestBaseEnchantment();
            random = new Mock<IRandom>();
            random.Setup(e => e.PercentDiceRoll(0)).Returns(false);
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void BaseEnchantment_ProcessEnchantment_NoTrigger()
        {
            enchantment.ActivationPercent = 0;

            Assert.IsFalse(enchantment.TestProcessEnchantment());
        }

        [TestMethod]
        public void BaseEnchantment_ProcessEnchantment_Trigger()
        {
            enchantment.ActivationPercent = 100;

            Assert.IsTrue(enchantment.TestProcessEnchantment());
        }


        private class UnitTestBaseEnchantment : BaseEnchantment
        {
            public bool TestProcessEnchantment()
            {
                return ProcessEnchantment();
            }
        }
    }
}
