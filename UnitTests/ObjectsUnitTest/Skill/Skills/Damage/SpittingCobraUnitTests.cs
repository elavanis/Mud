using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Stats;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Damage;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using static Objects.Damage.Damage;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class SpittingCobraUnitTests
    {
        SpittingCobra spittingCobra;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        List<IEnchantment> enchantments;

        [TestInitialize]
        public void Setup()
        {
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            enchantments = new List<IEnchantment>();

            defaultValue.Setup(e => e.DiceForSkillLevel(30)).Returns(dice.Object);
            defaultValue.Setup(e => e.DiceForSkillLevel(28)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            target.Setup(e => e.Enchantments).Returns(enchantments);
            performer.Setup(e => e.ConstitutionEffective).Returns(9);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            spittingCobra = new SpittingCobra();
        }

        [TestMethod]
        public void SpittingCobra_TeachMessage()
        {
            string expected = "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";
            Assert.AreEqual(expected, spittingCobra.TeachMessage);
        }

        [TestMethod]
        public void SpittingCobra_AdditionalEffect()
        {
            spittingCobra.AdditionalEffect(performer.Object, target.Object);

            Assert.AreEqual(1, enchantments.Count);

            IEnchantment addedEnchantment = enchantments[0];

            Assert.AreEqual(100, addedEnchantment.ActivationPercent);
            Assert.IsTrue(addedEnchantment.Effect is Objects.Effect.Damage);
            Assert.AreEqual(new DateTime(9999, 1, 1), addedEnchantment.EnchantmentEndingDateTime);

            Assert.AreEqual(Stats.Stat.Constitution, addedEnchantment.DefeatInfo.MobStat);
            Assert.AreEqual(9, addedEnchantment.DefeatInfo.CurrentEnchantmentPoints);

            Assert.AreEqual("Poison burns your face.", addedEnchantment.Parameter.TargetMessage.Message);
            Assert.IsNotNull(addedEnchantment.Parameter.Damage);
            Assert.AreSame(dice.Object, addedEnchantment.Parameter.Damage.Dice);
            Assert.AreEqual(DamageType.Poison, addedEnchantment.Parameter.Damage.Type);
        }
    }
}
