using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.World.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class DismountUnitTest
    {
        IMobileObjectCommand command;
        Mock<IWorld> world;
        Mock<IMobileObject> mob;
        Mock<ITagWrapper> tagWrapper;
        Mock<IResult> result;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            world = new Mock<IWorld>();
            mob = new Mock<IMobileObject>();
            tagWrapper = new Mock<ITagWrapper>();
            result = new Mock<IResult>();

            world.Setup(e => e.Dismount(mob.Object)).Returns(result.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Dismount();
        }


        [TestMethod]
        public void Dismount_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Dismount", result.ResultMessage);
        }

        [TestMethod]
        public void Dismount_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Dismount"));
        }


        [TestMethod]
        public void Dismount_PerformCommand()
        {
            IResult resultReturned = command.PerformCommand(mob.Object, null);

            world.Verify(e => e.Dismount(mob.Object), Times.Once);
            Assert.AreEqual(result.Object, resultReturned);
        }
    }
}
