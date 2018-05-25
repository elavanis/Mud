using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Guild;
using Objects.Magic;
using Objects.Magic.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ObjectsUnitTest.Guild
{
    [TestClass]
    public class BaseGuildUnitTest
    {
        UnitTestBaseGuild baseGuild;

        [TestInitialize]
        public void Setup()
        {
            baseGuild = new UnitTestBaseGuild();
        }

        [TestMethod]
        public void BaseGuild_Skills()
        {
            List<GuildAbility> skills = baseGuild.Skills;
            Assert.AreEqual(0, skills.Count);
        }

        [TestMethod]
        public void BaseGuild_Spells()
        {
            List<GuildAbility> spells = baseGuild.Spells;
            Assert.AreEqual(0, spells.Count);
        }

        private class UnitTestBaseGuild : BaseGuild
        {
            protected override List<GuildAbility> GenerateSkills()
            {
                return new List<GuildAbility>();
            }

            protected override List<GuildAbility> GenerateSpells()
            {
                return new List<GuildAbility>();
            }
        }
    }
}
