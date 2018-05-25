using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic.Enchantment;
using Moq;
using Objects.Effect.Interface;
using Objects.Global.Random.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Global.Direction;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class LeaveRoomEnchantmentUnitTest
    {
        LeaveRoomEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new LeaveRoomEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
            enchantment.Direction = Directions.Direction.East;
        }

        [TestMethod]
        public void LeaveRoomEnchantment_EnterRoom()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.LeaveRoom(mob.Object, Directions.Direction.East);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
        }
    }
}
