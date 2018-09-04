using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.God;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Concurrent;
using Objects.Mob.Interface;
using Objects.World.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class RetrieveGameStatsUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("RetrieveGameStats", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new RetrieveGameStats();
        }

        [TestMethod]
        public void RetrieveGameStats_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void RetrieveGameStats_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("RetrieveGameStats"));
        }

        [TestMethod]
        public void RetrieveGameStats_PerformCommand_Stats()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("GameStats", TagType.Info)).Returns("GameStats");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>();
            dict.TryAdd("GameStats", "GameStats");
            Mock<IWorld> world = new Mock<IWorld>();
            world.Setup(e => e.WorldResults).Returns(dict);
            GlobalReference.GlobalValues.World = world.Object;
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            IResult result = command.PerformCommand(mob.Object, null);
            Assert.AreEqual("GameStats", result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void RetrieveGameStats_PerformCommand_NoStats()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Unable to retrieve game stats.", TagType.Info)).Returns("Unable to retrieve game stats.");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>();
            Mock<IWorld> world = new Mock<IWorld>();
            world.Setup(e => e.WorldResults).Returns(dict);
            GlobalReference.GlobalValues.World = world.Object;
            Mock<IMobileObject> mob = new Mock<IMobileObject>();

            IResult result = command.PerformCommand(mob.Object, null);
            Assert.AreEqual("Unable to retrieve game stats.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }
    }
}
