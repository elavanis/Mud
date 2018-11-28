using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SpellbookUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<ISpell> spell1;
        Mock<ISpell> spell2;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Spellbook();
            spell1 = new Mock<ISpell>();
            spell2 = new Mock<ISpell>();
            Dictionary<string, ISpell> spells = new Dictionary<string, ISpell>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            spell1.Setup(e => e.AbilityName).Returns("ABC");
            spell1.Setup(e => e.ManaCost).Returns(1);
            spell2.Setup(e => e.AbilityName).Returns("AHHHHH");
            spell2.Setup(e => e.ManaCost).Returns(2);
            spells.Add(spell1.Object.AbilityName, spell1.Object);
            spells.Add(spell2.Object.AbilityName, spell2.Object);

            mob.Setup(e => e.SpellBook).Returns(spells);
        }

        [TestMethod]
        public void Spellbook_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Spellbook", result.ResultMessage);
        }

        [TestMethod]
        public void Spellbook_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Spellbook"));
        }

        [TestMethod]
        public void Spellbook_PerformCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Spell   Mana Cost");
            stringBuilder.AppendLine("ABC     1");
            stringBuilder.AppendLine("AHHHHH  2");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(stringBuilder.ToString().Trim(), result.ResultMessage);
        }
    }
}
