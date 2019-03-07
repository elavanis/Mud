using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using Objects.Room.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global;
using System.Collections.Generic;
using static Objects.Room.Room;
using Objects.Global.Engine.Interface;
using Objects.Command.PC;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class LearnUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        Mock<IMobileObject> mob;
        Mock<INonPlayerCharacter> npc1;
        Mock<IRoom> room;
        Mock<ICommand> mockCommand;
        Mock<IFindObjects> findObjects;
        Mock<IGuildMaster> guildMaster;
        Mock<IResult> teach;
        Mock<IResult> teachable;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            npc1 = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            mockCommand = new Mock<ICommand>();
            findObjects = new Mock<IFindObjects>();
            guildMaster = new Mock<IGuildMaster>();
            teach = new Mock<IResult>();
            teachable = new Mock<IResult>();

            Mock<IResult> result1 = new Mock<IResult>();
            result1.Setup(e => e.AllowAnotherCommand).Returns(false);
            result1.Setup(e => e.ResultMessage).Returns("1");

            Mock<IResult> result2 = new Mock<IResult>();
            result2.Setup(e => e.AllowAnotherCommand).Returns(false);
            result2.Setup(e => e.ResultMessage).Returns("2");

            npc1.Setup(e => e.KeyWords).Returns(new List<string>() { "npc1" });
            npc1.Setup(e => e.SentenceDescription).Returns("npc1 sentence");
            npc1.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guildMaster.Object });

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc1.Object });
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());

            mob.Setup(e => e.Room).Returns(room.Object);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            teach.Setup(e => e.AllowAnotherCommand).Returns(false);
            teach.Setup(e => e.ResultMessage).Returns("teach");

            teachable.Setup(e => e.AllowAnotherCommand).Returns(false);
            teachable.Setup(e => e.ResultMessage).Returns("teachable");

            Mock<IEngine> engine = new Mock<IEngine>();
            GlobalReference.GlobalValues.Engine = engine.Object;

            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "npc1", 0, false, false, true, true, false)).Returns(npc1.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;


            guildMaster.Setup(e => e.Teach(npc1.Object, mob.Object, "learn")).Returns(teach.Object);
            guildMaster.Setup(e => e.Teachable(npc1.Object, mob.Object)).Returns(teachable.Object);

            command = new Learn();
        }

        [TestMethod]
        public void Learn_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Learn [Skill/Spell Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Learn_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Learn"));
        }

        [TestMethod]
        public void Learn_PerformCommand_Teachable()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(teachable.Object, result);
        }

        [TestMethod]
        public void Learn_PerformCommand_Teach()
        {
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("learn");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(teach.Object, result);
        }

        [TestMethod]
        public void Learn_PerformCommand_NoGuildMaster()
        {
            Mock<IPersonality> personality = new Mock<IPersonality>();
            npc1.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no GuildMaster here to teach you.", result.ResultMessage);
        }
    }
}
