using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class MountUnitTest
    {
        IMobileObjectCommand command;
        Mock<ICommand> mockCommand;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMount> mount;
        Mock<IRoom> room1;
        Mock<IRoom> room2;
        Mock<IMobileObject> otherMob;
        Mock<IWorld> world;
        Mock<IParameter> parameter1;
        Mock<IParameter> parameter2;
        Mock<IFindObjects> findObjects;
        List<IMobileObject> riders;
        List<IParameter> parameters;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mockCommand = new Mock<ICommand>();
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            mount = new Mock<IMount>();
            room1 = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            otherMob = new Mock<IMobileObject>();
            world = new Mock<IWorld>();
            parameter1 = new Mock<IParameter>();
            parameter2 = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            riders = new List<IMobileObject>();
            parameters = new List<IParameter>();

            mockCommand.Setup(e => e.Parameters).Returns(parameters);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            performer.Setup(e => e.Mount).Returns(mount.Object);
            performer.Setup(e => e.Room).Returns(room1.Object);
            mount.Setup(e => e.Room).Returns(room1.Object);
            mount.Setup(e => e.SentenceDescription).Returns("mountSentence");
            mount.Setup(e => e.Riders).Returns(riders);
            mount.Setup(e => e.MaxRiders).Returns(1);
            otherMob.Setup(e => e.SentenceDescription).Returns("otherMob");
            parameter1.Setup(e => e.ParameterValue).Returns("pickup");
            parameter2.Setup(e => e.ParameterValue).Returns("otherMob");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(performer.Object, "otherMob", 0, false, false, true, true, false)).Returns(otherMob.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;

            command = new Mount();
        }

        [TestMethod]
        public void Mount_Instruction()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Mount {call}
Mount {mount}
Mount [pickup] [Mob Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Mount"));
        }

        [TestMethod]
        public void Mount_PerformCommand_NoMount()
        {
            performer.Setup(e => e.Mount).Returns<IMount>(null);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not own a mount.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_AlreadyMounted()
        {
            riders.Add(performer.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You are already mounted on your mountSentence.", result.ResultMessage);
            Assert.AreEqual(1, riders.Count);
            Assert.AreSame(performer.Object, riders[0]);
        }

        [TestMethod]
        public void Mount_PerformCommand_MountPresentMount()
        {
            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You mount your mountSentence.", result.ResultMessage);
            Assert.AreEqual(1, riders.Count);
            Assert.AreSame(performer.Object, riders[0]);
        }

        [TestMethod]
        public void Mount_PerformCommand_MountPresentMountKickedOffRider()
        {
            riders.Add(otherMob.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You mount your mountSentence.", result.ResultMessage);
            Assert.AreEqual(1, riders.Count);
            Assert.AreSame(performer.Object, riders[0]);
        }

        [TestMethod]
        public void Mount_PerformCommand_SummonMount()
        {
            mount.Setup(e => e.Room).Returns(room2.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Your mount has been called.", result.ResultMessage);
            world.Verify(e => e.AddMountToWorld(mount.Object), Times.Once);
            Assert.AreEqual(0, riders.Count);
            room2.Verify(e => e.RemoveMobileObjectFromRoom(mount.Object), Times.Once);
            room1.Verify(e => e.AddMobileObjectToRoom(mount.Object), Times.Once);
            mount.VerifySet(e => e.Room = room1.Object, Times.Once);
        }

        [TestMethod]
        public void Mount_PerformCommand_CallParameter()
        {
            performer.Setup(e => e.Mount).Returns<IMount>(null);
            parameter1.Setup(e => e.ParameterValue).Returns("call");
            parameters.Add(parameter1.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not own a mount.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_MountParameter()
        {
            performer.Setup(e => e.Mount).Returns<IMount>(null);
            parameter1.Setup(e => e.ParameterValue).Returns("mount");
            parameters.Add(parameter1.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not own a mount.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_NoMount()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            performer.Setup(e => e.Mount).Returns<IMount>(null);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not own a mount.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_DifferentRoom()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            mount.Setup(e => e.Room).Returns(room2.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Your mount is not in the same room as you.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_NotMounted()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need to mount your mount before picking up additional riders.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_NotMounted2()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            riders.Add(otherMob.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need to mount your mount before picking up additional riders.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_ToManyMounted()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            riders.Add(performer.Object);
            riders.Add(otherMob.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Your mount can not carry additional riders.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_AlreadyRiding()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            riders.Add(performer.Object);
            riders.Add(otherMob.Object);
            mount.Setup(e => e.MaxRiders).Returns(3);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("otherMob is already riding with you.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_AddRider()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            riders.Add(performer.Object);
            mount.Setup(e => e.MaxRiders).Returns(3);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You pickup otherMob.", result.ResultMessage);
            Assert.IsTrue(riders.Contains(otherMob.Object));
        }

        [TestMethod]
        public void Mount_PerformCommand_PickupParameter_CantFindRider()
        {
            parameter2.Setup(e => e.ParameterValue).Returns("notFound");
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            riders.Add(performer.Object);
            mount.Setup(e => e.MaxRiders).Returns(3);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unable to find notFound.", result.ResultMessage);
        }

        [TestMethod]
        public void Mount_PerformCommand_TooManyParameters()
        {
            parameters.Add(parameter1.Object);
            parameters.Add(parameter2.Object);
            parameters.Add(parameter1.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Mount {call}
Mount {mount}
Mount [pickup] [Mob Name]", result.ResultMessage);
        }
    }
}
