using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class DamageReceivedAfterDefenseEnchantmentUnitTest
    {
        DamageReceivedAfterDefenseEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;
        Mock<IRandom> random;
        Mock<IMobileObject> attacker;
        Mock<IMobileObject> defender;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            enchantment = new DamageReceivedAfterDefenseEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();
            random = new Mock<IRandom>();
            attacker = new Mock<IMobileObject>();
            defender = new Mock<IMobileObject>();
            room = new Mock<IRoom>();

            attacker.Setup(e => e.Room).Returns(room.Object);

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void DamageReceivedAfterDefenseEnchantment_DamageDealtAfterDefense_TargetIsAttacker()
        {
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            enchantment.DamageReceivedAfterDefense(attacker.Object, defender.Object, 10);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
            parameter.VerifySet(e => e.ObjectRoom = room.Object);
            parameter.VerifySet(e => e.Attacker = attacker.Object);
            parameter.VerifySet(e => e.Defender = defender.Object);
            parameter.VerifySet(e => e.Target = attacker.Object);
        }

        [TestMethod]
        public void DamageReceivedAfterDefenseEnchantment_DamageDealtAfterDefense_TargetIsDefender()
        {
            enchantment.TargetIsDefender = true;

            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);

            enchantment.DamageReceivedAfterDefense(attacker.Object, defender.Object, 10);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
            parameter.VerifySet(e => e.ObjectRoom = room.Object);
            parameter.VerifySet(e => e.Attacker = attacker.Object);
            parameter.VerifySet(e => e.Defender = defender.Object);
            parameter.VerifySet(e => e.Target = defender.Object);
        }
    }
}
