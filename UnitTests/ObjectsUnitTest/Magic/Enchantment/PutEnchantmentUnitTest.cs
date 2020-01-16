using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class PutEnchantmentUnitTest
    {
        PutEnchantment enchantment;
        Mock<IRandom> random;
        Mock<IMobileObject> performer;
        Mock<IItem> item;
        Mock<IContainer> container;
        Mock<IEffectParameter> effectParameter;
        Mock<IRoom> room;
        Mock<IEffect> effect;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            random = new Mock<IRandom>();
            performer = new Mock<IMobileObject>();
            item = new Mock<IItem>();
            container = new Mock<IContainer>();
            effectParameter = new Mock<IEffectParameter>();
            room = new Mock<IRoom>();
            effect = new Mock<IEffect>();
            enchantment = new PutEnchantment();

            enchantment.ActivationPercent = 100;
            enchantment.Parameter = effectParameter.Object;
            enchantment.Effect = effect.Object;
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            performer.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void PutEnchantment_Put()
        {
            enchantment.Put(performer.Object, item.Object, container.Object);

            effectParameter.VerifySet(e => e.ObjectRoom = room.Object);
            effectParameter.VerifySet(e => e.Item = item.Object);
            effectParameter.VerifySet(e => e.Target = performer.Object);
            effectParameter.VerifySet(e => e.Container = container.Object);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Once);
        }

        [TestMethod]
        public void GetEnchantment_Put_ContainerMatches()
        {
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void GetEnchantment_Put_ContaineDontrMatches()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
