using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Die.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Stats;
using Objects.Global.StringManuplation.Interface;
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
        Mock<ICommand> command;
        Mock<IParameter> parameterCommandName;
        Mock<IParameter> parameterTarget;
        Mock<IFindObjects> findObjects;
        Mock<IEffect> effect;
        Mock<INotify> notify;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            enchantments = new List<IEnchantment>();
            command = new Mock<ICommand>();
            parameterCommandName = new Mock<IParameter>();
            parameterTarget = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            effect = new Mock<IEffect>();
            notify = new Mock<INotify>();
            stringManipulator = new Mock<IStringManipulator>();

            defaultValue.Setup(e => e.DiceForSkillLevel(30)).Returns(dice.Object);
            defaultValue.Setup(e => e.DiceForSkillLevel(28)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            target.Setup(e => e.Enchantments).Returns(enchantments);
            performer.Setup(e => e.ConstitutionEffective).Returns(9);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterCommandName.Object, parameterTarget.Object });
            parameterCommandName.Setup(e => e.ParameterValue).Returns("commandName");
            parameterTarget.Setup(e => e.ParameterValue).Returns("target");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(performer.Object, "target", 0, true, true, true, true, true)).Returns(target.Object);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            spittingCobra = new SpittingCobra();
            spittingCobra.Effect = effect.Object;
        }

        [TestMethod]
        public void SpittingCobra_TeachMessage()
        {
            string expected = "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";
            Assert.AreEqual(expected, spittingCobra.TeachMessage);
        }

        [TestMethod]
        public void SpittingCobra_ProcessSkill_AdditionalEffect()
        {
            spittingCobra.ProcessSkill(performer.Object, command.Object);

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
