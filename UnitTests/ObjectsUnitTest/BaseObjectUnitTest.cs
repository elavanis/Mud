using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Magic.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest
{
    [TestClass]
    public class BaseObjectUnitTest
    {
        BaseObject baseObject;
        [TestInitialize]
        public void Setup()
        {
            //because base object is abstract...
            baseObject = new UnitTestBaseObject();
        }

        [TestMethod]
        public void BaseObject_KeyWords_BlankCreated()
        {
            Assert.AreEqual(0, baseObject.KeyWords.Count);
        }

        [TestMethod]
        public void BaseObject_KeyWords_Populated()
        {
            string word = "word";
            baseObject.KeyWords.Add(word);
            Assert.AreEqual(1, baseObject.KeyWords.Count);
            Assert.AreSame(word, baseObject.KeyWords[0]);
        }

        [TestMethod]
        public void BaseObject_FlavorOption_BlankCreated()
        {
            Assert.AreEqual(0, baseObject.FlavorOptions.Count);
        }

        [TestMethod]
        public void BaseObject_FlavorOption_Populated()
        {
            List<string> list = new List<string>();
            baseObject.FlavorOptions.Add("key", list);
            Assert.AreEqual(1, baseObject.FlavorOptions.Count);
            Assert.AreSame(list, baseObject.FlavorOptions["key"]);
        }

        [TestMethod]
        public void BaseObject_ZoneSyncOptions_BlankCreated()
        {
            Assert.AreEqual(0, baseObject.ZoneSyncOptions.Count);
        }

        [TestMethod]
        public void BaseObject_ZoneSyncOptions_Populated()
        {
            List<string> list = new List<string>();
            baseObject.ZoneSyncOptions.Add("key", list);
            Assert.AreEqual(1, baseObject.ZoneSyncOptions.Count);
            Assert.AreSame(list, baseObject.ZoneSyncOptions["key"]);
        }

        [TestMethod]
        public void BaseObject_Enchantments_BlankCreated()
        {
            Assert.AreEqual(0, baseObject.Enchantments.Count);
        }

        [TestMethod]
        public void BaseObject_Enchantments_Populated()
        {
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            baseObject.Enchantments.Add(enchantment.Object);
            Assert.AreEqual(1, baseObject.Enchantments.Count);
            Assert.AreSame(enchantment.Object, baseObject.Enchantments[0]);
        }

        [TestMethod]
        public void BaseObject_FinishLoad_ZoneObjectSyncLoad()
        {
            baseObject.ShortDescription = "short";
            baseObject.LongDescription = "long";
            baseObject.ExamineDescription = "examine";
            baseObject.SentenceDescription = "sentence";

            baseObject.ZoneSyncOptions.Add("short", new List<string>() { "a", "ShortDescription" });
            baseObject.ZoneSyncOptions.Add("long", new List<string>() { "a", "LongDescription" });
            baseObject.ZoneSyncOptions.Add("examine", new List<string>() { "a", "ExamineDescription" });
            baseObject.ZoneSyncOptions.Add("sentence", new List<string>() { "a", "SentenceDescription" });
            baseObject.ZoneSyncOptions.Add("ZoneSyncKeywords", new List<string>() { "a", "KeyWords" });

            baseObject.FinishLoad(1);

            Assert.AreEqual("ShortDescription", baseObject.ShortDescription);
            Assert.AreEqual("LongDescription", baseObject.LongDescription);
            Assert.AreEqual("ExamineDescription", baseObject.ExamineDescription);
            Assert.AreEqual("SentenceDescription", baseObject.SentenceDescription);
            Assert.AreEqual("KeyWords", baseObject.KeyWords[0]);
        }

        [TestMethod]
        public void BaseObject_FinishLoad_FlavorObjectsLoad()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(2)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

            baseObject.ShortDescription = "short";
            baseObject.LongDescription = "long";
            baseObject.ExamineDescription = "examine";
            baseObject.SentenceDescription = "sentence";
            baseObject.KeyWords.Add("keyword");

            baseObject.FlavorOptions.Add("short", new List<string>() { "a", "b" });
            baseObject.FlavorOptions.Add("long", new List<string>() { "a", "b" });
            baseObject.FlavorOptions.Add("examine", new List<string>() { "a", "b" });
            baseObject.FlavorOptions.Add("sentence", new List<string>() { "a", "b" });
            baseObject.FlavorOptions.Add("keyword", new List<string>() { "a", "b" });


            baseObject.FinishLoad(1);

            Assert.AreEqual("b", baseObject.ShortDescription);
            Assert.AreEqual("b", baseObject.LongDescription);
            Assert.AreEqual("b", baseObject.ExamineDescription);
            Assert.AreEqual("b", baseObject.SentenceDescription);
            Assert.AreEqual("b", baseObject.KeyWords[0]);
        }

        private class UnitTestBaseObject : BaseObject
        {

        }
    }
}
