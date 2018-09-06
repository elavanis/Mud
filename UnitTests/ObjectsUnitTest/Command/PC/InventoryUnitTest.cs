using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Item.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class InventoryUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("(Inv)entory", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Inventory();
        }

        [TestMethod]
        public void Inventory_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Inventory_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("Inv"));
            Assert.IsTrue(result.Contains("Inventory"));
        }

        [TestMethod]
        public void Inventory_PerformCommand()
        {
            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            tagWrapper.Setup(e => e.WrapInTag("ShortDescription", TagType.Item)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Inventory_PerformCommand_NoInventory()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(new List<IItem>());
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            tagWrapper.Setup(e => e.WrapInTag("You are not carrying anything.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
