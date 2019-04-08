using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Item.Items;
using Moq;
using Objects.Magic.Interface;
using System.Collections.Generic;
using Objects.Global.Random.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Global.DefaultValues.Interface;
using Objects.Die;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.Interface;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class EnchanteryUnitTest
    {
        Enchantery enchantery;
        Mock<IWeapon> weapon;
        Mock<IArmor> armor;
        Mock<IRandom> random;
        Mock<IDefaultValues> defaults;
        Mock<ITagWrapper> tagWrapper;
        List<IEnchantment> enchantments;
        [TestInitialize]
        public void Setup()
        {
            enchantery = new Enchantery();
            weapon = new Mock<IWeapon>();
            armor = new Mock<IArmor>();
            random = new Mock<IRandom>();
            defaults = new Mock<IDefaultValues>();
            tagWrapper = new Mock<ITagWrapper>();
            enchantments = new List<IEnchantment>();

            weapon.Setup(e => e.Enchantments).Returns(enchantments);
            armor.Setup(e => e.Enchantments).Returns(enchantments);
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            random.Setup(e => e.PercentDiceRoll(1)).Returns(false);
            defaults.Setup(e => e.DiceForWeaponLevel(0)).Returns(new Dice(1, 2));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));


            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.DefaultValues = defaults.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void Enchantery_Defaults()
        {
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item is gone and only a charred ring remains.", enchantery.EnchantmentFail);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item continues to have a faint glow and hum slightly.", enchantery.EnchantmentSuccess);
            Assert.AreEqual(1000, enchantery.CostToEnchantLevel1Item);
        }

        [TestMethod]
        public void Enchantery_Properties()
        {
            enchantery.EnchantmentFail = "1";
            enchantery.EnchantmentSuccess = "2";
            enchantery.CostToEnchantLevel1Item = 1;

            Assert.AreEqual("1", enchantery.EnchantmentFail);
            Assert.AreEqual("2", enchantery.EnchantmentSuccess);
            Assert.AreEqual(1, enchantery.CostToEnchantLevel1Item);
        }

        [TestMethod]
        public void Enchantery_Enchant_WeaponSuccess()
        {
            IResult result = enchantery.Enchant(weapon.Object);

            Assert.IsTrue(enchantments.Count == 1);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item continues to have a faint glow and hum slightly.", result.ResultMessage);
        }

        [TestMethod]
        public void Enchantery_Enchant_WeaponFail()
        {
            random.Setup(e => e.PercentDiceRoll(100)).Returns(false);

            IResult result = enchantery.Enchant(weapon.Object);

            Assert.IsTrue(enchantments.Count == 0);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item is gone and only a charred ring remains.", result.ResultMessage);
        }

        [TestMethod]
        public void Enchantery_Enchant_ArmorSuccess()
        {
            IResult result = enchantery.Enchant(armor.Object);

            Assert.IsTrue(enchantments.Count == 1);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item continues to have a faint glow and hum slightly.", result.ResultMessage);
        }


        [TestMethod]
        public void Enchantery_Enchant_ArmorSuccess2()
        {
            random.Setup(e => e.Next(2)).Returns(1);

            IResult result = enchantery.Enchant(armor.Object);

            Assert.IsTrue(enchantments.Count == 1);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item continues to have a faint glow and hum slightly.", result.ResultMessage);
        }


        [TestMethod]
        public void Enchantery_Enchant_ArmorFail()
        {
            random.Setup(e => e.PercentDiceRoll(100)).Returns(false);

            IResult result = enchantery.Enchant(armor.Object);

            Assert.IsTrue(enchantments.Count == 0);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item is gone and only a charred ring remains.", result.ResultMessage);
        }
    }
}
