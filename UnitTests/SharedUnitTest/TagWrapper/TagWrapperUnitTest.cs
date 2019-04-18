using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Shared.TagWrapper.TagWrapper;

namespace SharedUnitTest.TagWrapper
{
    [TestClass]
    public class TagWrapperUnitTest
    {
        Shared.TagWrapper.TagWrapper wrapper;

        [TestInitialize]
        public void Setup()
        {
            //We actually don't need to reference GlobalValues but I want this here to make sure I didn't miss it
            //GlobalReference.GlobalValues = new GlobalValues();

            wrapper = new Shared.TagWrapper.TagWrapper();
        }

        [TestMethod]
        public void TagWrapper_WrapInTag()
        {
            string expected = "<Info>Test</Info>";
            string result = wrapper.WrapInTag("Test");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TagWrapper_WrapInTag_Blank()
        {
            string expected = null;
            string result = wrapper.WrapInTag("");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TagWrapper_WrapInTag_PlayerCharacter()
        {
            string expected = "<PlayerCharacter>Test</PlayerCharacter>";
            string result = wrapper.WrapInTag("Test", TagType.PlayerCharacter);

            Assert.AreEqual(expected, result);
        }
    }
}
