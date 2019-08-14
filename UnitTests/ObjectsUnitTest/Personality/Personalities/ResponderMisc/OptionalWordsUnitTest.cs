using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Objects.Personality.ResponderMisc;

namespace ObjectsUnitTest.Personality.Personalities.ResponderMisc
{
    [TestClass]
    public class OptionalWordsUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void OptionalWords_TriggerWords_Set()
        {
            OptionalWords optionalWords = new OptionalWords();
            Assert.IsNotNull(optionalWords.TriggerWords);
        }
    }
}
