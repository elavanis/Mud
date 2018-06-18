using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Command.PC;
using Objects.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Random.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Objects.World.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Global.Notify.Interface;
using Objects.Global.Commands.Interface;
using Objects.Command;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class FleeUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> attacker;
        Mock<ICommand> mockCommand;
        Mock<IRandom> random;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<IExit> exit;
        Mock<IZone> zone;
        Mock<IWorld> world;
        Mock<INotify> notify;
        Mock<ICommandList> commandList;
        Mock<IMobileObjectCommand> mobileObjectCommand;
        List<IParameter> parameters;

        Dictionary<string, IMobileObjectCommand> dictionaryCommandList;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            attacker = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            random = new Mock<IRandom>();
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            exit = new Mock<IExit>();
            zone = new Mock<IZone>();
            world = new Mock<IWorld>();
            notify = new Mock<INotify>();
            commandList = new Mock<ICommandList>();
            mobileObjectCommand = new Mock<IMobileObjectCommand>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
            parameters = new List<IParameter>();
            dictionaryCommandList = new Dictionary<string, IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag("Flee {Direction}", TagType.Info)).Returns("message");
            tagWrapper.Setup(e => e.WrapInTag("You run around waving your arms and shouting \"Somebody save me.\" but then realize nothing is fighting you.", TagType.Info)).Returns("wave");
            tagWrapper.Setup(e => e.WrapInTag("looked at new room", TagType.Info)).Returns("looked at new room");
            performer.Setup(e => e.IsInCombat).Returns(true);
            performer.Setup(e => e.Opponent).Returns(attacker.Object);
            performer.Setup(e => e.DexterityEffective).Returns(100);
            performer.Setup(e => e.Room).Returns(room.Object);
            attacker.Setup(e => e.DexterityEffective).Returns(10);
            random.Setup(e => e.Next(10)).Returns(10);
            random.Setup(e => e.Next(100)).Returns(100);
            random.Setup(e => e.Next(1)).Returns(0);
            exit.Setup(e => e.Zone).Returns(1);
            exit.Setup(e => e.Room).Returns(2);
            room.Setup(e => e.East).Returns(exit.Object);
            rooms.Add(2, room2.Object);
            zones.Add(1, zone.Object);
            mockCommand.Setup(e => e.Parameters).Returns(parameters);
            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);
            room.Setup(e => e.Leave(performer.Object, Direction.East)).Returns(true);
            dictionaryCommandList.Add("LOOK", mobileObjectCommand.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(dictionaryCommandList);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;


            mobileObjectCommand.Setup(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>())).Returns(new Result(true, "looked at new room"));
            command = new Flee();
        }

        [TestMethod]
        public void Flee_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Flee_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Flee"));
        }

        [TestMethod]
        public void Flee_PerformCommand_NoIncombat()
        {
            performer.Setup(e => e.IsInCombat).Returns(false);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("wave", result.ResultMessage);
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeSuceess()
        {
            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
        }


    }
}
