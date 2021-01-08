using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Personality.Interface;
using static Objects.Mob.MobileObject;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class BuyUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IResult> mockResult;
        Mock<IMobileObject> mobileObject;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> npc;
        Mock<IMerchant> merchant;
        Mock<IPersonality> personality;
        Mock<ICommand> mockCommand;
        Mock<IParameter> parmaeter;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mockResult = new Mock<IResult>();
            mobileObject = new Mock<IMobileObject>();
            room = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            merchant = new Mock<IMerchant>();
            personality = new Mock<IPersonality>();
            mockCommand = new Mock<ICommand>();
            parmaeter = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            merchant.Setup(e => e.Buy(npc.Object, mobileObject.Object, 1)).Returns(mockResult.Object);
            merchant.Setup(e => e.List(npc.Object, mobileObject.Object)).Returns(mockResult.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            mobileObject.Setup(e => e.Room).Returns(room.Object);

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
            parmaeter.Setup(e => e.ParameterValue).Returns("1");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmaeter.Object });

            Assert.AreSame(mockResult.Object, command.PerformCommand(mobileObject.Object, mockCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_List()
        {
            parmaeter.Setup(e => e.ParameterValue).Returns("0");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parmaeter.Object });

            Assert.AreSame(mockResult.Object, command.PerformCommand(mobileObject.Object, mockCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_NoParameter()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Assert.AreSame(mockResult.Object, command.PerformCommand(mobileObject.Object, mockCommand.Object));
        }

        [TestMethod]
        public void Buy_PerformCommand_NoMerchant()
        {
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mobileObject.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("There is no merchant here to sell to you.", result.ResultMessage);
        }
    }
}
