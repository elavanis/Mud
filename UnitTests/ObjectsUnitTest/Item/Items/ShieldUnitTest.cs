using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
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
            GlobalReference.GlobalValues = new GlobalValues();

            shield = new Shield();
        }

        [TestMethod]
        public void Shield_Constructor()
        {
            Assert.AreEqual(Equipment.AvalableItemPosition.Wield, shield.ItemPosition);
        }
    }
}
