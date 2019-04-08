using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.PC;
using System.Collections.Generic;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using System.Linq;
using static Objects.Room.Room;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class KillUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        Mock<IMobileObject> mob;
        Mock<INonPlayerCharacter> npc1;
        Mock<INonPlayerCharacter> npc2;
        Mock<IRoom> room;
        Mock<ICommand> mockCommand;
        Mock<ICombat> combatEngine;
        Mock<IFindObjects> findObjects;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            npc1 = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            mockCommand = new Mock<ICommand>();
            combatEngine = new Mock<ICombat>();
            findObjects = new Mock<IFindObjects>();

            Mock<IResult> result1 = new Mock<IResult>();
            result1.Setup(e => e.AllowAnotherCommand).Returns(false);
            result1.Setup(e => e.ResultMessage).Returns("1");

            Mock<IResult> result2 = new Mock<IResult>();
            result2.Setup(e => e.AllowAnotherCommand).Returns(false);
            result2.Setup(e => e.ResultMessage).Returns("2");

            npc1.Setup(e => e.KeyWords).Returns(new List<string>() { "npc1" });
            npc1.Setup(e => e.SentenceDescription).Returns("npc1 sentence");
            npc2.Setup(e => e.KeyWords).Returns(new List<string>() { "npc2" });
            npc2.Setup(e => e.SentenceDescription).Returns("npc2 sentence");

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc1.Object, npc2.Object });
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());

            mob.Setup(e => e.Room).Returns(room.Object);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Combat).Returns(combatEngine.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "npc1", 0, false, false, true, true, true)).Returns(npc1.Object);
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            command = new Kill();
        }

        [TestMethod]
        public void Kill_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("(K)ill {Target}", result.ResultMessage);
        }

        [TestMethod]
        public void Kill_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("K"));
            Assert.IsTrue(result.Contains("Kill"));
        }

        [TestMethod]
        public void Kill_PerformCommand_NoParmeterWithNpc()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You begin to attack npc1 sentence.", result.ResultMessage);
            combatEngine.Verify(e => e.AddCombatPair(mob.Object, npc1.Object), Times.Once);
        }

        [TestMethod]
        public void Kill_PerformCommand_NoParmeterNoNpc()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find anything to kill.", result.ResultMessage);
        }

        [TestMethod]
        public void Kill_PerformCommand_WithParameter()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("npc1");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You begin to attack npc1 sentence.", result.ResultMessage);
            combatEngine.Verify(e => e.AddCombatPair(mob.Object, npc1.Object), Times.Once);
        }

        [TestMethod]
        public void Kill_PerformCommand_WithParmeterNoNpc()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("npc2");
            parameter.Setup(e => e.ParameterNumber).Returns(0);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find anything that matches that description to kill.", result.ResultMessage);
        }

        [TestMethod]
        public void Kill_PerformCommand_PeacefulRoom()
        {
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.Peaceful });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were ready to attack but then you sense of peace rush over you and you decided not to attack.", result.ResultMessage);
        }

        [TestMethod]
        public void Kill_PerformCommand_Asleep()
        {
            mob.Setup(e => e.Position).Returns(Objects.Mob.MobileObject.CharacterPosition.Sleep);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not kill someone while you are asleep.", result.ResultMessage);
        }
    }
}
