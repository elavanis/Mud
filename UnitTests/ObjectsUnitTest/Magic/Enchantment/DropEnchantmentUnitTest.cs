using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic.Enchantment;
using Moq;
using Objects.Effect.Interface;
using Objects.Global.Random.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class DropEnchantmentUnitTest
    {
        DropEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new DropEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
        }

        [TestMethod]
        public void DropEnchantment_Drop()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            mob.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.Drop(mob.Object, item.Object);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
            parameter.VerifySet(e => e.ObjectRoom = room.Object);
        }
    }
}
