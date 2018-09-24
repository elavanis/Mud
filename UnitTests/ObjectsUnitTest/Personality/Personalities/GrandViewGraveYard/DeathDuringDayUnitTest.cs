using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.GrandViewGraveYard;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.GrandViewGraveYard
{
    [TestClass]
    public class DeathDuringDayUnitTest
    {
        DeathDuringDay deathDuringDay;
        Mock<IGameDateTime> gameDateTime;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        Mock<ICorpse> corpse;
        Mock<List<IItem>> corpseItems;

        [TestInitialize]
        public void Setup()
        {
            deathDuringDay = new DeathDuringDay();
            gameDateTime = new Mock<IGameDateTime>();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            corpse = new Mock<ICorpse>();
            corpseItems = new Mock<List<IItem>>();

            npc.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Items).Returns(new List<IItem>() { corpse.Object });
            corpse.Setup(e => e.Items).Returns(corpseItems.Object);

            GlobalReference.GlobalValues.GameDateTime = gameDateTime.Object;
        }

        [TestMethod]
        public void DeathDuringDay_Process_Day()
        {
            gameDateTime.Setup(e => e.InGameDateTime).Returns(new DateTime(1, 1, 1, 1, 1, 1));

            string result = deathDuringDay.Process(npc.Object, null);

            Assert.AreEqual("", result);
            npc.Verify(e => e.Die(), Times.Once);
            //corpseItems.Verify(e => e.Clear(), Times.Once);
        }

        [TestMethod]
        public void DeathDuringDay_Process_Night()
        {
            gameDateTime.Setup(e => e.InGameDateTime).Returns(new DateTime(1, 1, 1, 12, 1, 1));

            string result = deathDuringDay.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            npc.Verify(e => e.Die(), Times.Never);
            //corpseItems.Verify(e => e.Clear(), Times.Once);
        }

        [TestMethod]
        public void DeathDuringDay_Process_RemoveCorpse()
        {
            gameDateTime.Setup(e => e.InGameDateTime).Returns(new DateTime(1, 1, 1, 1, 1, 1));
            room.Setup(e => e.Items).Returns(new List<IItem>() { corpse.Object, corpse.Object, corpse.Object, corpse.Object, corpse.Object, corpse.Object });

            string result = deathDuringDay.Process(npc.Object, null);

            Assert.AreEqual("", result);
            npc.Verify(e => e.Die(), Times.Once);
            room.Verify(e => e.RemoveItemFromRoom(corpse.Object), Times.Once);
            //corpseItems.Verify(e => e.Clear(), Times.Once);
        }
    }
}
