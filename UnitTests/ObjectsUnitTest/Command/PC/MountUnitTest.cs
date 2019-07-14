using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class MountUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Mount();
        }

        [TestMethod]
        public void Mount_Instruction()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Mount {call}
Mount [pickup] [Mob Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Mount"));
        }

        [TestMethod]
        public void Mount_WriteUnitTest()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
