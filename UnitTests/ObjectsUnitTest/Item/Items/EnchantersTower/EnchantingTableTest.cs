using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Interface;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.EnchantersTower;
using Objects.Item.Items.Interface;
using Objects.Magic.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

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
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<IFindObjects> findObjects;
        Dictionary<int, IZone> zones;
        Dictionary<int, IRoom> rooms;
        Mock<IItem> gem;
        Mock<IRandom> random;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            enchantingTable = new EnchantingTable();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();
            pedistalItem = new Mock<IItem>();
            pedistalContainer = pedistalItem.As<IContainer>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            zones = new Dictionary<int, IZone>();
            rooms = new Dictionary<int, IRoom>();
            findObjects = new Mock<IFindObjects>();
            gem = new Mock<IItem>();
            random = new Mock<IRandom>();
            tagWrapper = new Mock<ITagWrapper>();


            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);
            zones.Add(23, zone.Object);
            rooms.Add(8, room.Object);
            findObjects.Setup(e => e.FindItemsInRoom(room.Object, "pedestal")).Returns(new List<IItem> { (IItem)pedistalContainer.Object });
            pedistalContainer.Setup(e => e.Items).Returns(new List<IItem>() { gem.Object });
            gem.Setup(e => e.Zone).Returns(16);
            gem.Setup(e => e.Id).Returns(1);
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void EnchantingTable_Enchant_CrystalNotInPlace()
        {
            pedistalContainer.Setup(e => e.Items).Returns(new List<IItem>());

            IResult result = enchantingTable.Enchant(item.Object);

            Assert.AreEqual("The pedestal doesn't seem to be getting enough energy.  Maybe there needs to be something to focus the energy coming down from the top of the tower.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void EnchantingTable_Enchant_CrystalInPlace()
        {
            IResult result = enchantingTable.Enchant(item.Object);

            Assert.AreEqual("The item begins to glow and then flashes a bright light.  The item is gone and only a charred ring remains.", result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
        }
    }
}
