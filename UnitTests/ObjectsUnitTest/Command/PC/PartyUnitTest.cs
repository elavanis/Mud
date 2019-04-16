using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
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
    public class PartyUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IParameter> parameterInvite;
        Mock<IParameter> parameterDeclineInvite;
        Mock<IParameter> parameterOther1;
        Mock<IParameter> parameterOther2;
        Mock<IPlayerCharacter> pc;
        Mock<IWorld> world;
        Mock<IParty> party;
        Mock<IEngine> engine;
        Mock<INotify> notify;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Communication)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            parameterInvite = new Mock<IParameter>();
            parameterDeclineInvite = new Mock<IParameter>();
            parameterOther1 = new Mock<IParameter>();
            parameterOther2 = new Mock<IParameter>();
            pc = new Mock<IPlayerCharacter>();
            world = new Mock<IWorld>();
            party = new Mock<IParty>();
            engine = new Mock<IEngine>();
            notify = new Mock<INotify>();

            parameterInvite.Setup(e => e.ParameterValue).Returns("invite");
            parameterDeclineInvite.Setup(e => e.ParameterValue).Returns("decline");
            parameterOther1.Setup(e => e.ParameterValue).Returns("other1");
            parameterOther2.Setup(e => e.ParameterValue).Returns("other2");
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "other1" });
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>() { pc.Object });
            engine.Setup(e => e.Party).Returns(party.Object);
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "performer" });

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            command = new Party();
        }

        [TestMethod]
        public void Party_WritePartyUnitTests()
        {
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void Party_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Party Invite {Person To Invite}
Party Decline
Party {Message To Send To Party}", result.ResultMessage);
        }

        [TestMethod]
        public void Party_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Party"));
        }

        [TestMethod]
        public void Party_PerformCommand_NoParmaeters()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(@"Party Invite {Person To Invite}
Party Decline
Party {Message To Send To Party}", result.ResultMessage);
        }

        [TestMethod]
        public void Party_PerformCommand_Invite()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterInvite.Object, parameterOther1.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            party.Verify(e => e.Invite(mob.Object, pc.Object), Times.Once);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Party_PerformCommand_Invite_CantFindPlayer()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterInvite.Object, parameterOther2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual("Unable to find player other2.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_PerformCommand_Decline()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterDeclineInvite.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            party.Verify(e => e.DeclinePartyInvite(mob.Object), Times.Once);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Party_PerformCommand_Chat_NoParty()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterOther1.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.AreEqual("You are not in a party so you can't not chat with them.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_PerformCommand_Chat_InParty()
        {
            party.Setup(e => e.CurrentPartyMembers(mob.Object)).Returns(new List<IMobileObject>() { mob.Object, pc.Object } as IReadOnlyList<IMobileObject>);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterOther1.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsNull(result);
            notify.Verify(e => e.Mob(mob.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: other1")), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: other1")), Times.Once);
        }

        [TestMethod]
        public void Party_PerformCommand_Chat_FalseInvite()
        {
            party.Setup(e => e.CurrentPartyMembers(mob.Object)).Returns(new List<IMobileObject>() { mob.Object, pc.Object } as IReadOnlyList<IMobileObject>);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterInvite.Object, parameterOther1.Object, parameterOther2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsNull(result);
            notify.Verify(e => e.Mob(mob.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: invite other1 other2")), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: invite other1 other2")), Times.Once);
        }

        [TestMethod]
        public void Party_PerformCommand_Chat_FalseDecline()
        {
            party.Setup(e => e.CurrentPartyMembers(mob.Object)).Returns(new List<IMobileObject>() { mob.Object, pc.Object } as IReadOnlyList<IMobileObject>);

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterDeclineInvite.Object, parameterOther1.Object, parameterOther2.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsNull(result);
            notify.Verify(e => e.Mob(mob.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: decline other1 other2")), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "performer party chats: decline other1 other2")), Times.Once);
        }
    }
}