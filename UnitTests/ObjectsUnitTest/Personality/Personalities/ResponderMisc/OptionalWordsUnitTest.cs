using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality.Personalities.ResponderMisc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.ResponderMisc
{
    [TestClass]
    public class OptionalWordsUnitTest
    {
        [TestMethod]
        public void OptionalWords_TriggerWords_Set()
        {
            OptionalWords optionalWords = new OptionalWords();
            Assert.IsNotNull(optionalWords.TriggerWords);
        }
    }
}
