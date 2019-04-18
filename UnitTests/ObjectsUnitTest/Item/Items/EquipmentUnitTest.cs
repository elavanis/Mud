using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
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
            GlobalReference.GlobalValues = new GlobalValues();

            equipment = new Equipment();
        }

        [TestMethod]
        public void Equipment_ItemPosition()
        {
            Assert.AreEqual(AvalableItemPosition.Held, equipment.ItemPosition);
        }
    }
}
