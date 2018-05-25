using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Language;
using Objects.Global.Language.Interface;
using Objects.Global.Random.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.Language
{
    [TestClass]
    public class TranslatorUnitTest
    {
        Translator translator;

        [TestInitialize]
        public void Setup()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1, 10)).Returns(5);
            GlobalReference.GlobalValues.Random = random.Object;

            Mock<ITranslatorAlgorithm> transAlg = new Mock<ITranslatorAlgorithm>();
            transAlg.Setup(e => e.CalculateHash("Magic")).Returns("magic");
            transAlg.Setup(e => e.CalculateHash("magictestmessage")).Returns("01");
            transAlg.Setup(e => e.CalculateHash("AncientMagic")).Returns("ancientMagic");
            transAlg.Setup(e => e.CalculateHash("ancientMagictestmessage")).Returns("02");
            transAlg.Setup(e => e.CalculateHash("magiczeromessage")).Returns("00000000000000000000000000000000");

            translator = new Translator(transAlg.Object);
        }

        [TestMethod]
        public void Translator_Translate()
        {
            Assert.AreEqual("u f t unftt bhf", translator.Translate(Translator.Languages.Magic, "test message"));
        }

        [TestMethod]
        public void Translator_Translate2()
        {
            Assert.AreEqual("v gu vo guuci g", translator.Translate(Translator.Languages.AncientMagic, "test message"));
        }

        [TestMethod]
        public void Translator_Translate_HashedAllZero()
        {
            Assert.AreEqual("m r ebzrf fntr", translator.Translate(Translator.Languages.Magic, "zero message"));
        }
    }
}