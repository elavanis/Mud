using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Damage
{
    [TestClass]
    public class DamageUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void Damage_Constructor()
        {
            Mock<IDice> dice = new Mock<IDice>();

            Objects.Damage.Damage damage = new Objects.Damage.Damage(dice.Object);

            Assert.AreSame(dice.Object, damage.Dice);
        }
    }
}
