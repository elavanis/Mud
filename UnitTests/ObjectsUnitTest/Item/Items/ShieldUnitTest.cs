using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Item.Items;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class ShieldUnitTest
    {
        Shield shield;

        [TestInitialize]
        public void Setup()
        {
            shield = new Shield();
        }

        [TestMethod]
        public void Shield_Constructor()
        {
            Assert.AreEqual(Equipment.AvalableItemPosition.Wield, shield.ItemPosition);
        }
    }
}
