using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Item.Items;
using static Objects.Item.Items.Equipment;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class EquipmentUnitTest
    {
        Equipment equipment;

        [TestInitialize]
        public void Setup()
        {
            equipment = new Equipment();
        }

        [TestMethod]
        public void Equipment_ItemPosition()
        {
            Assert.AreEqual(AvalableItemPosition.NotWorn, equipment.ItemPosition);
        }
    }
}
