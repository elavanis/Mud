using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Shared.TagWrapper.Interface;
using Objects.Effect;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Shared.Sound.Interface;
using Objects.Global.Serialization.Interface;
using System.Collections.Generic;
using Objects.Room.Interface;
using Objects.Item.Interface;
using Objects.Magic.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class ReplenishUnitTest
    {
        Replenish effect;
        Mock<IEffectParameter> parameter;
        Mock<IItem> item;
        List<IItem> items;

        [TestInitialize]
        public void Setup()
        {
            effect = new Replenish();
            parameter = new Mock<IEffectParameter>();
            item = new Mock<IItem>();
            items = new List<IItem>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            Mock<ISerialization> serilization = new Mock<ISerialization>();


            parameter.Setup(e => e.ObjectRoom).Returns(room.Object);
            parameter.Setup(e => e.Item).Returns(item.Object);
            room.Setup(e => e.Items).Returns(items);
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            enchantment.Setup(e => e.Parameter).Returns(parameter.Object);
            serilization.Setup(e => e.Serialize(item.Object)).Returns("obj");
            serilization.Setup(e => e.Deserialize<IItem>("obj")).Returns(item.Object);

            GlobalReference.GlobalValues.Serialization = serilization.Object;
        }



        [TestMethod]
        public void Damage_ProcessEffect()
        {
            effect.ProcessEffect(parameter.Object);

            parameter.VerifySet(e => e.Item = null);
            parameter.VerifySet(e => e.ObjectRoom = null);
            Assert.IsTrue(items.Contains(item.Object));
        }
    }
}
