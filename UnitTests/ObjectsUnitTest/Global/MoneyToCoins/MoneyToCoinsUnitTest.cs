using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectsUnitTest.Global.MoneyToCoins
{
    [TestClass]
    public class MoneyToCoinsUnitTest
    {
        Objects.Global.MoneyToCoins.MoneyToCoins money;

        [TestInitialize]
        public void Setup()
        {
            money = new Objects.Global.MoneyToCoins.MoneyToCoins();
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_1()
        {
            string expected = "1 copper";
            string actual = money.FormatedAsCoins(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_1000()
        {
            string expected = "1 silver";
            string actual = money.FormatedAsCoins(1000);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_1000000()
        {
            string expected = "1 gold";
            string actual = money.FormatedAsCoins(1000000);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_1000000000()
        {
            string expected = "1 platinum";
            string actual = money.FormatedAsCoins(1000000000);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_1001001001()
        {
            string expected = "1 platinum 1 gold 1 silver 1 copper";
            string actual = money.FormatedAsCoins(1001001001);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoneyToCoins_FormatedCoins_0()
        {
            string expected = "0 copper";
            string actual = money.FormatedAsCoins(0);
            Assert.AreEqual(expected, actual);
        }
    }
}
