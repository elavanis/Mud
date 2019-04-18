using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Item.Interface;
using Objects.Mob;

namespace ObjectsUnitTest.Item
{
    [TestClass]
    public class ItemUnitTest
    {

        Objects.Item.Item item;
        Mock<ISerialization> serialization;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            item = new Objects.Item.Item();

            serialization = new Mock<ISerialization>();

            serialization.Setup(e => e.Serialize(item)).Returns("serial");
            serialization.Setup(e => e.Deserialize<IItem>("serial")).Returns(new Objects.Item.Item());

            GlobalReference.GlobalValues.Serialization = serialization.Object;
        }

        [TestMethod]
        public void Item_FinishLoad()
        {
            Mock<IDefaultValues> defaultValue = new Mock<IDefaultValues>();
            defaultValue.Setup(e => e.MoneyForNpcLevel(1)).Returns(5);
            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;

            item.Level = 1;
            item.FinishLoad(-1);

            Assert.AreEqual(5ul, item.Value);
        }

        [TestMethod]
        public void Item_Attributes_Blank()
        {
            Assert.IsNotNull(item.Attributes);
            Assert.AreEqual(0, item.Attributes.Count);
        }

        [TestMethod]
        public void Item_Attributes_Populated()
        {
            Objects.Item.Item.ItemAttribute attribute = Objects.Item.Item.ItemAttribute.Invisible;
            item.Attributes.Add(attribute);

            Assert.AreEqual(1, item.Attributes.Count);
            Assert.AreEqual(attribute, item.Attributes[0]);
        }

        [TestMethod]
        public void Item_MobileObjectAttributes_Blank()
        {
            Assert.IsNotNull(item.AttributesForMobileObjectsWhenEquiped);
            Assert.AreEqual(0, item.AttributesForMobileObjectsWhenEquiped.Count);
        }

        [TestMethod]
        public void Item_MobileObjectAttributes_Populated()
        {
            MobileObject.MobileAttribute attribute = MobileObject.MobileAttribute.Fly;
            item.AttributesForMobileObjectsWhenEquiped.Add(attribute);

            Assert.AreEqual(1, item.AttributesForMobileObjectsWhenEquiped.Count);
            Assert.AreEqual(attribute, item.AttributesForMobileObjectsWhenEquiped[0]);
        }

        [TestMethod]
        public void Item_Clone()
        {
            item.Clone();

            serialization.Verify(e => e.Serialize(item), Times.Once);
            serialization.Verify(e => e.Deserialize<IItem>("serial"), Times.Once);
        }
    }
}
