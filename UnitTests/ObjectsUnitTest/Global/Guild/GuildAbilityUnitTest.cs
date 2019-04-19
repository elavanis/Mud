using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Guild;
using System.Collections.Generic;
using static Objects.Guild.Guild;
using Objects.Guild;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Moq;
using Objects.Die.Interface;
using Shared.TagWrapper.Interface;
using Objects.Global.Language.Interface;
using static Objects.Global.Language.Translator;

namespace ObjectsUnitTest.Global.Guild
{
    //TODO Come up with a better unit test
    [TestClass]
    public class GuildAbilityUnitTest
    {
        GuildAbilities abiltites;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<ITranslator> translator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            translator = new Mock<ITranslator>();

            defaultValues.Setup(e => e.DiceForSpellLevel(It.IsAny<int>())).Returns(dice.Object);
            defaultValues.Setup(e => e.DiceForSkillLevel(It.IsAny<int>())).Returns(dice.Object);
            translator.Setup(e => e.Translate(It.IsAny<Languages>(), It.IsAny<string>())).Returns((Languages x, string y) => (y));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Translator = translator.Object;

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
