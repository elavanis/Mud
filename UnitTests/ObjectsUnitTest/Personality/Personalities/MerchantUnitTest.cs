using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality;
using Moq;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using System.Collections.Generic;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Command.Interface;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class MerchantUnitTest
    {
        Merchant merchant;
        Mock<INonPlayerCharacter> mockMerchant;
        Mock<IMobileObject> mockMobileObject;
        Mock<IItem> item;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mockMerchant = new Mock<INonPlayerCharacter>();
            mockMobileObject = new Mock<IMobileObject>();
            item = new Mock<IItem>();
            moneyToCoins = new Mock<IMoneyToCoins>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockMerchant.Setup(e => e.Level).Returns(5);
            mockMerchant.Setup(e => e.CharismaEffective).Returns(2);
            mockMobileObject.Setup(e => e.CharismaEffective).Returns(1);
            item.Setup(e => e.Level).Returns(5);
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            item.Setup(e => e.Value).Returns(100);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            merchant = new Merchant();
        }

        [TestMethod]
        public void Merchant_Process_ReturnsSent()
        {
            string sent = "sent";

            Assert.AreSame(sent, merchant.Process(null, sent));
        }

        [TestMethod]
        public void Merchant_Sell_ItemLevelHigherThanMerchant()
        {
            mockMerchant.Setup(e => e.Level).Returns(4);

            IResult result = merchant.Sell(mockMerchant.Object, mockMobileObject.Object, item.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("The merchant was unwilling to buy that from you.", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Sell_ItemSold()
        {
            List<IItem> items = new List<IItem>();
            items.Add(item.Object);
            mockMobileObject.Setup(e => e.Money).Returns(0);
            mockMobileObject.Setup(e => e.Items).Returns(items);

            moneyToCoins.Setup(e => e.FormatedAsCoins(5)).Returns("5");

            IResult result = merchant.Sell(mockMerchant.Object, mockMobileObject.Object, item.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You sold the SentenceDescription for 5.", result.ResultMessage);
            mockMobileObject.VerifySet(e => e.Money = 5);
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public void Merchant_Offer_ItemLevelHigherThanMerchant()
        {
            List<IItem> items = new List<IItem>();
            items.Add(item.Object);
            mockMobileObject.Setup(e => e.Money).Returns(0);
            mockMobileObject.Setup(e => e.Items).Returns(items);

            IResult result = merchant.Offer(mockMerchant.Object, mockMobileObject.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("ShortDescription - 5", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_DoesNotHaveThatManyItems()
        {
            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("The merchant does not carry that many items.", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_NotEnoughMoney()
        {
            merchant.Sellables.Add(item.Object);

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");

            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need 2000 to buy the SentenceDescription.", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_ItemBough()
        {
            merchant.Sellables.Add(item.Object);

            List<IItem> items = new List<IItem>();
            mockMobileObject.Setup(e => e.Money).Returns(2000);
            mockMobileObject.Setup(e => e.Items).Returns(items);

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");

            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You bought the SentenceDescription for 2000.", result.ResultMessage);
            mockMobileObject.VerifySet(e => e.Money = 0);
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Merchant_List()
        {
            merchant.Sellables.Add(item.Object);

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");

            IResult result = merchant.List(mockMerchant.Object, mockMobileObject.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Item Name             Price\r\n1    ShortDescription 2000", result.ResultMessage);
        }
    }
}
