using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Guild;
using Objects.Guild.Interface;
using Objects.Magic.Interface;
using Objects.Magic.Spell.Damage;
using Objects.Magic.Spell.Heal.Health;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class MagicUserTest
    {
        Objects.Personality.Personalities.MagicUser magicUser;
        Mock<INonPlayerCharacter> npc;
        Mock<IGuild> guild;
        DamageSpell damageSpell;
        CureSpell cureSpell;
        Mock<IDefaultValues> defaultValues;
        Mock<ITagWrapper> tagwrapper;
        Dictionary<string, ISpell> spellBook;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValues = new Mock<IDefaultValues>();
            tagwrapper = new Mock<ITagWrapper>();
            defaultValues.Setup(e => e.ReduceValues(1, 2)).Returns(new Dice(1, 2));
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;

            magicUser = new Objects.Personality.Personalities.MagicUser();
            npc = new Mock<INonPlayerCharacter>();
            guild = new Mock<IGuild>();
            damageSpell = new DamageSpell("damageSpell", 1, 2, Objects.Damage.Damage.DamageType.Cold, 10);
            cureSpell = new CureSpell("cureSpell", 1, 2, 10);
            GuildAbility guildAbility = new GuildAbility();
            GuildAbility guildAbility2 = new GuildAbility();
            spellBook = new Dictionary<string, ISpell>();

            npc.Setup(e => e.Level).Returns(5);
            npc.Setup(e => e.SpellBook).Returns(spellBook);
            npc.Setup(e => e.Health).Returns(90);
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.Mana).Returns(100);
            guildAbility.Level = 2;
            guildAbility2.Level = 2;
            guildAbility.Abiltiy = damageSpell;
            guildAbility2.Abiltiy = cureSpell;
            guild.Setup(e => e.Spells).Returns(new List<GuildAbility>() { guildAbility, guildAbility2 });

            GlobalReference.GlobalValues.TickCounter = 5;
        }

        [TestMethod]
        public void MagicUser_AddSpells()
        {
            magicUser.AddSpells(npc.Object, guild.Object);

            Assert.IsTrue(magicUser.DamageSpells.Contains(damageSpell));
            Assert.IsTrue(magicUser.CureSpell.Contains(cureSpell));
            Assert.IsTrue(spellBook.ContainsKey("DAMAGESPELL"));
            Assert.AreSame(damageSpell, spellBook["DAMAGESPELL"]);
            Assert.IsTrue(spellBook.ContainsKey("CURESPELL"));
            Assert.AreSame(cureSpell, spellBook["CURESPELL"]);
        }

        [TestMethod]
        public void MagicUser_AddSpells_DontAddToHighSpells()
        {
            npc.Setup(e => e.Level).Returns(1);

            magicUser.AddSpells(npc.Object, guild.Object);

            Assert.AreEqual(0, magicUser.DamageSpells.Count);
            Assert.AreEqual(0, magicUser.CureSpell.Count);
            Assert.AreEqual(0, spellBook.Count);
        }

        [TestMethod]
        public void MagicUser_Process()
        {
            npc.Setup(e => e.IsInCombat).Returns(true);
            magicUser.DamageSpells.Add(damageSpell);

            string result = magicUser.Process(npc.Object, null);

            Assert.AreEqual("Cast damageSpell", result);
        }

        [TestMethod]
        public void MagicUser_Process_NotEnoughMana()
        {
            npc.Setup(e => e.IsInCombat).Returns(true);
            npc.Setup(e => e.Mana).Returns(1);
            magicUser.DamageSpells.Add(damageSpell);

            string result = magicUser.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void MagicUser_Process_BelowHalfHealth()
        {
            npc.Setup(e => e.Health).Returns(10);
            magicUser.CureSpell.Add(cureSpell);

            string result = magicUser.Process(npc.Object, null);

            Assert.AreEqual("Cast cureSpell", result);
        }
    }

    public class DamageSpell : BaseDamageSpell
    {
        public DamageSpell(string spellName, int die, int sides, Objects.Damage.Damage.DamageType damageType, int manaCost = -1) : base(spellName, die, sides, damageType, manaCost)
        {
        }
    }

    public class CureSpell : BaseCureSpell
    {
        public CureSpell(string spellName, int die, int sides, int manaCost = -1) : base(spellName, die, sides)
        {
        }
    }
}
