using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Item.Items;
using static Objects.Item.Item;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class FountainUnitTest
    {
        [TestMethod]
        public void Fountain_Constructor()
        {
            Fountain fountain = new Fountain();

            Assert.IsTrue(fountain.Attributes.Contains(ItemAttribute.NoGet));
            Assert.AreEqual("Fountain", fountain.KeyWords[0]);
        }
    }
}
