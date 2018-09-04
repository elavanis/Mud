using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Global;
using System.Collections.Generic;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Item.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SellUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Sell {Item Keyword}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            room = new Mock<IRoom>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);

            command = new Sell();
        }

        [TestMethod]
        public void Sell_Instructions()
        {
            IResult result = command.Instructions;

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Sell_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Sell"));
        }

        [TestMethod]
        public void Sell_PerformCommand_NoMerchant()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();

            tagWrapper.Setup(e => e.WrapInTag("There is no merchant here to sell to.", TagType.Info)).Returns("message");
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { new Mock<IPersonality>().Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Sell_PerformCommand_Merchant()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IResult> mockResult = new Mock<IResult>();

            tagWrapper.Setup(e => e.WrapInTag("There is no merchant here to sell to.", TagType.Info)).Returns("message");
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            mob.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            merchant.Setup(e => e.Sell(npc.Object, mob.Object, item.Object)).Returns(mockResult.Object);
            parameter.Setup(e => e.ParameterValue).Returns("item");
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
        }

        [TestMethod]
        public void Sell_PerformCommand_MerchantItemNotFound()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IResult> mockResult = new Mock<IResult>();

            tagWrapper.Setup(e => e.WrapInTag("There is no merchant here to sell to.", TagType.Info)).Returns("message");
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            mob.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            merchant.Setup(e => e.Offer(npc.Object, mob.Object)).Returns(mockResult.Object);
            parameter.Setup(e => e.ParameterValue).Returns("notItem");
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
        }

        [TestMethod]
        public void Sell_PerformCommand_MerchantNoItem()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            Mock<IResult> mockResult = new Mock<IResult>();

            tagWrapper.Setup(e => e.WrapInTag("There is no merchant here to sell to.", TagType.Info)).Returns("message");
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Items).Returns(new List<IItem>());
            merchant.Setup(e => e.Offer(npc.Object, mob.Object)).Returns(mockResult.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
        }
    }
}
