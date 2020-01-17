using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic.Enchantment;
using Moq;
using Objects.Effect.Interface;
using Objects.Global.Random.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Item.Items.Interface;
using Objects.Interface;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class GetEnchantmentUnitTest
    {
        GetEnchantment enchantment;
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
            enchantment = new GetEnchantment();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            performer.Setup(e => e.Room).Returns(room.Object);
            containerObject.Setup(e => e.Id).Returns(1);
            containerObject.Setup(e => e.Zone).Returns(2);
            objectId.Setup(e => e.Id).Returns(1);
            objectId.Setup(e => e.Zone).Returns(2);
            enchantment.ActivationPercent = 100;
            enchantment.Parameter = effectParameter.Object;
            enchantment.Effect = effect.Object;

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void GetEnchantment_Get()
        {
            enchantment.Get(performer.Object, item.Object, null);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Once);
            effectParameter.VerifySet(e => e.ObjectRoom = room.Object);
        }

        [TestMethod]
        public void GetEnchantment_Get_ContainerMatches()
        {
            enchantment.MatchingContainerId = objectId.Object;

            enchantment.Get(performer.Object, item.Object, container.Object);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Once);
        }

        [TestMethod]
        public void GetEnchantment_Get_ContaineDontrMatches()
        {
            objectId.Setup(e => e.Id).Returns(3);
            enchantment.MatchingContainerId = objectId.Object;

            enchantment.Get(performer.Object, item.Object, container.Object);

            effect.Verify(e => e.ProcessEffect(effectParameter.Object), Times.Never);
        }
    }
}
