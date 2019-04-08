using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Direction;
using Objects.Global.Random.Interface;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class LeaveRoomMovePcEnchantmentUnitTest
    {
        LeaveRoomMovePcEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new LeaveRoomMovePcEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
            enchantment.Direction = Directions.Direction.East;
        }

        [TestMethod]
        public void LeaveRoomMovePcEnchantment_LeaveRoom_NPC()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.LeaveRoom(npc.Object, Directions.Direction.East);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Never);
        }

        [TestMethod]
        public void LeaveRoomMovePcEnchantment_LeaveRoom_PC()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            enchantment.LeaveRoom(pc.Object, Directions.Direction.East);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
        }
    }
}
