using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Guild;
using System.Collections.Generic;
using Objects.Skill.Interface;
using static Objects.Guild.Guild;
using Objects.Magic.Interface;
using Objects.Guild;

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
            abiltites = new GuildAbilities();
        }

        [TestMethod]
        public void GuildAbilities_Skills()
        {
            List<GuildAbility> skill = abiltites.Skills[Guilds.Warrior];
            Assert.IsNotNull(skill);
            Assert.IsTrue(skill.Count > 0);
        }

        [TestMethod]
        public void GuildAbilities_Spells()
        {
            List<GuildAbility> spell = abiltites.Spells[Guilds.Mage];
            Assert.IsNotNull(spell);
            Assert.IsTrue(spell.Count > 0);
        }
    }
}
