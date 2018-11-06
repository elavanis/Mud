using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Mob.Interface;
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
        Mock<IPlayerCharacter> pc;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            pc = new Mock<IPlayerCharacter>();

            tagWrapper.Setup(e => e.WrapInTag("Areas", TagType.Info)).Returns("message");
            tagWrapper.Setup(e => e.WrapInTag("Zone -- Level: 5\r\n", TagType.Info)).Returns("Zone -- Level: 5");
            zone.Setup(e => e.Name).Returns("Zone");
            zone.Setup(e => e.Level).Returns(5);
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
            zones.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(zones);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;

            command = new Areas();

        }

        [TestMethod]
        public void Areas_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Areas_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Areas"));
        }

        [TestMethod]
        public void Areas_PerformCommand()
        {
            IResult result = command.PerformCommand(pc.Object, null);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Zone -- Level: 5", result.ResultMessage);

        }

    }
}
