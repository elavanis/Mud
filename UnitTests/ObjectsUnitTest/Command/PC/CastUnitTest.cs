using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.Interface;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Magic.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class CastUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("(C)ast [Spell Name] {Parameter(s)}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Cast();
        }

        [TestMethod]
        public void Cast_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Cast_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("C"));
            Assert.IsTrue(result.Contains("Cast"));
        }

        [TestMethod]
        public void Cast_PerformCommand_NoParameter()
        {
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("What spell would you like to cast?", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Cast_PerformCommand_SpellNotKnown()
        {
            Dictionary<string, ISpell> spellBook = new Dictionary<string, ISpell>();
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.SpellBook).Returns(spellBook);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("spell");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You do not know that spell.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Cast_PerformCommand_SpellKnown()
        {
            Dictionary<string, ISpell> spellBook = new Dictionary<string, ISpell>();
            Mock<ISpell> spell = new Mock<ISpell>();

            spellBook.Add("SPELL", spell.Object);
            Mock<IResult> mockResult = new Mock<IResult>();

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.SpellBook).Returns(spellBook);
            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("spell");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            spell.Setup(e => e.ProcessSpell(mob.Object, mockCommand.Object)).Returns(mockResult.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            spell.Verify(e => e.ProcessSpell(mob.Object, mockCommand.Object), Times.Once);
        }
    }
}
