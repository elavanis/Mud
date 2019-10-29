using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class OpenEnchantmentUnitTest
    {
        OpenEnchantment enchantment;
        Mock<IEffect> effect;
        Mock<IEffectParameter> parameter;
        Mock<IRandom> random;
        Mock<IMobileObject> mob;
        Mock<IItem> item;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            enchantment = new OpenEnchantment();
            effect = new Mock<IEffect>();
            parameter = new Mock<IEffectParameter>();
            random = new Mock<IRandom>();
            mob = new Mock<IMobileObject>();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect.Object;
            enchantment.Parameter = parameter.Object;
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            mob.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void OpenEnchantmentUnitTest_WriteSome()
        {
            enchantment.Open(mob.Object, item.Object);

            effect.Verify(e => e.ProcessEffect(parameter.Object), Times.Once);
            parameter.VerifySet(e => e.ObjectRoom = room.Object);
            parameter.VerifySet(e => e.Item = item.Object);
            parameter.VerifySet(e => e.Target = mob.Object);
        }
    }
}