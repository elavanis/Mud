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
    public class WhoUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> pc;
        Mock<IPlayerCharacter> god;
        Mock<IWorld> world;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            pc = new Mock<IPlayerCharacter>();
            god = new Mock<IPlayerCharacter>();
            world = new Mock<IWorld>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            pc.Setup(e => e.God).Returns(false);
            pc.Setup(e => e.Name).Returns("player character");
            pc.Setup(e => e.Level).Returns(10);
            pc.Setup(e => e.Title).Returns("title");
            god.Setup(e => e.God).Returns(true);
            god.Setup(e => e.Name).Returns("god");
            god.Setup(e => e.Level).Returns(1);
            god.Setup(e => e.Title).Returns<string>(null);
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>() { pc.Object, god.Object });

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;

            command = new Who();
        }

        [TestMethod]
        public void Who_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Who", result.ResultMessage);
        }

        [TestMethod]
        public void Who_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Who"));
        }

        [TestMethod]
        public void Who_MessageDisplayed()
        {
            string expected = @"Gods
Level  Name              Title
1      god               

Players
Level  Name              Title
10     player character  title";

            IResult result = command.PerformCommand(null, null);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(expected, result.ResultMessage);
        }
    }
}
