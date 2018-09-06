using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality.Personalities;
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
        [TestInitialize]
        public void Setup()
        {
            merchant = new Merchant();
            mockMerchant = new Mock<INonPlayerCharacter>();
            mockMerchant.Setup(e => e.Level).Returns(5);
            mockMerchant.Setup(e => e.CharismaEffective).Returns(2);

            mockMobileObject = new Mock<IMobileObject>();
            mockMobileObject.Setup(e => e.CharismaEffective).Returns(1);

            item = new Mock<IItem>();
            item.Setup(e => e.Level).Returns(5);
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            item.Setup(e => e.Value).Returns(100);

            moneyToCoins = new Mock<IMoneyToCoins>();
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

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
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("The merchant was unwilling to buy that from you.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mockMerchant.Setup(e => e.Level).Returns(4);

            IResult result = merchant.Sell(mockMerchant.Object, mockMobileObject.Object, item.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Sell_ItemSold()
        {
            List<IItem> items = new List<IItem>();
            items.Add(item.Object);
            mockMobileObject.Setup(e => e.Money).Returns(0);
            mockMobileObject.Setup(e => e.Items).Returns(items);

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You sold the SentenceDescription for 5.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            moneyToCoins.Setup(e => e.FormatedAsCoins(5)).Returns("5");


            IResult result = merchant.Sell(mockMerchant.Object, mockMobileObject.Object, item.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
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

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("ShortDescription - 5", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;


            IResult result = merchant.Offer(mockMerchant.Object, mockMobileObject.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_DoesNotHaveThatManyItems()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("The merchant does not carry that many items.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;


            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_NotEnoughMoney()
        {
            merchant.Sellables.Add(item.Object);

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You need 2000 to buy the SentenceDescription.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");


            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Merchant_Buy_ItemBough()
        {
            merchant.Sellables.Add(item.Object);

            List<IItem> items = new List<IItem>();
            mockMobileObject.Setup(e => e.Money).Returns(2000);
            mockMobileObject.Setup(e => e.Items).Returns(items);

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You bought the SentenceDescription for 2000.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");


            IResult result = merchant.Buy(mockMerchant.Object, mockMobileObject.Object, 1);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mockMobileObject.VerifySet(e => e.Money = 0);
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Merchant_List()
        {
            merchant.Sellables.Add(item.Object);

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Item Name             Price\r\n1    ShortDescription 2000", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            moneyToCoins.Setup(e => e.FormatedAsCoins(2000)).Returns("2000");


            IResult result = merchant.List(mockMerchant.Object, mockMobileObject.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
