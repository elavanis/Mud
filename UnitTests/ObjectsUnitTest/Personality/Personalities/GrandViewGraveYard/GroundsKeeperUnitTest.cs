using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.GrandViewGraveYard;
using Objects.Room.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Personality.Personalities.GrandViewGraveYard
{
    [TestClass]
    public class GroundsKeeperUnitTest
    {
        GroundsKeeper groundsKeeper;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        Mock<ICorpse> corpse;


        [TestInitialize]
        public void Setup()
        {
            groundsKeeper = new GroundsKeeper();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            corpse = new Mock<ICorpse>();

            npc.Setup(e => e.Room).Returns(room.Object);
            corpse.Setup(e => e.KeyWords).Returns(new List<string>() { "corpse" });
        }

        [TestMethod]
        public void GroundsKeeper_Process_CorpseFound()
        {
            room.Setup(e => e.Items).Returns(new List<IItem>() { corpse.Object });

            string result = groundsKeeper.Process(npc.Object, null);

            Assert.AreEqual("", result);
            npc.Verify(e => e.EnqueueCommand("Get corpse"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote The groundskeeper starts digging a grave for the corpse."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote The groundskeeper places the body in the grave."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Say And stay there this time."), Times.Once);
        }

        [TestMethod]
        public void GroundsKeeper_Process_NoCorpseFound()
        {
            room.Setup(e => e.Items).Returns(new List<IItem>());

            string result = groundsKeeper.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            npc.Verify(e => e.EnqueueCommand("Get corpse"), Times.Never);
            npc.Verify(e => e.EnqueueCommand("Emote The groundskeeper starts digging a grave for the corpse."), Times.Never);
            npc.Verify(e => e.EnqueueCommand("Emote The groundskeeper places the body in the grave."), Times.Never);
            npc.Verify(e => e.EnqueueCommand("Say And stay there this time."), Times.Never);
        }
    }
}
