using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.World.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class DismountUnitTest
    {
        IMobileObjectCommand command;
        Mock<IWorld> world;
        Mock<IMobileObject> mob;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            command = new Dismount();

            world = new Mock<IWorld>();
            mob = new Mock<IMobileObject>();

            GlobalReference.GlobalValues.World = world.Object;
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
        public void Dismount_WriteUnitTests()
        {
            IResult result = command.PerformCommand(mob.Object, null);

            world.Verify(e => e.Dismount(mob.Object), Times.Once);
            Assert.IsNotNull(result);
        }
    }
}
