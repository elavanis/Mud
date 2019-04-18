using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using Objects.Command;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command
{
    [TestClass]
    public class ResultUnitTest
    {
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.AsciiArt)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void Result_Constructor_1Parameter()
        {
            Result result = new Result("", false);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("", result.ResultMessage);
        }

        [TestMethod]
        public void Result_Constructor_2Parameter()
        {
            Result result = new Result("test", true);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("test", result.ResultMessage);
        }

        [TestMethod]
        public void Result_Constructor_3Parameter()
        {
            Result result = new Result("art", false, TagType.AsciiArt);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("art", result.ResultMessage);
        }
    }
}