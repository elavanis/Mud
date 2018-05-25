using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Shared.TagWrapper.Interface;
using Moq;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Personality.Personalities.Interface;
using Objects.Mob.Interface;
using Objects.Command;
using Objects.Personality.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class JoinUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        Mock<IMobileObject> mob;
        Mock<INonPlayerCharacter> npc1;
        Mock<INonPlayerCharacter> npc2;
        Mock<IGuildMaster> guildMaster1;
        Mock<IRoom> room;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Join", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            npc1 = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            guildMaster1 = new Mock<IGuildMaster>();
            room = new Mock<IRoom>();
            mockCommand = new Mock<ICommand>();

            Mock<IResult> result1 = new Mock<IResult>();
            result1.Setup(e => e.ResultSuccess).Returns(true);
            result1.Setup(e => e.ResultMessage).Returns("1");

            Mock<IResult> result2 = new Mock<IResult>();
            result2.Setup(e => e.ResultSuccess).Returns(true);
            result2.Setup(e => e.ResultMessage).Returns("2");

            guildMaster1.Setup(e => e.Join(npc1.Object, mob.Object)).Returns(result1.Object);
            guildMaster1.Setup(e => e.Join(npc2.Object, mob.Object)).Returns(result2.Object);

            npc1.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guildMaster1.Object });
            npc2.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guildMaster1.Object });

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc1.Object, npc2.Object });

            mob.Setup(e => e.Room).Returns(room.Object);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());


            command = new Join();


        }

        [TestMethod]
        public void Join_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Join_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Join"));
        }

        [TestMethod]
        public void Join_PerformCommand_NoParm()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("1", result.ResultMessage);
        }

        [TestMethod]
        public void Join_PerformCommand_SecondGuildMaster()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterNumber).Returns(1);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("2", result.ResultMessage);
        }

        [TestMethod]
        public void Join_PerformCommand_NoGuildMaster()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            tagWrapper.Setup(e => e.WrapInTag("There is no Guildmaster here to induct you.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
