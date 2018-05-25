using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class HealerUnitTest
    {
        IHealer healer;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        Mock<IPlayerCharacter> pc;
        Mock<IRandom> random;
        [TestInitialize]
        public void Setup()
        {
            healer = new Healer();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            pc = new Mock<IPlayerCharacter>();
            random = new Mock<IRandom>();
            Mock<ISpell> spell = new Mock<ISpell>();
            Dictionary<string, ISpell> spellBook = new Dictionary<string, ISpell>();

            healer.CastPercent = 100;
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            random.Setup(e => e.PercentDiceRoll(100)).Returns(true);
            npc.Setup(e => e.Room).Returns(room.Object);
            npc.Setup(e => e.SpellBook).Returns(spellBook);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "PC" });
            spell.Setup(e => e.SpellName).Returns("spell");
            spellBook.Add(spell.Object.SpellName, spell.Object);

            GlobalReference.GlobalValues.Random = random.Object;

        }

        [TestMethod]
        public void Healer_Constructor()
        {
            healer = new Healer(10);

            Assert.AreEqual(10, healer.CastPercent);
        }

        [TestMethod]
        public void Healer_Process_CommandNotNull()
        {
            string command = "command";

            Assert.AreSame(command, healer.Process(npc.Object, command));
        }

        [TestMethod]
        public void Healer_Process_NoPc()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());

            Assert.IsNull(healer.Process(npc.Object, null));
        }

        [TestMethod]
        public void Healer_Process_PcPresent()
        {
            Assert.AreEqual("cast spell PC", healer.Process(npc.Object, null));
        }
    }
}
