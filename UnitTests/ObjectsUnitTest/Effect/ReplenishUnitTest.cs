using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Global;
using System.Collections.Generic;
using Objects.Room.Interface;
using Objects.Item.Interface;
using Objects.Magic.Interface;
using Objects.Global.Serialization.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class ReplenishUnitTest
    {
        Replenish effect;
        Mock<IEffectParameter> parameter;
        Mock<IItem> item;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            effect = new Replenish();
            parameter = new Mock<IEffectParameter>();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            Mock<ISerialization> serilization = new Mock<ISerialization>();


            parameter.Setup(e => e.ObjectRoom).Returns(room.Object);
            parameter.Setup(e => e.Item).Returns(item.Object);
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            enchantment.Setup(e => e.Parameter).Returns(parameter.Object);
            serilization.Setup(e => e.Serialize(item.Object)).Returns("obj");
            serilization.Setup(e => e.Deserialize<IItem>("obj")).Returns(item.Object);

            GlobalReference.GlobalValues.Serialization = serilization.Object;
        }



        [TestMethod]
        public void Replenish_ProcessEffect()
        {
            effect.ProcessEffect(parameter.Object);

            parameter.VerifySet(e => e.Item = null);
            parameter.VerifySet(e => e.ObjectRoom = null);
            room.Verify(e => e.AddItemToRoom(item.Object, 0));
        }
    }
}
