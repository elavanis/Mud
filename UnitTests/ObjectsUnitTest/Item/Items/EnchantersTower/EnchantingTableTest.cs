using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Item.Interface;
using Objects.Item.Items.EnchantersTower;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Item.Items.EnchantersTower
{
    [TestClass]
    public class EnchantingTableTest
    {
        EnchantingTable enchantingTable;
        Mock<IItem> item;
        Mock<IRoom> room;
        Mock<IItem> pedistalItem;
        Mock<IContainer> pedistalContainer;

        [TestInitialize]
        public void Setup()
        {
            enchantingTable = new EnchantingTable();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();
            pedistalItem = new Mock<IItem>();
            pedistalContainer = pedistalItem.As<IContainer>();



            //room.Setup(e => e.Items).Returns(new List<IItem>() { pedistalContainer.Object });

        }

        [TestMethod]
        public void EnchantingTable_Enchant_CrystalNotInPlace()
        {
            enchantingTable.Enchant(item.Object);

            Assert.AreEqual(1, 2);
        }
    }
}
