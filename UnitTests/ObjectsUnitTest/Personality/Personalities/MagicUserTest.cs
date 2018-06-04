using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Ability.Interface;
using Objects.Damage;
using Objects.Guild.Interface;
using Objects.Magic.Spell.Damage;
using Objects.Magic.Spell.Heal.Health;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

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

        [TestInitialize]
        public void Setup()
        {
            magicUser = new Objects.Personality.Personalities.MagicUser();
            npc = new Mock<INonPlayerCharacter>();
            guild = new Mock<IGuild>();
            damageSpell = new DamageSpell("damageSpell", 1, 2, Objects.Damage.Damage.DamageType.Cold);
            cureSpell = new CureSpell("cureSpell", 1, 2);

            guild.Setup(e => e.Spells).Returns(new List<Objects.Guild.GuildAbility>() { })
        }

        [TestMethod]
        public void MagicUser_AddSpells()
        {

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
