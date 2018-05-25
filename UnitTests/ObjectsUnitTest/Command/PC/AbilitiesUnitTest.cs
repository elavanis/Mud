using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Magic;
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
        IMobileObjectCommand command;
        Mock<IMobileObject> mob;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Abils|Abilities", TagType.Info)).Returns("message");
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

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
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
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Spells\r\nspell\r\n\r\nSkills\r\nskill  - Active\r\nskill2 - Passive", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = command.PerformCommand(mob.Object, null);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Abilities_PerformCommand_NoAbilities()
        {
            mob.Setup(e => e.SpellBook).Returns(new Dictionary<string, ISpell>());
            mob.Setup(e => e.KnownSkills).Returns(new Dictionary<string, ISkill>());

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Spells\r\n<None>\r\n\r\nSkills\r\n<None>", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            IResult result = command.PerformCommand(mob.Object, null);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
