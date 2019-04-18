using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.Settings;

namespace ObjectsUnitTest.Global.DefaultValues
{
    [TestClass]
    public class DefaultValuesUnitTest
    {
        Objects.Global.DefaultValues.DefaultValues defaultValue;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            GlobalReference.GlobalValues.Settings = new Settings();
            GlobalReference.GlobalValues.Settings.MaxLevel = 100;
            defaultValue = new Objects.Global.DefaultValues.DefaultValues();
        }

        [TestMethod]
        public void DefaultValues_DiceForArmorLevel_Level1()
        {
            IDice dice = defaultValue.DiceForArmorLevel(1);

            Assert.AreEqual(1, dice.Die);
            Assert.AreEqual(1, dice.Sides);
        }

        [TestMethod]
        public void DefaultValues_DiceForArmorLevel_Level100()
        {
            IDice dice = defaultValue.DiceForArmorLevel(100);

            Assert.AreEqual(5, dice.Die);
            Assert.AreEqual(5011, dice.Sides);
        }

        [TestMethod]
        public void DefaultValues_DiceForWeaponLevel_Level1()
        {
            IDice dice = defaultValue.DiceForWeaponLevel(1);

            Assert.AreEqual(9, dice.Die);
            Assert.AreEqual(3, dice.Sides);
        }

        [TestMethod]
        public void DefaultValues_DiceForWeaponLevel_Level100()
        {
            IDice dice = defaultValue.DiceForWeaponLevel(100);

            Assert.AreEqual(27, dice.Die);
            Assert.AreEqual(12539, dice.Sides);
        }


        [TestMethod]
        public void DefaultValues_MoneyForNpcLevel_Level0()
        {
            uint money = defaultValue.MoneyForNpcLevel(0);
            Assert.AreEqual(0u, money);
        }

        [TestMethod]
        public void DefaultValues_MoneyForNpcLevel_Level1()
        {
            uint money = defaultValue.MoneyForNpcLevel(1);
            Assert.AreEqual(100u, money);
        }

        [TestMethod]
        public void DefaultValues_MoneyForNpcLevel_Level100()
        {
            uint money = defaultValue.MoneyForNpcLevel(100);
            Assert.AreEqual(1262643u, money);
        }

        [TestMethod]
        public void DefaultValues_DiceForTrapLevel_Level1()
        {
            IDice die = defaultValue.DiceForTrapLevel(1);
            Assert.AreEqual(40, die.Die);
            Assert.AreEqual(5, die.Sides);
        }

        [TestMethod]
        public void DefaultValues_DiceForTrapLevel_Level100()
        {
            IDice die = defaultValue.DiceForTrapLevel(100);
            Assert.AreEqual(2776, die.Die);
            Assert.AreEqual(4001, die.Sides);
        }
    }
}
