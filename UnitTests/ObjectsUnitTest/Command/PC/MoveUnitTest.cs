using System;
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
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Objects.Global.Commands.Interface;
using Objects.Command;
using static Objects.Global.Direction.Directions;

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
        //Mock<IWorld> world;

        //Mock<IExit> exit;
        //Mock<IResult> failedMockResponse;
        //Mock<IZone> zone;
        //Mock<IRoom> differntRoom;
        //Mock<ICommandList> commandList;



        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("(N)orth\r\n(E)ast\r\n(S)outh\r\n(W)est\r\n(U)p\r\n(D)own", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            canDoSomething = new Mock<ICanMobDoSomething>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            room = new Mock<IRoom>();

            command = new Move();
            mob.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.CanMobDoSomething = canDoSomething.Object;
        }

        [TestMethod]
        public void Move_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
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
            Mock<IResult> failedMockResponse = new Mock<IResult>();

            canDoSomething.Setup(e => e.Move(mob.Object)).Returns(failedMockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_NoExit()
        {
            tagWrapper.Setup(e => e.WrapInTag("There is no obvious way to leave that way.", TagType.Info)).Returns("message");
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns<IExit>(null);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Move_PerformCommand_DoorClosed()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IDoor> door = new Mock<IDoor>();

            tagWrapper.Setup(e => e.WrapInTag("You will need to open the SentenceDescription first.", TagType.Info)).Returns("message");
            mockCommand.Setup(e => e.CommandName).Returns("North");
            door.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            door.Setup(e => e.Opened).Returns(false);
            exit.Setup(e => e.Door).Returns(door.Object);
            room.Setup(e => e.North).Returns(exit.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }


        [TestMethod]
        public void Move_PerformCommand_CanNotEnter()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();

            differntRoom.Setup(e => e.CheckEnter(mob.Object)).Returns(failedMockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns(exit.Object);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);

            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_CanNotLeave()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();

            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.CheckLeave(mob.Object)).Returns(failedMockResponse.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveNorth()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.North).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("North");
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.North)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.North), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveEast()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.East).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("East");
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.East)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.East), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveSouth()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.South).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("South");
            room.Setup(e => e.South).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.South)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.South), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveWest()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.West).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("West");
            room.Setup(e => e.West).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.West)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.West), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveUp()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.Up).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("Up");
            room.Setup(e => e.Up).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Up)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Up), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }

        [TestMethod]
        public void Move_PerformCommand_MoveDown()
        {
            Mock<IExit> exit = new Mock<IExit>();
            Mock<IRoom> differntRoom = new Mock<IRoom>();
            Mock<IResult> failedMockResponse = new Mock<IResult>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dRoom = new Dictionary<int, IRoom>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            Mock<IMobileObjectCommand> look = new Mock<IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Look", TagType.Info)).Returns("message");
            room.Setup(e => e.Down).Returns(exit.Object);
            mockCommand.Setup(e => e.CommandName).Returns("Down");
            room.Setup(e => e.Down).Returns(exit.Object);
            room.Setup(e => e.Leave(mob.Object, Direction.Down)).Returns(true);
            dZone.Add(0, zone.Object);
            world.Setup(e => e.Zones).Returns(dZone);
            dRoom.Add(0, differntRoom.Object);
            zone.Setup(e => e.Rooms).Returns(dRoom);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);
            commands.Add("LOOK", look.Object);
            look.Setup(e => e.PerformCommand(mob.Object, It.IsAny<ICommand>())).Returns(failedMockResponse.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(failedMockResponse.Object, result);
            room.Verify(e => e.Leave(mob.Object, Direction.Down), Times.Once);
            mob.VerifySet(e => e.Room = differntRoom.Object);
            differntRoom.Verify(e => e.Enter(mob.Object), Times.Once);
        }
    }
}
