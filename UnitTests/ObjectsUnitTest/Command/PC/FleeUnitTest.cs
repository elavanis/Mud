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
using Objects.Global.Random.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Objects.World.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Global.Notify.Interface;
using Objects.Global.Commands.Interface;
using Objects.Command;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class FleeUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IPlayerCharacter> performer;
        Mock<IMobileObject> attacker;
        Mock<ICommand> mockCommand;
        Mock<IRandom> random;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<IRoom> room3;
        Mock<IExit> exit;
        Mock<IExit> exit2;
        Mock<IZone> zone;
        Mock<IWorld> world;
        Mock<INotify> notify;
        Mock<ICommandList> commandList;
        Mock<IMobileObjectCommand> mobileObjectCommand;
        Mock<IParameter> parameter;
        List<IParameter> parameters;
        ITranslationMessage message = null;

        Dictionary<string, IMobileObjectCommand> dictionaryCommandList;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IPlayerCharacter>();
            attacker = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            random = new Mock<IRandom>();
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            room3 = new Mock<IRoom>();
            exit = new Mock<IExit>();
            exit2 = new Mock<IExit>();
            zone = new Mock<IZone>();
            world = new Mock<IWorld>();
            notify = new Mock<INotify>();
            commandList = new Mock<ICommandList>();
            mobileObjectCommand = new Mock<IMobileObjectCommand>();
            parameter = new Mock<IParameter>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
            parameters = new List<IParameter>();
            dictionaryCommandList = new Dictionary<string, IMobileObjectCommand>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
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
            exit2.Setup(e => e.Zone).Returns(1);
            exit2.Setup(e => e.Room).Returns(3);
            room.Setup(e => e.Leave(performer.Object, Direction.North)).Returns(true);
            room.Setup(e => e.Leave(performer.Object, Direction.East)).Returns(true);
            room.Setup(e => e.Leave(performer.Object, Direction.South)).Returns(true);
            room.Setup(e => e.Leave(performer.Object, Direction.West)).Returns(true);
            room.Setup(e => e.Leave(performer.Object, Direction.Up)).Returns(true);
            room.Setup(e => e.Leave(performer.Object, Direction.Down)).Returns(true);
            rooms.Add(2, room2.Object);
            rooms.Add(3, room3.Object);
            zones.Add(1, zone.Object);
            mockCommand.Setup(e => e.Parameters).Returns(parameters);
            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);
            dictionaryCommandList.Add("LOOK", mobileObjectCommand.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(dictionaryCommandList);
            notify.Setup(e => e.Mob(performer.Object, It.IsAny<ITranslationMessage>())).Callback<IMobileObject, ITranslationMessage>((mob, translationMessage) => { message = translationMessage; });

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;


            mobileObjectCommand.Setup(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>())).Returns(new Result("looked at new room", false));
            command = new Flee();
        }

        [TestMethod]
        public void Flee_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Flee {Direction}", result.ResultMessage);
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

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You run around waving your arms and shouting \"Somebody save me.\" but then realize nothing is fighting you.", result.ResultMessage);
        }

        #region Flee Each Direction
        [TestMethod]
        public void Flee_PerformCommand_FleeNorth()
        {
            room.Setup(e => e.North).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee North.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeEast()
        {
            room.Setup(e => e.East).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee East.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeSouth()
        {
            room.Setup(e => e.South).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee South.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeWest()
        {
            room.Setup(e => e.West).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee West.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeUp()
        {
            room.Setup(e => e.Up).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee Up.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_FleeDown()
        {
            room.Setup(e => e.Down).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee Down.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }
        #endregion Flee Each Direction

        #region Flee Specify Each Direction
        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeNorth()
        {
            parameter.Setup(e => e.ParameterValue).Returns("North");
            parameters.Add(parameter.Object);
            room.Setup(e => e.North).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee North.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeEast()
        {
            parameter.Setup(e => e.ParameterValue).Returns("East");
            parameters.Add(parameter.Object);
            room.Setup(e => e.East).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee East.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeSouth()
        {
            parameter.Setup(e => e.ParameterValue).Returns("South");
            parameters.Add(parameter.Object);
            room.Setup(e => e.South).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee South.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeWest()
        {
            parameter.Setup(e => e.ParameterValue).Returns("West");
            parameters.Add(parameter.Object);
            room.Setup(e => e.West).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee West.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeUp()
        {
            parameter.Setup(e => e.ParameterValue).Returns("Up");
            parameters.Add(parameter.Object);
            room.Setup(e => e.Up).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee Up.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeDown()
        {
            parameter.Setup(e => e.ParameterValue).Returns("Down");
            parameters.Add(parameter.Object);
            room.Setup(e => e.Down).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee Down.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeWrongWay()
        {
            parameter.Setup(e => e.ParameterValue).Returns("Up");
            parameters.Add(parameter.Object);
            room.Setup(e => e.Down).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You tried to flee Up but were unable to instead fled Down.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }

        [TestMethod]
        public void Flee_PerformCommand_SpecifyFleeNotValidDirection()
        {
            parameter.Setup(e => e.ParameterValue).Returns("ABC");
            parameters.Add(parameter.Object);
            room.Setup(e => e.Down).Returns(exit.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            mobileObjectCommand.Verify(e => e.PerformCommand(performer.Object, It.IsAny<ICommand>()), Times.Once);
            Assert.AreEqual("You flee Down.", message.Message);
            performer.Verify(e => e.AddTitle("{performer} bravely ran away"));
        }
        #endregion Flee Specify Each Direction

        [TestMethod]
        public void Flee_PerformCommand_CantRunAway()
        {
            performer.Setup(e => e.DexterityEffective).Returns(10);
            attacker.Setup(e => e.DexterityEffective).Returns(100);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You attempt to run away but are not able to.", result.ResultMessage);
        }
    }
}
