using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Language
{
    [TestClass]
    public class TranslationMessageUnitTest
    {

        TranslationMessage translationMessage;

        Mock<IMobileObject> mob;
        Mock<ITranslationPair> translationPair;
        Mock<ITagWrapper> tagWrapper;
        List<ITranslationPair> list;

        [TestInitialize]
        public void Setup()
        {
            mob = new Mock<IMobileObject>();
            translationPair = new Mock<ITranslationPair>();
            tagWrapper = new Mock<ITagWrapper>();
            list = new List<ITranslationPair>() { translationPair.Object };

            translationPair.Setup(e => e.GetTranslation(mob.Object)).Returns("Worked");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            translationMessage = new TranslationMessage("wrapped{0}", TagType.Info, list);
        }

        [TestMethod]
        public void TranslationMessage_Constructor()
        {
            Assert.AreEqual("wrapped{0}", translationMessage.Message);
            Assert.AreEqual(list, translationMessage.TranslationPair);
        }

        [TestMethod]
        public void TranslationMessage_GetTranslatedMessage_List()
        {
            string result = translationMessage.GetTranslatedMessage(mob.Object);
            Assert.AreEqual("wrappedWorked", result);
            translationPair.Verify(e => e.GetTranslation(mob.Object), Times.Once);
        }

        [TestMethod]
        public void TranslationMessage_GetTranslatedMessage_NoList()
        {
            translationMessage = new TranslationMessage("wrapped{0}", TagType.Info);

            string result = translationMessage.GetTranslatedMessage(mob.Object);
            Assert.AreEqual("wrapped{0}", result);
            translationPair.Verify(e => e.GetTranslation(mob.Object), Times.Never);
        }
    }
}
