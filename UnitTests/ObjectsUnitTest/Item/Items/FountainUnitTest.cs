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
            Fountain fountain = new Fountain("examineDescription", "lookDescription", "sentenceDescription", "shortDescription");

            Assert.AreEqual("examineDescription", fountain.ExamineDescription);
            Assert.AreEqual("lookDescription", fountain.LookDescription);
            Assert.AreEqual("sentenceDescription", fountain.SentenceDescription);
            Assert.AreEqual("shortDescription", fountain.ShortDescription);
            Assert.IsTrue(fountain.Attributes.Contains(ItemAttribute.NoGet));
            Assert.AreEqual("Fountain", fountain.KeyWords[0]);
        }
    }
}
