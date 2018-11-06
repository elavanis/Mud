using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality.Personalities;
using static Objects.Guild.Guild;
using Objects.Mob.Interface;
using Moq;
using System.Collections.Generic;
using Objects.Personality.Interface;
using Objects.Command.Interface;
using Objects.Guild.Guilds;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Global.Guild.Interface;
using Objects.Skill.Interface;
using Objects.Magic.Interface;
using Objects.Guild;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class GuildMasterUnitTest
    {
        GuildMaster guildMaster;
        HashSet<Guilds> guildHashSet;
        Dictionary<string, ISkill> knowSkills;
        Dictionary<string, ISpell> spellBook;
        Mock<IMobileObject> mob;
        Mock<INonPlayerCharacter> npc;
        Mock<ITagWrapper> tagwrapper;
        Mock<IGuildAbilities> guildAbilites;
        [TestInitialize]
        public void Setup()
        {
            guildMaster = new GuildMaster();
            guildMaster.Guild = Guilds.Wizard;

            guildHashSet = new HashSet<Guilds>();

            knowSkills = new Dictionary<string, ISkill>();
            spellBook = new Dictionary<string, ISpell>();

            mob = new Mock<IMobileObject>();
            npc = new Mock<INonPlayerCharacter>();
            tagwrapper = new Mock<ITagWrapper>();
            guildAbilites = new Mock<IGuildAbilities>();
            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;

            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "mob" });
            mob.Setup(e => e.Guild).Returns(guildHashSet);
            mob.Setup(e => e.KnownSkills).Returns(knowSkills);
            mob.Setup(e => e.SpellBook).Returns(spellBook);

            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { guildMaster });

            Dictionary<Guilds, List<GuildAbility>> skills = new Dictionary<Guilds, List<GuildAbility>>();
            Mock<ISkill> skill = new Mock<ISkill>();
            skill.Setup(e => e.ToString()).Returns("skill");
            skill.Setup(e => e.TeachMessage).Returns("skill teach message");
            skill.Setup(e => e.AbilityName).Returns("skill");
            skills.Add(Guilds.Wizard, new List<GuildAbility>() { new GuildAbility(skill.Object, 5) });
            guildAbilites.Setup(e => e.Skills).Returns(skills);

            Dictionary<Guilds, List<GuildAbility>> spells = new Dictionary<Guilds, List<GuildAbility>>();
            Mock<ISpell> spell = new Mock<ISpell>();
            spell.Setup(e => e.ToString()).Returns("spell");
            spell.Setup(e => e.TeachMessage).Returns("spell teach message");
            spell.Setup(e => e.AbilityName).Returns("spell");
            spells.Add(Guilds.Wizard, new List<GuildAbility>() { new GuildAbility(spell.Object, 5) });
            guildAbilites.Setup(e => e.Spells).Returns(spells);
            GlobalReference.GlobalValues.GuildAbilities = guildAbilites.Object;

            tagwrapper.Setup(e => e.WrapInTag("", TagType.Info)).Returns("message");
        }


        [TestMethod]
        public void GuildMaster_Constructor()
        {
            GuildMaster guildMaster = new GuildMaster(Guilds.Gladiator);

            Assert.AreEqual(Guilds.Gladiator, guildMaster.Guild);
        }

        [TestMethod]
        public void GuildMaster_Join_JoinsMage()
        {
            mob.Setup(e => e.GuildPoints).Returns(1);

            IResult result = guildMaster.Join(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob Welcome to the Wizard guild."), Times.Once);
            mob.VerifySet(e => e.GuildPoints = 0);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            Assert.IsTrue(guildHashSet.Contains(Guilds.Wizard));
        }

        [TestMethod]
        public void GuildMaster_Join_NotEnoughGuildPoints()
        {
            mob.Setup(e => e.GuildPoints).Returns(0);

            IResult result = guildMaster.Join(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob You need to gain more experience in the world before I can allow you to join the Wizard guild."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Join_AlreadyJoinedMage()
        {
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.GuildPoints).Returns(1);

            IResult result = guildMaster.Join(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob You are already a member of the Wizard guild."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_NotPartOfGuildNoPoints()
        {
            IResult result = guildMaster.Teach(npc.Object, mob.Object, "learnable");

            npc.Verify(e => e.EnqueueCommand("Tell mob I'm sorry but I can not teach you until you gain more experience and join our guild."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_NotPartOfGuildHasPoints()
        {
            mob.Setup(e => e.GuildPoints).Returns(1);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "learnable");

            npc.Verify(e => e.EnqueueCommand("Tell mob Please join our guild so that I may teach you our ways."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_AbilityNotFound()
        {
            guildHashSet.Add(Guilds.Wizard);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "learnable");

            npc.Verify(e => e.EnqueueCommand("Tell mob I can not teach you that."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_SkillNotHighEnough()
        {
            guildHashSet.Add(Guilds.Wizard);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "skill");

            npc.Verify(e => e.EnqueueCommand("Tell mob You are not high enough level for that yet."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_SpellNotHighEnough()
        {
            guildHashSet.Add(Guilds.Wizard);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "spell");

            npc.Verify(e => e.EnqueueCommand("Tell mob You are not high enough level for that yet."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_LearnSkill()
        {
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "skill");

            npc.Verify(e => e.EnqueueCommand("Tell mob skill teach message"), Times.Once);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            Assert.IsTrue(knowSkills.ContainsKey("SKILL"));
        }

        [TestMethod]
        public void GuildMaster_Teach_LearnSpell()
        {
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "spell");

            npc.Verify(e => e.EnqueueCommand("Tell mob spell teach message"), Times.Once);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            Assert.IsTrue(spellBook.ContainsKey("SPELL"));
        }

        [TestMethod]
        public void GuildMaster_Teach_AlreadyKnowSkill()
        {
            knowSkills.Add("SKILL", null);
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "skill");

            npc.Verify(e => e.EnqueueCommand("Tell mob You already know that skill."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teach_AlreadyKnowSpell()
        {
            spellBook.Add("SPELL", null);
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teach(npc.Object, mob.Object, "spell");

            npc.Verify(e => e.EnqueueCommand("Tell mob You already know that spell."), Times.Once);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teachable_AlreadyKnowSkill()
        {
            knowSkills.Add("skill", null);
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teachable(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob I can teach you the following. \r\nspell"), Times.Once);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teachable_AlreadyKnowSpell()
        {
            spellBook.Add("spell", null);
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(10);

            IResult result = guildMaster.Teachable(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob I can teach you the following. \r\nskill"), Times.Once);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Teachable_NotHighEnoughLevel()
        {
            guildHashSet.Add(Guilds.Wizard);
            mob.Setup(e => e.Level).Returns(1);

            IResult result = guildMaster.Teachable(npc.Object, mob.Object);

            npc.Verify(e => e.EnqueueCommand("Tell mob I can not teach you anything at this time."), Times.Once);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void GuildMaster_Process_ReturnsSent()
        {
            string sent = "sent";

            Assert.AreSame(sent, guildMaster.Process(null, sent));
        }
    }
}
