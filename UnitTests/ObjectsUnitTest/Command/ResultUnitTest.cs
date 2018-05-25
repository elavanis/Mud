using System;
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
        [TestMethod]
        public void Result_Constructor_1Parameter()
        {
            Result result = new Result(true, null);

            Assert.IsTrue(result.ResultSuccess);
            Assert.IsNull(result.ResultMessage);
        }

        [TestMethod]
        public void Result_Constructor_2Parameter()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("test", TagType.Info)).Returns("success");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Result result = new Result(false, "test");

            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("success", result.ResultMessage);
        }

        [TestMethod]
        public void Result_Constructor_3Parameter()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("art", TagType.AsciiArt)).Returns("success");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Result result = new Result(true, "art", TagType.AsciiArt);

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("success", result.ResultMessage);
        }
    }
}