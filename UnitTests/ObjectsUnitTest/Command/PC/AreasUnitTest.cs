using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.TagWrapper;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class AreasUnitTest
    {
        IMobileObjectCommand command;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Abils|Abilities", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Areas();

        }


        [TestMethod]
        public void Areas_Instructions()
        {
            IResult result = command.Instructions;

             Assert.IsFalse(result.AllowAnotherCommand);
            //Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Areas_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Areas"));
        }

    }
}
