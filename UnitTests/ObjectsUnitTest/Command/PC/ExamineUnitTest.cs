using System;
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
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Examine {Object Name}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Examine();
            mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Position).Returns(CharacterPosition.Stand);

            cando = new Mock<ICanMobDoSomething>();
            cando.Setup(e => e.SeeDueToLight(mob.Object)).Returns(true);
            GlobalReference.GlobalValues.CanMobDoSomething = cando.Object;

            item = new Mock<IItem>();
            item.Setup(e => e.ExamineDescription).Returns("desc");

            Mock<IFindObjects> find = new Mock<IFindObjects>();
            find.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 0, true, true, true, true, true)).Returns(item.Object);
            find.Setup(e => e.FindObjectOnPersonOrInRoom(mob.Object, "item", 1, true, true, true, true, true)).Returns<IBaseObject>(null);
            find.Setup(e => e.DetermineFoundObjectTagType(item.Object)).Returns(TagType.Item);
            GlobalReference.GlobalValues.FindObjects = find.Object;
        }

        [TestMethod]
        public void Examine_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
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
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("desc", TagType.Item)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("item");
            parm.Setup(e => e.ParameterNumber).Returns(0);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_DidNotFindObject()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You were unable to find that what you were looking for.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("item");
            parm.Setup(e => e.ParameterNumber).Returns(1);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_ExamineRoom()
        {
            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.ExamineDescription).Returns("ExamineDescription");
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("ExamineDescription", TagType.Room)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;


            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_RoomDark()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You can not see here because it is to dark.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            cando.Setup(e => e.SeeDueToLight(mob.Object)).Returns(false);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Examine_PerformCommand_Asleep()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You need to wake up before trying to examining things.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob.Setup(e => e.Position).Returns(CharacterPosition.Sleep);

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
