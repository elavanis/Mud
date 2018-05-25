using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Item.Items;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class MoneyUnitTest
    {
        Money money;

        [TestInitialize]
        public void Setup()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("Zero");
            moneyToCoins.Setup(e => e.FormatedAsCoins(1)).Returns("One");
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            money = new Money();
        }

        [TestMethod]
        public void MoneyUnitTest_Constructor_Blank()
        {
            Assert.AreEqual(0ul, money.Value);
            Assert.IsTrue(money.KeyWords.Contains("money"));
            Assert.IsTrue(money.KeyWords.Contains("coins"));
            Assert.AreEqual("coins", money.SentenceDescription);
            Assert.AreEqual("A purse of coins.", money.ShortDescription);
            Assert.AreEqual("A purse of coins worth Zero.", money.LongDescription);
            Assert.AreEqual("While the purse is not worth anything the items in side are worth keeping.", money.ExamineDescription);
        }

        [TestMethod]
        public void MoneyUnitTest_Constructor_Value()
        {
            money = new Money(1);

            Assert.AreEqual(1ul, money.Value);
            Assert.IsTrue(money.KeyWords.Contains("money"));
            Assert.IsTrue(money.KeyWords.Contains("coins"));
            Assert.AreEqual("coins", money.SentenceDescription);
            Assert.AreEqual("A purse of coins.", money.ShortDescription);
            Assert.AreEqual("A purse of coins worth One.", money.LongDescription);
            Assert.AreEqual("While the purse is not worth anything the items in side are worth keeping.", money.ExamineDescription);
        }
    }
}
