using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;

namespace ObjectsUnitTest.Damage
{
    [TestClass]
    public class DamageUnitTest
    {
        [TestMethod]
        public void Damage_Constructor()
        {
            Mock<IDice> dice = new Mock<IDice>();

            Objects.Damage.Damage damage = new Objects.Damage.Damage(dice.Object);

            Assert.AreSame(dice.Object, damage.Dice);
        }
    }
}
