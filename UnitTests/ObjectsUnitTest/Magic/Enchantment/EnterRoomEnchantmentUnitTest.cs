using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic.Enchantment;
using Moq;
using Objects.Effect.Interface;
using Objects.Global.Random.Interface;
using Objects.Mob.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class EnterRoomEnchantmentUnitTest
    {
        EnterRoomEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new EnterRoomEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
        }

        [TestMethod]
        public void EnterRoomEnchantment_EnterRoom()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.EnterRoom(mob.Object);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
        }
    }
}
