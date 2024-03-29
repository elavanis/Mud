﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Item.Items;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class CorpseUnitTest
    {
        Corpse corpse;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            corpse = new Corpse("examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
        }

        [TestMethod]
        public void Corpse_Constructor()
        {
            Assert.AreEqual("examineDescription", corpse.ExamineDescription);
            Assert.AreEqual("lookDescription", corpse.LookDescription);
            Assert.AreEqual("sentenceDescription", corpse.SentenceDescription);
            Assert.AreEqual("shortDescription", corpse.ShortDescription);
            Assert.IsTrue(corpse.Opened);
            Assert.AreEqual("", corpse.OpenMessage);
            Assert.AreEqual("", corpse.CloseMessage);
        }

        [TestMethod]
        public void Corpse_Opened()
        {
            Assert.IsTrue(corpse.Opened);
        }
    }
}
