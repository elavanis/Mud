using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.God;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class PossessUnitTest
    {

        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<ICommand> commandMock;
        Mock<IMobileObject> mob;
        Mock<IFindObjects> find;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IParameter> parmeter;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            commandMock = new Mock<ICommand>();
            find = new Mock<IFindObjects>();
            room = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            parmeter = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag("Possess {Mob Keyword}", TagType.Info)).Returns("possess");
            tagWrapper.Setup(e => e.WrapInTag("You were not possessing anyone.", TagType.Info)).Returns("You were not possessing anyone.");
            tagWrapper.Setup(e => e.WrapInTag("You release control of mob.", TagType.Info)).Returns("You release control of mob.");
            tagWrapper.Setup(e => e.WrapInTag("You possessed pc.", TagType.Info)).Returns("You possessed pc.");
            tagWrapper.Setup(e => e.WrapInTag("You possessed npc.", TagType.Info)).Returns("You possessed npc.");
            tagWrapper.Setup(e => e.WrapInTag("You were unable to find other.", TagType.Info)).Returns("You were unable to find other.");
            mob.Setup(e => e.SentenceDescription).Returns("mob");
            find.Setup(e => e.FindNpcInRoom(room.Object, "npc")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            find.Setup(e => e.FindNpcInRoom(room.Object, "pc")).Returns(new List<INonPlayerCharacter>());
            find.Setup(e => e.FindNpcInRoom(room.Object, "other")).Returns(new List<INonPlayerCharacter>());
            find.Setup(e => e.FindPcInRoom(room.Object, "pc")).Returns(new List<IPlayerCharacter>() { pc.Object });
            find.Setup(e => e.FindPcInRoom(room.Object, "npc")).Returns(new List<IPlayerCharacter>());
            find.Setup(e => e.FindPcInRoom(room.Object, "other")).Returns(new List<IPlayerCharacter>());
            mob.Setup(e => e.Room).Returns(room.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = find.Object;

            command = new Possess();
        }

        [TestMethod]
        public void Possess_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("possess", result.ResultMessage);
        }

        [TestMethod]
        public void Possess_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Possess"));
        }

        [TestMethod]
        public void Possess_PerformCommand_NoParameterNoPossession()
        {
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were not possessing anyone.", result.ResultMessage);
        }

        [TestMethod]
        public void Possess_PerformCommand_NoParameterPossessing()
        {
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.PossedMob).Returns(mob.Object);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You release control of mob.", result.ResultMessage);
            mob.VerifySet(e => e.PossedMob = null);
            mob.VerifySet(e => e.PossingMob = null);
        }

        [TestMethod]
        public void Possess_PerformCommand_PossessPc()
        {
            parmeter.Setup(e => e.ParameterValue).Returns("pc");

            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter.Object });

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You possessed pc.", result.ResultMessage);
            pc.VerifySet(e => e.PossingMob = mob.Object);
            mob.VerifySet(e => e.PossedMob = pc.Object);
        }

        [TestMethod]
        public void Possess_PerformCommand_PossessNpc()
        {
            parmeter.Setup(e => e.ParameterValue).Returns("npc");
            mob.Setup(e => e.PossedMob).Returns(npc.Object);
            npc.Setup(e => e.PossingMob).Returns(pc.Object);

            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter.Object });

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You possessed npc.", result.ResultMessage);
            npc.VerifySet(e => e.PossingMob = null);
            npc.VerifySet(e => e.PossingMob = mob.Object);
            mob.VerifySet(e => e.PossedMob = npc.Object);
        }

        [TestMethod]
        public void Possess_PerformCommand_PossessNotFound()
        {
            parmeter.Setup(e => e.ParameterValue).Returns("other");

            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmeter.Object });

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You were unable to find other.", result.ResultMessage);
        }
    }
}
