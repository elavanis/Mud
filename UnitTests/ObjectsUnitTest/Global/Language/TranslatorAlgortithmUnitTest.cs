using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.Language
{

    [TestClass]
    public class TranslatorAlgortithmUnitTest
    {
        TranslatorAlgorithm algorithm;

        [TestInitialize]
        public void Setup()
        {
            algorithm = new TranslatorAlgorithm();
        }

        [TestMethod]
        public void TranslatorAlgorithm_CalculateHash_Test()
        {
            Assert.AreEqual("0CBC6611F5540BD0809A388DC95A615B", algorithm.CalculateHash("Test"));
        }

        [TestMethod]
        public void TranslatorAlgorithm_CalculateHash_AnotherTest()
        {
            Assert.AreEqual("70AAD1C10AE4E09994EE37CC52ED4EC8", algorithm.CalculateHash("AnotherTest"));
        }
    }
}
