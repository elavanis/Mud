using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Personality.ResponderMisc;
using Objects.Personality.ResponderMisc.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Personality.Personalities.ResponderMisc
{
    [TestClass]
    public class ResponseUnitTest
    {
        Response response;
        Mock<IOptionalWords> optionalWords;
        Mock<IOptionalWords> optionalWords2;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            response = new Response();
            optionalWords = new Mock<IOptionalWords>();
            optionalWords2 = new Mock<IOptionalWords>();

            optionalWords.Setup(e => e.TriggerWords).Returns(new List<string>() { "optionalWords1-1", "optionalWords1-2" });
            optionalWords2.Setup(e => e.TriggerWords).Returns(new List<string>() { "optionalWords2-1", "optionalWords2-2" });
            response.RequiredWordSets.Add(optionalWords.Object);
        }

        [TestMethod]
        public void Response_Match_FirstWord_Matched()
        {
            Assert.IsTrue(response.Match(new List<string>() { "optionalWords1-1" }));
        }

        [TestMethod]
        public void Response_Match_MultiplWords_Matched()
        {
            Assert.IsTrue(response.Match(new List<string>() { "nonMatchWord", "optionalWords1-1" }));
        }

        [TestMethod]
        public void Response_Match_MultiplWords_NoMatch()
        {
            Assert.IsFalse(response.Match(new List<string>() { "nonMatchWord", "nonMatchWord2" }));
        }

        [TestMethod]
        public void Response_Match_MultiRequired_Matched()
        {
            response.RequiredWordSets.Add(optionalWords2.Object);

            Assert.IsTrue(response.Match(new List<string>() { "optionalWords1-1", "optionalWords2-2" }));
        }

        [TestMethod]
        public void Response_Match_MultiRequired_MoMatch()
        {
            response.RequiredWordSets.Add(optionalWords2.Object);

            Assert.IsFalse(response.Match(new List<string>() { "optionalWords1-1", "nonMatchWord2" }));
        }
    }
}
