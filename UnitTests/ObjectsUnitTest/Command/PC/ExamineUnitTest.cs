using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using static Objects.Mob.MobileObject;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Item.Interface;
using Objects.Interface;
using Objects.Room.Interface;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class ExamineUnitTest
    {
        IMobileObjectCommand command;
        Mock<IMobileObject> mob;
        Mock<ICanMobDoSomething> cando;
        Mock<IItem> item;
        Mock<ITagWrapper> tagWrapper;
        Mock<INonPlayerCharacter> npc;
        Mock<IFindObjects> find;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            cando = new Mock<ICanMobDoSomething>();
            item = new Mock<IItem>();
            find = new Mock<IFindObjects>();
            npc = new Mock<INonPlayerCharacter>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Item)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.NonPlayerCharacter)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Room)).Returns((string x, TagType y) => (x));
            mob.Setup(e => e.Position).Returns(CharacterPosition.Stand);
            cando.Setup(e => e.SeeDueToLight(mob.Object)).Returns(true);
            item.Setup(e => e.ExamineDescription).Returns("desc");
            find.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, true, true, true)).Returns(item.Object);
            find.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 1, true, true, true, true, true)).Returns<IBaseObject>(null);
            find.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "npc", 0, true, true, true, true, true)).Returns(npc.Object);
            find.Setup(e => e.DetermineFoundObjectTagType(item.Object)).Returns(TagType.Item);
            find.Setup(e => e.DetermineFoundObjectTagType(npc.Object)).Returns(TagType.NonPlayerCharacter);
            npc.Setup(e => e.ExamineDescription).Returns("Examine Description");
            npc.Setup(e => e.HealthDescription).Returns("Health Description");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.CanMobDoSomething = cando.Object;
            GlobalReference.GlobalValues.FindObjects = find.Object;

            command = new Examine();
        }

        [TestMethod]
        public void Examine_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Examine {Object Name}", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Examine"));
        }

        [TestMethod]
        public void Examine_PerformCommand_FindObject()
        {
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("item");
            parm.Setup(e => e.ParameterNumber).Returns(0);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("desc", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_FindMob()
        {
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("npc");
            parm.Setup(e => e.ParameterNumber).Returns(0);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Examine Description\r\nHealth Description", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_DidNotFindObject()
        {
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("item");
            parm.Setup(e => e.ParameterNumber).Returns(1);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find that what you were looking for.", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_ExamineRoom()
        {
            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.ExamineDescription).Returns("ExamineDescription");
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("ExamineDescription", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_RoomDark()
        {
            cando.Setup(e => e.SeeDueToLight(mob.Object)).Returns(false);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not see here because it is to dark.", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_Asleep()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Sleep);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need to wake up before trying to examining things.", result.ResultMessage);
        }
    }
}
