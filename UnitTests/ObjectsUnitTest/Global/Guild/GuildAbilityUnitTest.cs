using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Guild;
using System.Collections.Generic;
using Objects.Skill.Interface;
using static Objects.Guild.Guild;
using Objects.Magic.Interface;
using Objects.Guild;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Moq;
using Objects.Die.Interface;
using Shared.TagWrapper.Interface;

namespace ObjectsUnitTest.Global.Guild
{
    //TODO Come up with a better unit test
    [TestClass]
    public class GuildAbilityUnitTest
    {
        GuildAbilities abiltites;

        [TestInitialize]
        public void Setup()
        {
            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(It.IsAny<int>())).Returns(dice.Object);
            defaultValues.Setup(e => e.DiceForSkillLevel(It.IsAny<int>())).Returns(dice.Object);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            abiltites = new GuildAbilities();
        }

        [TestMethod]
        public void GuildAbilities_Skills()
        {
            List<GuildAbility> skill = abiltites.Skills[Guilds.Gladiator];
            Assert.IsNotNull(skill);
            Assert.IsTrue(skill.Count > 0);
        }

        [TestMethod]
        public void GuildAbilities_Spells()
        {
            List<GuildAbility> spell = abiltites.Spells[Guilds.Wizard];
            Assert.IsNotNull(spell);
            Assert.IsTrue(spell.Count > 0);
        }
    }
}
