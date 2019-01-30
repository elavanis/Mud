using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using Objects.World.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class BuyUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Buy();
        }

        [TestMethod]
        public void Buy_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Buy {Item Number}", result.ResultMessage);
        }

        [TestMethod]
        public void Buy_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Buy"));
        }

        [TestMethod]
        public void Buy_PerformCommand_Buy()
        {
            Mock<IResult> mockResult = new Mock<IResult>();

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            merchant.Setup(e => e.Buy(npc.Object, mob.Object, 1)).Returns(mockResult.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ICommand> mockedCommand = new Mock<ICommand>();
            Mock<IParameter> parmaeter = new Mock<IParameter>();
            parmaeter.Setup(e => e.ParameterValue).Returns("1");
            mockedCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmaeter.Object });

            Assert.AreSame(mockResult.Object, command.PerformCommand(mob.Object, mockedCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_List()
        {
            Mock<IResult> mockResult = new Mock<IResult>();

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            merchant.Setup(e => e.List(npc.Object, mob.Object)).Returns(mockResult.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ICommand> mockedCommand = new Mock<ICommand>();
            Mock<IParameter> parmaeter = new Mock<IParameter>();
            parmaeter.Setup(e => e.ParameterValue).Returns("0");
            mockedCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmaeter.Object });

            Assert.AreSame(mockResult.Object, command.PerformCommand(mob.Object, mockedCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_NoParameter()
        {
            Mock<IResult> mockResult = new Mock<IResult>();

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            merchant.Setup(e => e.List(npc.Object, mob.Object)).Returns(mockResult.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ICommand> mockedCommand = new Mock<ICommand>();
            mockedCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Assert.AreSame(mockResult.Object, command.PerformCommand(mob.Object, mockedCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_NoMerchant()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IPersonality> personality = new Mock<IPersonality>();
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            mob.Setup(e => e.Room).Returns(room.Object);

            Mock<ICommand> mockedCommand = new Mock<ICommand>();
            mockedCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockedCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no merchant here to sell to you.", result.ResultMessage);
        }
    }
}
