using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic.Enchantment;
using Objects.Global.Random.Interface;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Effect.Interface;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class HeartbeatBigTickEnchantmentUnitTest
    {
        HeartbeatBigTickEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new HeartbeatBigTickEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
        }

        [TestMethod]
        public void HeartbeatBigTickEnchantment_HeartbeatBigTick()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.HeartbeatBigTick(mob.Object);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
        }
    }
}
