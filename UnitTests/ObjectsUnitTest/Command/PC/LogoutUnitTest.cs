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
using Objects.World.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class LogoutUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<IPlayerCharacter> pc;
        Mock<ICommand> mockCommand;
        Mock<INotify> notify;
        Mock<IWorld> world;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            pc = new Mock<IPlayerCharacter>();
            mockCommand = new Mock<ICommand>();
            notify = new Mock<INotify>();
            world = new Mock<IWorld>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Connection)).Returns((string x, TagType y) => (x));
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.World = world.Object;


            command = new Logout();
        }

        [TestMethod]
        public void Logout_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Logout", result.ResultMessage);
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

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Only PlayerCharacters can logout.", result.ResultMessage);
        }

        [TestMethod]
        public void Logout_PerformCommand_LoggedOut()
        {

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Exit Connection", result.ResultMessage);
            world.Verify(e => e.LogOutCharacter(pc.Object), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
        }
    }
}
