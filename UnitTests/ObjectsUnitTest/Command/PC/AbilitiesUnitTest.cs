using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Magic.Interface;
using System.Collections.Generic;
using Objects.Skill.Interface;
using Objects.Command.Interface;
using System.Linq;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class AbilitiesUnitTest
    {
        IMobileObjectCommand command = null!;
        Mock<IMobileObject> mob = null!;
        Mock<ITagWrapper> tagWrapper = null!;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Abilities();
            mob = new Mock<IMobileObject>();
            Mock<ISpell> spell = new Mock<ISpell>();
            Mock<ISkill> skill = new Mock<ISkill>();
            Mock<ISkill> skill2 = new Mock<ISkill>();
            skill2.Setup(e => e.Passive).Returns(true);
            Dictionary<string, ISpell> spellBook = new Dictionary<string, ISpell>();
            spellBook.Add("spell", spell.Object);
            mob.Setup(e => e.SpellBook).Returns(spellBook);

            Dictionary<string, ISkill> skills = new Dictionary<string, ISkill>();
            skills.Add("skill", skill.Object);
            skills.Add("skill2", skill2.Object);
            mob.Setup(e => e.KnownSkills).Returns(skills);
        }

        [TestMethod]
        public void Abilities_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Abils|Abilities", result.ResultMessage);
        }

        [TestMethod]
        public void Abilities_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("Abils"));
            Assert.IsTrue(result.Contains("Abilities"));
        }

        [TestMethod]
        public void Abilities_PerformCommand_Abilities()
        {
            IResult result = command.PerformCommand(mob.Object, new Objects.Command.Command());
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Spells\r\nspell\r\n\r\nSkills\r\nskill  - Active\r\nskill2 - Passive", result.ResultMessage);
        }

        [TestMethod]
        public void Abilities_PerformCommand_NoAbilities()
        {
            mob.Setup(e => e.SpellBook).Returns(new Dictionary<string, ISpell>());
            mob.Setup(e => e.KnownSkills).Returns(new Dictionary<string, ISkill>());

            IResult result = command.PerformCommand(mob.Object, new Objects.Command.Command());
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("Spells\r\n<None>\r\n\r\nSkills\r\n<None>", result.ResultMessage);
        }
    }
}
