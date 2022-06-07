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

            equipment = new Equipment(AvalableItemPosition.Wield, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
        }

        [TestMethod]
        public void Equipment_Constructor()
        {
            Assert.AreEqual(AvalableItemPosition.Wield, equipment.ItemPosition);
            Assert.AreEqual("examineDescription", equipment.ExamineDescription);
            Assert.AreEqual("lookDescription", equipment.LookDescription);
            Assert.AreEqual("sentenceDescription", equipment.SentenceDescription);
            Assert.AreEqual("shortDescription", equipment.ShortDescription);
        }

        [TestMethod]
        public void Equipment_ItemPosition()
        {
            Assert.AreEqual(AvalableItemPosition.Held, equipment.ItemPosition);
        }
    }
}
