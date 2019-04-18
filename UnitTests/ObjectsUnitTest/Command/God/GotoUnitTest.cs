using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.God;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Objects.Room.Interface;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global.Commands.Interface;
using Objects.Command;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class GotoUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> pc;
        Mock<IWorld> world;
        Mock<INonPlayerCharacter> npc1;
        Mock<INonPlayerCharacter> npc2;
        Mock<IPlayerCharacter> pc1;
        Mock<IPlayerCharacter> pc2;
        Mock<INotify> notify;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            pc = new Mock<IPlayerCharacter>();
            world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Mock<IRoom> roomStart = new Mock<IRoom>();
            Mock<IRoom> roomEnd = new Mock<IRoom>();
            Dictionary<int, IZone> dictionaryZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dictionaryRoom = new Dictionary<int, IRoom>();
            npc1 = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            pc1 = new Mock<IPlayerCharacter>();
            pc2 = new Mock<IPlayerCharacter>();
            notify = new Mock<INotify>();
            Mock<ICanMobDoSomething> canDoSomething = new Mock<ICanMobDoSomething>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mockCommand = new Mock<IMobileObjectCommand>();
            Dictionary<string, IMobileObjectCommand> dictionaryMobileObjectCommand = new Dictionary<string, IMobileObjectCommand>();

            dictionaryZone.Add(1, zone.Object);
            dictionaryRoom.Add(2, roomStart.Object);
            dictionaryRoom.Add(3, roomEnd.Object);
            world.Setup(e => e.Zones).Returns(dictionaryZone);
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);
            pc.Setup(e => e.Room).Returns(roomStart.Object);
            roomStart.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc1.Object });
            roomEnd.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc2.Object });
            roomStart.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc1.Object });
            roomEnd.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc2.Object });
            pc.Setup(e => e.God).Returns(true);
            pc.Setup(e => e.GotoEnterMessage).Returns("God Enters");
            pc.Setup(e => e.GotoLeaveMessage).Returns("God Leaves");
            canDoSomething.Setup(e => e.SeeObject(It.IsAny<IMobileObject>(), pc.Object)).Returns(true);
            commandList.Setup(e => e.PcCommandsLookup).Returns(dictionaryMobileObjectCommand);
            mockCommand.Setup(e => e.PerformCommand(It.IsAny<IMobileObject>(), It.IsAny<ICommand>())).Returns(new Result("look result", true));
            dictionaryMobileObjectCommand.Add("LOOK", mockCommand.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CanMobDoSomething = canDoSomething.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            command = new Goto();
        }

        [TestMethod]
        public void Goto_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Goto [ZoneId] [RoomId] \r\nGoto [PlayerName]", result.ResultMessage);
        }

        [TestMethod]
        public void Goto_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Goto"));
        }

        [TestMethod]
        public void Goto_PerformCommand_NoParameters()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Where would you like to goto?", result.ResultMessage);
        }

        [TestMethod]
        public void Goto_PerformCommand_TwoParametersNotNumbers()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parmeter1 = new Mock<IParameter>();
            Mock<IParameter> parmeter2 = new Mock<IParameter>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter1.Object, parmeter2.Object });
            parmeter1.Setup(e => e.ParameterValue).Returns("a");
            parmeter2.Setup(e => e.ParameterValue).Returns("a");

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Goto [ZoneId] [RoomId]", result.ResultMessage);
        }

        [TestMethod]
        public void Goto_PerformCommand_TwoParametersRoomNotFound()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parmeter1 = new Mock<IParameter>();
            Mock<IParameter> parmeter2 = new Mock<IParameter>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter1.Object, parmeter2.Object });
            parmeter1.Setup(e => e.ParameterValue).Returns("10");
            parmeter2.Setup(e => e.ParameterValue).Returns("20");

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find zone 10 room 20.", result.ResultMessage);
        }

        [TestMethod]
        public void Goto_PerformCommand_TwoParametersRoomFound()
        {
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parmeter1 = new Mock<IParameter>();
            Mock<IParameter> parmeter2 = new Mock<IParameter>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter1.Object, parmeter2.Object });
            parmeter1.Setup(e => e.ParameterValue).Returns("1");
            parmeter2.Setup(e => e.ParameterValue).Returns("3");

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("look result", result.ResultMessage);
            notify.Verify(e => e.Room(pc.Object, null, pc.Object.Room, It.IsAny<ITranslationMessage>(), null, true, false), Times.Exactly(2));
        }
    }
}
