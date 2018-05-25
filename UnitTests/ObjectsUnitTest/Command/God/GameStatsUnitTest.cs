using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.God;
using Objects.World.Interface;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Concurrent;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class GameStatsUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("GameStats", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new GameStats();
        }

        [TestMethod]
        public void GameStats_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GameStats_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("GameStats"));
        }

        [TestMethod]
        public void GameStats_PerformCommand()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            tagWrapper.Setup(e => e.WrapInTag("Calculating Stats", TagType.Info)).Returns("Calculating Stats");
            Mock<IWorld> world = new Mock<IWorld>();
            GlobalReference.GlobalValues.World = world.Object;
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            world.Setup(e => e.WorldCommands).Returns(queue);

            IResult result = command.PerformCommand(mob.Object, null);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("Calculating Stats", result.ResultMessage);
            Assert.AreEqual(1, queue.Count);
            string message;
            queue.TryDequeue(out message);
            Assert.AreEqual("GameStats", message);
            mob.Verify(e => e.EnqueueCommand("RetrieveGameStats"), Times.Once);

        }
    }
}
