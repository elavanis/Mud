using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
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
        List<IMobileObject> riders;

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
            riders = new List<IMobileObject>();


            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            performer.Setup(e => e.Mount).Returns(mount.Object);
            performer.Setup(e => e.Room).Returns(room1.Object);
            mount.Setup(e => e.Room).Returns(room1.Object);
            mount.Setup(e => e.SentenceDescription).Returns("mountSentence");
            mount.Setup(e => e.Riders).Returns(riders);
            mount.Setup(e => e.MaxRiders).Returns(1);
            
            

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;

            command = new Mount();
        }

        [TestMethod]
        public void Mount_Instruction()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Mount {call}
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
            riders.Add(otherMob.Object);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Your mount has been called.", result.ResultMessage);
            world.Verify(e => e.AddMountToWorld(mount.Object), Times.Once);
            Assert.AreEqual(0, riders.Count);
            room1.Verify(e => e.AddMobileObjectToRoom(mount.Object), Times.Once);
            mount.VerifySet(e => e.Room = room1.Object, Times.Once);
        }













        [TestMethod]
        public void Mount_WriteUnitTest()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
