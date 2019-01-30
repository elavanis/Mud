using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.Interface;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Magic.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Item.Items.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Global.Settings.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Random.Interface;
using Objects.Command;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class EnchantUnitTest
    {
        Enchant command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Enchant();
        }

        [TestMethod]
        public void Enchant_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Enchant [Item Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Enchant_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Enchant"));
        }

        [TestMethod]
        public void Enchant_PerformCommand_NoParameter()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to enchant?", result.ResultMessage);
        }

        [TestMethod]
        public void Enchant_PerformCommand_NoEnchantery()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();

            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is nothing to enchant with here.", result.ResultMessage);
        }

        [TestMethod]
        public void Enchant_PerformCommand_NoItem()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<IEnchantery> enchantery = new Mock<IEnchantery>();

            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "Enchantery", 0, true, true, true, true, true)).Returns((IBaseObject)enchantery.Object);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find the item.", result.ResultMessage);
        }

        [TestMethod]
        public void Enchant_PerformCommand_NotEnoughGold()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<IEnchantery> enchantery = new Mock<IEnchantery>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "Enchantery", 0, true, true, true, true, true)).Returns((IBaseObject)enchantery.Object);
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);
            item.Setup(e => e.Level).Returns(10);
            enchantery.Setup(e => e.CostToEnchantLevel1Item).Returns(10);
            settings.Setup(e => e.Multiplier).Returns(2);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10240)).Returns("correct money");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need correct money to bind the enchantment.", result.ResultMessage);
        }

        [TestMethod]
        public void Enchant_PerformCommand_EnchantmentFailed()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<IEnchantery> enchantery = new Mock<IEnchantery>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            List<IItem> itemsHeldByMob = new List<IItem>();

            mob.Setup(e => e.Money).Returns(10240);
            mob.Setup(e => e.Items).Returns(itemsHeldByMob);
            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "Enchantery", 0, true, true, true, true, true)).Returns((IBaseObject)enchantery.Object);
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);
            item.Setup(e => e.Level).Returns(10);
            enchantery.Setup(e => e.CostToEnchantLevel1Item).Returns(10);
            enchantery.Setup(e => e.Enchant(item.Object)).Returns(new Result("failure", true));
            settings.Setup(e => e.Multiplier).Returns(2);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10240)).Returns("correct money");
            itemsHeldByMob.Add(item.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("failure", result.ResultMessage);
            Assert.IsFalse(itemsHeldByMob.Contains(item.Object));
            mob.VerifySet(e => e.Money = 0);
        }

        [TestMethod]
        public void Enchant_PerformCommand_EnchantmentSucceed()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
            Mock<IEnchantery> enchantery = new Mock<IEnchantery>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            List<IItem> itemsHeldByMob = new List<IItem>();

            mob.Setup(e => e.Money).Returns(10240);
            mob.Setup(e => e.Items).Returns(itemsHeldByMob);
            parameter.Setup(e => e.ParameterValue).Returns("item");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "Enchantery", 0, true, true, true, true, true)).Returns((IBaseObject)enchantery.Object);
            findObjects.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);
            item.Setup(e => e.Level).Returns(10);
            enchantery.Setup(e => e.CostToEnchantLevel1Item).Returns(10);
            enchantery.Setup(e => e.Enchant(item.Object)).Returns(new Result("success", false));
            settings.Setup(e => e.Multiplier).Returns(2);
            moneyToCoins.Setup(e => e.FormatedAsCoins(10240)).Returns("correct money");
            itemsHeldByMob.Add(item.Object);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("success", result.ResultMessage);
            Assert.IsTrue(itemsHeldByMob.Contains(item.Object));
            mob.VerifySet(e => e.Money = 0);
        }
    }
}
