using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Room.Interface;
using Objects.Item.Items.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Objects.Global.Commands.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class MoveUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<ICanMobDoSomething> canDoSomething;
        Mock<IRoom> room;
        Mock<IMount> mount;
        Mock<IExit> exit;
        Mock<IRoom> differntRoom;
        Mock<IResult> mockResponse;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Dictionary<int, IZone> dZone;
        Dictionary<int, IRoom> dRoom;
        Mock<ICommandList> commandList;
        Dictionary<string, IMobileObjectCommand> commands;
        Mock<IMobileObjectCommand> look;
        Mock<IDoor> door;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            canDoSomething = new Mock<ICanMobDoSomething>();
            room = new Mock<IRoom>();
            mount = new Mock<IMount>();
            exit = new Mock<IExit>();
            differntRoom = new Mock<IRoom>();
            mockResponse = new Mock<IResult>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            dZone = new Dictionary<int, IZone>();
            dRoom = new Dictionary<int, IRoom>();
            commandList = new Mock<ICommandList>();
            commands = new Dictionary<string, IMobileObjectCommand>();
            look = new Mock<IMobileObjectCommand>();
            door = new Mock<IDoor>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            mount.Setup(e => e.Room).Returns(room.Object);
            mount.Setup(e => e.Riders).Returns(new List<IMobileObject>() { mob.Object });
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(mockResponse.Object);
            look.Setup(e => e.PerformCommand(mount.Object, It.IsAny<ICommand>())).Returns(mockResponse.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            mockResponse.Setup(e => e.ResultMessage).Returns("look result");



            dZone.Add(0, zone.Object);
            dRoom.Add(0, differntRoom.Object);
            commands.Add("LOOK", look.Object);


            GlobalReference.GlobalValues.CanMobDoSomething = canDoSomething.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Move();
        }

        [TestMethod]
        public void Move_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("(N)orth\r\n(E)ast\r\n(S)outh\r\n(W)est\r\n(U)p\r\n(D)own", result.ResultMessage);
        }

        [TestMethod]
        public void Move_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(12, result.Count());
            Assert.IsTrue(result.Contains("N"));
            Assert.IsTrue(result.Contains("North"));
            Assert.IsTrue(result.Contains("E"));
            Assert.IsTrue(result.Contains("East"));
            Assert.IsTrue(result.Contains("S"));
            Assert.IsTrue(result.Contains("South"));
            Assert.IsTrue(result.Contains("W"));
            Assert.IsTrue(result.Contains("West"));
            Assert.IsTrue(result.Contains("U"));
            Assert.IsTrue(result.Contains("Up"));
            Assert.IsTrue(result.Contains("D"));
            Assert.IsTrue(result.Contains("Down"));
        }

        [TestMethod]
        public void Move_PerformCommand_CannotMove()
        {
            canDoSomething.Setup(e => e.Move(mob.Object)).Returns(mockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_NoExit()
        {
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns<IExit>(null);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no obvious way to leave that way.", result.ResultMessage);
        }

        [TestMethod]
        public void Move_PerformCommand_DoorClosed()
        {

            mockCommand.Setup(e => e.CommandName).Returns("North");
            door.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            door.Setup(e => e.Opened).Returns(false);
            exit.Setup(e => e.Door).Returns(door.Object);
            room.Setup(e => e.North).Returns(exit.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You will need to open the SentenceDescription first.", result.ResultMessage);
        }

        [TestMethod]
        public void Move_PerformCommand_CanNotEnter()
        {
            differntRoom.Setup(e => e.CheckEnter(mob.Object)).Returns(mockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns(exit.Object);

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_CanNotLeave()
        {
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.CheckLeave(mob.Object)).Returns(mockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveNorth()
        {
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.North, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.North, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveEast()
        {
            mockCommand.Setup(e => e.CommandName).Returns("East");
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.East, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.East, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveSouth()
        {
            mockCommand.Setup(e => e.CommandName).Returns("South");
            room.Setup(e => e.South).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.South, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.South, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveWest()
        {
            mockCommand.Setup(e => e.CommandName).Returns("West");
            room.Setup(e => e.West).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.West, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.West, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveUp()
        {
            mockCommand.Setup(e => e.CommandName).Returns("Up");
            room.Setup(e => e.Up).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Up, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Up, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveDown()
        {
            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MountNotInRoom()
        {
            mob.Setup(e => e.Mount).Returns(mount.Object);
            mount.Setup(e => e.Room).Returns<IRoom>(null);

            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);

            mount.VerifySet(e => e.Room = differntRoom.Object, Times.Never);
            mount.Verify(e => e.EnqueueCommand("Down"), Times.Never);
        }

        [TestMethod]
        public void Move_PerformCommand_MountNotSameRoom()
        {
            mob.Setup(e => e.Mount).Returns(mount.Object);
            mount.Setup(e => e.Room).Returns<IRoom>(null);

            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);

            mount.VerifySet(e => e.Room = differntRoom.Object, Times.Never);
            mount.Verify(e => e.EnqueueCommand("Down"), Times.Never);
        }

        [TestMethod]
        public void Move_PerformCommand_MountNotMounted()
        {
            mob.Setup(e => e.Mount).Returns(mount.Object);
            mount.Setup(e => e.Riders).Returns(new List<IMobileObject>());

            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, false), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);

            mount.VerifySet(e => e.Room = differntRoom.Object, Times.Never);
            mount.Verify(e => e.EnqueueCommand("Down"), Times.Never);
        }

        [TestMethod]
        public void Move_PerformCommand_Mounted()
        {
            mob.Setup(e => e.Mount).Returns(mount.Object);

            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(null, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, false), Times.Never);
            mob.VerifySet(e => e.Room = differntRoom.Object, Times.Never);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Never);

            mount.VerifySet(e => e.Room = differntRoom.Object, Times.Never);
            mount.Verify(e => e.EnqueueCommand("Down"), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MountMoves()
        {
            mob.Setup(e => e.Mount).Returns(mount.Object);

            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mount.Object, Direction.Down, false)).Returns(true);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mount.Object, mockCommand.Object);

            Assert.AreSame(mockResponse.Object, result);
            mount.VerifySet(e => e.Room = differntRoom.Object, Times.Once);
            mob.Verify(e => e.EnqueueMessage("look result"), Times.Once);
            room.Verify(e => e.Leave(mob.Object, Direction.Down, true), Times.Once);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }
    }
}
