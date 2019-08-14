using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality;
using Objects.Mob.Interface;
using Moq;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class AggressiveUnitTest
    {
        Aggressive aggressive;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            aggressive = new Aggressive();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();

            npc.Setup(e => e.Room).Returns(room.Object);
        }

        [TestMethod]
        public void Aggressive_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, aggressive.Process(npc.Object, command));
        }

        [TestMethod]
        public void Aggressive_Process_NoPcInRoom()
        {
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());

            Assert.IsNull(aggressive.Process(npc.Object, null));
        }

        [TestMethod]
        public void Aggressive_Process_CanNoSeePc()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<ICanMobDoSomething> canDo = new Mock<ICanMobDoSomething>();

            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            canDo.Setup(e => e.SeeObject(npc.Object, pc.Object)).Returns(false);

            GlobalReference.GlobalValues.CanMobDoSomething = canDo.Object;

            Assert.IsNull(aggressive.Process(npc.Object, null));
        }

        [TestMethod]
        public void Aggressive_Process_CanNoSee1stPc()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IPlayerCharacter> pc2 = new Mock<IPlayerCharacter>();
            Mock<ICanMobDoSomething> canDo = new Mock<ICanMobDoSomething>();

            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object, pc2.Object });
            canDo.Setup(e => e.SeeObject(npc.Object, pc.Object)).Returns(false);
            canDo.Setup(e => e.SeeObject(npc.Object, pc2.Object)).Returns(true);
            pc2.Setup(e => e.KeyWords).Returns(new List<string>() { "target" });

            GlobalReference.GlobalValues.CanMobDoSomething = canDo.Object;

            Assert.AreEqual("Kill target", aggressive.Process(npc.Object, null));
        }
    }
}
