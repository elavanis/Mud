using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality;
using Moq;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class GuardianUnitTest
    {
        Guardian guardian;
        Mock<INonPlayerCharacter> npc;
        Mock<INonPlayerCharacter> npc2;
        Mock<IPlayerCharacter> pc;
        Mock<IRoom> room;
        Mock<IEngine> engine;
        Mock<ICombat> combat;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            guardian = new Guardian();
            npc = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            room = new Mock<IRoom>();
            engine = new Mock<IEngine>();
            combat = new Mock<ICombat>();

            npc.Setup(e => e.Room).Returns(room.Object);
            engine.Setup(e => e.Combat).Returns(combat.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;
        }

        [TestMethod]
        public void Guardian_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, guardian.Process(npc.Object, command));
        }

        [TestMethod]
        public void Guardian_Process_NoPc()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());

            Assert.IsNull(guardian.Process(npc.Object, null));
        }

        [TestMethod]
        public void Guardian_Process_NoNPc()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());

            Assert.IsNull(guardian.Process(npc.Object, null));
        }

        [TestMethod]
        public void Guardian_Process_GuardianFighting()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object, npc2.Object });

            combat.Setup(e => e.IsInCombat(npc.Object)).Returns(true);

            Assert.IsNull(guardian.Process(npc.Object, null));
        }

        [TestMethod]
        public void Guardian_Process_PcNotInCombat()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object, npc2.Object });

            Assert.IsNull(guardian.Process(npc.Object, null));
        }

        [TestMethod]
        public void Guardian_Process_PcFightingButNotWithNpc()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object, npc2.Object });
            pc.Setup(e => e.IsInCombat).Returns(true);

            Assert.IsNull(guardian.Process(npc.Object, null));
        }

        [TestMethod]
        public void Guardian_Process_PcFightingWithNpc()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object, npc2.Object });
            pc.Setup(e => e.IsInCombat).Returns(true);
            pc.Setup(e => e.AreFighting(npc2.Object)).Returns(true);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "pc" });

            Assert.AreEqual("kill pc", guardian.Process(npc.Object, null));
        }
    }
}
