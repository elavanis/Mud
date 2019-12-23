using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Objects.Interface;
using static Objects.Global.Direction.Directions;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class OpenUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IFindObjects> findObjects;
        Mock<IParameter> parameter;
        Mock<IDoor> door;
        Mock<IItem> item;
        Mock<IOpenable> openableItem;
        Mock<IOpenable> openableDoor;
        Mock<IItem> key;
        Mock<IResult> mockResult;
        Mock<IDoor> door2;
        Mock<IRoom> room2;
        Mock<IExit> exit2;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Dictionary<int, IZone> zones;
        Dictionary<int, IRoom> rooms;
        Mock<IBaseObjectId> linkedRoomId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            findObjects = new Mock<IFindObjects>();
            parameter = new Mock<IParameter>();
            door = new Mock<IDoor>();
            item = new Mock<IItem>();
            openableItem = item.As<IOpenable>();
            openableDoor = door.As<IOpenable>();
            key = new Mock<IItem>();
            mockResult = new Mock<IResult>();
            door2 = new Mock<IDoor>();
            room2 = new Mock<IRoom>();
            exit2 = new Mock<IExit>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            zones = new Dictionary<int, IZone>();
            rooms = new Dictionary<int, IRoom>();
            linkedRoomId = new Mock<IBaseObjectId>();
            command = new Open();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(item.Object);
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "door", 0, true, true, false, false, true)).Returns(door.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
            door.Setup(e => e.Locked).Returns(true);
            door.Setup(e => e.Linked).Returns(true);
            door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
            openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
            exit2.Setup(e => e.Door).Returns(door2.Object);
            linkedRoomId.Setup(e => e.Id).Returns(2);
            linkedRoomId.Setup(e => e.Zone).Returns(1);
            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.World = world.Object;
        }

        [TestMethod]
        public void Open_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Open [Item Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Open_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Open"));
        }

        [TestMethod]
        public void Open_PerformCommand_NoParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("While you ponder what to open you let you mouth hang open.  Hey you did open something!", result.ResultMessage);
        }

        [TestMethod]
        public void Open_PerformCommand_NothingFound()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find that what you were looking for.", result.ResultMessage);
        }

        [TestMethod]
        public void Open_PerformCommand_ItemNotOpenable()
        {
            item = new Mock<IItem>();

            parameter.Setup(e => e.ParameterValue).Returns("item");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(item.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You found what you were looking for but could not figure out how to open it.", result.ResultMessage);
        }

        [TestMethod]
        public void Open_PerformCommand_DoorLockedAndNoKey()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            mob.Setup(e => e.Items).Returns(new List<IItem>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("The door is locked and will not open.", result.ResultMessage);
        }

        [TestMethod]
        public void Open_PerformCommand_DoorUnlockAndOpen()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.Linked).Returns(false);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
        }

        [TestMethod]
        public void Open_PerformCommand_Container()
        {
            parameter.Setup(e => e.ParameterValue).Returns("item");
            openableItem.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorNorth()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.North);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.North).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorEast()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.East);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.East).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorSouth()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.South);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.South).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorWest()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.West);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.West).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorUp()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.Up);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.Up).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }

        [TestMethod]
        public void Open_PerformCommand_OpenOtherDoorDown()
        {
            parameter.Setup(e => e.ParameterValue).Returns("door");
            door.Setup(e => e.LinkedRoomDirection).Returns(Direction.Down);
            zones.Add(1, zone.Object);
            rooms.Add(2, room2.Object);
            room2.Setup(e => e.Down).Returns(exit2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            door2.VerifySet(e => e.Locked = false);
            door2.VerifySet(e => e.Opened = true);
        }
    }
}
