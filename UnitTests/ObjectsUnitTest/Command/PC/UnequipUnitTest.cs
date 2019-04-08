using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Item.Items.Interface;
using Objects.Item.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class UnequipUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Unequip();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
        }

        [TestMethod]
        public void Unequip_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Unequip [Item Name]", result.ResultMessage);
        }

        [TestMethod]
        public void Unequip_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Unequip"));
        }

        [TestMethod]
        public void Unequip_PerformCommand_NoParameter()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What would you like to unequip?", result.ResultMessage);
        }

        [TestMethod]
        public void Unequip_PerformCommand_EquipmentNotFound()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();

            parameter.Setup(e => e.ParameterValue).Returns("helmet");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not appear to have helmet equipped.", result.ResultMessage);
        }

        [TestMethod]
        public void Unequip_PerformCommand_EquipmentFound()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            Mock<IEquipment> equipment2 = new Mock<IEquipment>();
            List<IItem> items = new List<IItem>();

            parameter.Setup(e => e.ParameterValue).Returns("helmet");
            parameter.Setup(e => e.ParameterNumber).Returns(1);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            equipment.Setup(e => e.KeyWords).Returns(new List<string>() { "helmet" });
            equipment2.Setup(e => e.KeyWords).Returns(new List<string>() { "helmet" });
            equipment2.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equipment.Object, equipment2.Object });
            mob.Setup(e => e.Items).Returns(items);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You removed SentenceDescription.", result.ResultMessage);
            mob.Verify(e => e.RemoveEquipment(equipment2.Object), Times.Once);
            mob.Verify(e => e.ResetMaxStatValues(), Times.Once);
            Assert.AreEqual(1, items.Count);
            Assert.IsTrue(items.Contains(equipment2.Object));
        }
    }
}

