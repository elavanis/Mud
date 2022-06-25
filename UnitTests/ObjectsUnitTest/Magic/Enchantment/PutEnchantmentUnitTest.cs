using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Interface;
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
        Mock<IBaseObject> containerObject;
        Mock<IEffectParameter> effectParameter;
        Mock<IRoom> room;
        Mock<IEffect> effect;
        Mock<IBaseObjectId> objectId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            random = new Mock<IRandom>();
            performer = new Mock<IMobileObject>();
            item = new Mock<IItem>();
            container = new Mock<IContainer>();
            containerObject = container.As<IBaseObject>();
            effectParameter = new Mock<IEffectParameter>();
            room = new Mock<IRoom>();
            effect = new Mock<IEffect>();
            objectId = new Mock<IBaseObjectId>();
            enchantment = new PutEnchantment();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            performer.Setup(e => e.Room).Returns(room.Object);
            containerObject.Setup(e => e.Id).Returns(1);
            containerObject.Setup(e => e.ZoneId).Returns(2);
            objectId.Setup(e => e.Id).Returns(1);
            objectId.Setup(e => e.Zone).Returns(2);
            enchantment.ActivationPercent = 100;
            enchantment.Parameter = effectParameter.Object;
            enchantment.Effect = effect.Object;

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
            enchantment.MatchingContainerId = objectId.Object;

            enchantment.Put(performer.Object, item.Object, container.Object);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Once);
        }

        [TestMethod]
        public void GetEnchantment_Put_ContaineDontrMatches()
        {
            objectId.Setup(e => e.Id).Returns(3);
            enchantment.MatchingContainerId = objectId.Object;

            enchantment.Put(performer.Object, item.Object, container.Object);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Never);
        }
    }
}
