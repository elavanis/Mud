using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Magic.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest
{
    [TestClass]
    public class BaseObjectUnitTest
    {
        BaseObject baseObject;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            //because base object is abstract...
            baseObject = new UnitTestBaseObject("examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void BaseObject_Constructor()
        {
            Assert.AreEqual("examineDescription", baseObject.ExamineDescription);
            Assert.AreEqual("lookDescription", baseObject.LookDescription);
            Assert.AreEqual("sentenceDescription", baseObject.SentenceDescription);
            Assert.AreEqual("shortDescription", baseObject.ShortDescription);
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
            baseObject.LookDescription = "look";
            baseObject.ExamineDescription = "examine";
            baseObject.SentenceDescription = "sentence";

            baseObject.ZoneSyncOptions.Add("short", new List<string>() { "a", "ShortDescription" });
            baseObject.ZoneSyncOptions.Add("look", new List<string>() { "a", "LookDescription" });
            baseObject.ZoneSyncOptions.Add("examine", new List<string>() { "a", "ExamineDescription" });
            baseObject.ZoneSyncOptions.Add("sentence", new List<string>() { "a", "SentenceDescription" });
            baseObject.ZoneSyncOptions.Add("ZoneSyncKeywords", new List<string>() { "a", "KeyWords" });

            baseObject.FinishLoad(1);

            Assert.AreEqual("ShortDescription", baseObject.ShortDescription);
            Assert.AreEqual("LookDescription", baseObject.LookDescription);
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
            baseObject.LookDescription = "long";
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
            Assert.AreEqual("b", baseObject.LookDescription);
            Assert.AreEqual("b", baseObject.ExamineDescription);
            Assert.AreEqual("b", baseObject.SentenceDescription);
            Assert.AreEqual("b", baseObject.KeyWords[0]);
        }

        [TestMethod]
        public void BaseObject_Turn()
        {
            baseObject.SentenceDescription = "sentence";

            IResult result = baseObject.Turn(null, null);

            Assert.AreEqual("You try to turn the sentence but nothing happens.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void BaseObject_Pushn()
        {
            baseObject.SentenceDescription = "sentence";

            IResult result = baseObject.Push(null, null);

            Assert.AreEqual("You try to push the sentence but nothing happens.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        private class UnitTestBaseObject : BaseObject
        {
            public UnitTestBaseObject(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
            {
            }
        }
    }
}

