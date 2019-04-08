using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Language.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Global.Language.Translator;

namespace ObjectsUnitTest.Language
{
    [TestClass]
    public class TranslationPairUnitTest
    {
        TranslationPair translationPair;
        Mock<IMobileObject> mob;
        Mock<ITranslator> translate;
        [TestInitialize]
        public void Setup()
        {
            translationPair = new TranslationPair(Languages.Magic, "magic");
            mob = new Mock<IMobileObject>();
            translate = new Mock<ITranslator>();

            mob.Setup(e => e.KnownLanguages).Returns(new HashSet<Languages>() { Languages.AncientMagic });
            translate.Setup(e => e.Translate(Languages.Magic, "magic")).Returns("unknown");

            GlobalReference.GlobalValues.Translator = translate.Object;
        }

        [TestMethod]
        public void TranslationPair_Constructor()
        {
            Assert.AreEqual(Languages.Magic, translationPair.Language);
            Assert.AreEqual("magic", translationPair.TranslationPart);
        }

        [TestMethod]
        public void TranslationPair_GetTranslation_UnknownLanguage()
        {
            string result = translationPair.GetTranslation(mob.Object);

            Assert.AreEqual("unknown", result);
            translate.Verify(e => e.Translate(Languages.Magic, "magic"), Times.Once);
        }

        [TestMethod]
        public void TranslationPair_GetTranslation_KnownLanguage()
        {
            mob.Setup(e => e.KnownLanguages).Returns(new HashSet<Languages>() { Languages.Magic });

            string result = translationPair.GetTranslation(mob.Object);

            Assert.AreEqual("magic", result);
        }
    }
}
