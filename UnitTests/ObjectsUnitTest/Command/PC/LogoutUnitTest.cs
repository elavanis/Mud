using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Shared.TagWrapper.Interface;
using Moq;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Global.Commands.Interface;
using Objects.Room.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class LogoutUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IMobileObjectCommand> save;
        Mock<IRoom> room;
        List<IPlayerCharacter> pcs;
        Mock<INotify> notify;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            save = new Mock<IMobileObjectCommand>();
            room = new Mock<IRoom>();
            pcs = new List<IPlayerCharacter>();
            notify = new Mock<INotify>();

            tagWrapper.Setup(e => e.WrapInTag("Logout", TagType.Info)).Returns("message");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(pcs);

            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Dictionary<string, IMobileObjectCommand> commands = new Dictionary<string, IMobileObjectCommand>();
            commands.Add("SAVE", save.Object);
            commandList.Setup(e => e.PcCommandsLookup).Returns(commands);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;


            command = new Logout();
        }

        [TestMethod]
        public void Logout_Instructions()
        {
            IResult result = command.Instructions;

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Logout_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Logout"));
        }

        [TestMethod]
        public void Logout_PerformCommand_NotAPc()
        {
            tagWrapper.Setup(e => e.WrapInTag("Only PlayerCharacters can logout.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Logout_PerformCommand_LoggedOut()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.Room).Returns(room.Object);
            pcs.Add(pc.Object);

            tagWrapper.Setup(e => e.WrapInTag("Exit Connection", TagType.Connection)).Returns("message");
            tagWrapper.Setup(e => e.WrapInTag("You have been successfully logged out.", TagType.Info)).Returns("EnqueueMessage");

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object), Times.Once);
            save.Verify(e => e.PerformCommand(pc.Object, mockCommand.Object), Times.Once);
            pc.VerifySet(e => e.Room = null);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
        }
    }
}
