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
    public class CloseUnitTest
    {
        [TestMethod]
        public void Close_WriteSomeUnitTests()
        {
        }
    }
    //public class OpenUnitTest
    //{
    //    IMobileObjectCommand command;
    //    Mock<ITagWrapper> tagWrapper;
    //    Mock<IMobileObject> mob;
    //    Mock<ICommand> mockCommand;

    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        GlobalReference.GlobalValues = new GlobalValues();

    //        tagWrapper = new Mock<ITagWrapper>();
    //        tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
    //        GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

    //        mob = new Mock<IMobileObject>();
    //        mockCommand = new Mock<ICommand>();
    //        command = new Open();

    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
    //    }

    //    [TestMethod]
    //    public void Open_Instructions()
    //    {
    //        IResult result = command.Instructions;

    //        Assert.IsTrue(result.AllowAnotherCommand);
    //        Assert.AreEqual("Open [Item Name]", result.ResultMessage);
    //    }

    //    [TestMethod]
    //    public void Open_CommandTrigger()
    //    {
    //        IEnumerable<string> result = command.CommandTrigger;
    //        Assert.AreEqual(1, result.Count());
    //        Assert.IsTrue(result.Contains("Open"));
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_NoParameter()
    //    {
    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.IsTrue(result.AllowAnotherCommand);
    //        Assert.AreEqual("While you ponder what to open you let you mouth hang open.  Hey you did open something!", result.ResultMessage);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_NothingFound()
    //    {
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();

    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.IsTrue(result.AllowAnotherCommand);
    //        Assert.AreEqual("You were unable to find that what you were looking for.", result.ResultMessage);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_ItemNotOpenable()
    //    {
    //        Mock<IItem> item = new Mock<IItem>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(item.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.IsTrue(result.AllowAnotherCommand);
    //        Assert.AreEqual("You found what you were looking for but could not figure out how to open it.", result.ResultMessage);
    //    }


    //    [TestMethod]
    //    public void Open_PerformCommand_DoorLockedAndNoKey()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>());
    //        door.Setup(e => e.Locked).Returns(true);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.IsTrue(result.AllowAnotherCommand);
    //        Assert.AreEqual("The door is locked and will not open.", result.ResultMessage);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_DoorUnlockAndOpen()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_Container()
    //    {
    //        Mock<IItem> item = new Mock<IItem>();
    //        Mock<IOpenable> openable = item.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IResult> mockResult = new Mock<IResult>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(item.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        openable.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorNorth()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.North);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.North).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorEast()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.East);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.East).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorSouth()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.South);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.South).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorWest()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.West);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.West).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorUp()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.Up);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.Up).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }

    //    [TestMethod]
    //    public void Open_PerformCommand_OpenOtherDoorDown()
    //    {
    //        Mock<IDoor> door = new Mock<IDoor>();
    //        Mock<IDoor> door2 = new Mock<IDoor>();
    //        Mock<IRoom> room2 = new Mock<IRoom>();
    //        Mock<IExit> exit2 = new Mock<IExit>();
    //        Mock<IOpenable> openableDoor = door.As<IOpenable>();
    //        Mock<IFindObjects> findObjects = new Mock<IFindObjects>();
    //        Mock<IParameter> parameter = new Mock<IParameter>();
    //        Mock<IItem> key = new Mock<IItem>();
    //        Mock<IResult> mockResult = new Mock<IResult>();
    //        Mock<IWorld> world = new Mock<IWorld>();
    //        Mock<IZone> zone = new Mock<IZone>();
    //        Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
    //        Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
    //        Mock<IBaseObjectId> linkedRoomId = new Mock<IBaseObjectId>();

    //        findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, false, false, true)).Returns(door.Object);
    //        parameter.Setup(e => e.ParameterValue).Returns("item");
    //        mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
    //        mob.Setup(e => e.Items).Returns(new List<IItem>() { key.Object });
    //        door.Setup(e => e.Locked).Returns(true);
    //        door.Setup(e => e.Linked).Returns(true);
    //        linkedRoomId.Setup(e => e.Zone).Returns(1);
    //        linkedRoomId.Setup(e => e.Id).Returns(2);
    //        door.Setup(e => e.LinkedRoomId).Returns(linkedRoomId.Object);
    //        door.Setup(e => e.LinkedRoomDirection).Returns(Direction.Down);
    //        openableDoor.Setup(e => e.Open(mob.Object)).Returns(mockResult.Object);
    //        world.Setup(e => e.Zones).Returns(zones);
    //        zones.Add(1, zone.Object);
    //        zone.Setup(e => e.Rooms).Returns(rooms);
    //        rooms.Add(2, room2.Object);
    //        exit2.Setup(e => e.Door).Returns(door2.Object);
    //        room2.Setup(e => e.Down).Returns(exit2.Object);

    //        GlobalReference.GlobalValues.FindObjects = findObjects.Object;
    //        GlobalReference.GlobalValues.World = world.Object;

    //        IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
    //        Assert.AreSame(mockResult.Object, result);
    //        door2.VerifySet(e => e.Locked = false);
    //        door2.VerifySet(e => e.Opened = true);
    //    }
    //}
}
