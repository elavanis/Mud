using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Notify.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class LoadMobUnitTest
    {
        LoadMob loadMob;
        Mock<IEffectParameter> effectParameter;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> npc;
        Mock<INotify> notify;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            loadMob = new LoadMob();
            effectParameter = new Mock<IEffectParameter>();
            room = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            notify = new Mock<INotify>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();

            effectParameter.Setup(e => e.Target).Returns(room.Object);
            effectParameter.Setup(e => e.Performer).Returns(npc.Object);
            npc.Setup(e => e.Clone()).Returns(npc.Object);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.Hour).Returns(1);

            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_RoomNull()
        {
            effectParameter.Setup(e => e.Target).Returns<IRoom>(null);

            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Never);
            npc.Verify(e => e.Clone(), Times.Never);
            npc.VerifySet(e => e.Room = room.Object, Times.Never);
            npc.Verify(e => e.FinishLoad(-1), Times.Never);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_PerformerNull()
        {
            effectParameter.Setup(e => e.Performer).Returns<IMobileObject>(null);

            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Never);
            npc.Verify(e => e.Clone(), Times.Never);
            npc.VerifySet(e => e.Room = room.Object, Times.Never);
            npc.Verify(e => e.FinishLoad(-1), Times.Never);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_HoursNull()
        {
            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Once);
            npc.Verify(e => e.Clone(), Times.Once);
            npc.VerifySet(e => e.Room = room.Object, Times.Once);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_HoursMatch()
        {
            loadMob.HoursToLoad = new List<int>() { 1 };

            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Once);
            npc.Verify(e => e.Clone(), Times.Once);
            npc.VerifySet(e => e.Room = room.Object, Times.Once);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_HoursDontMatch()
        {
            loadMob.HoursToLoad = new List<int>() { 2 };

            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Never);
            npc.Verify(e => e.Clone(), Times.Never);
            npc.VerifySet(e => e.Room = room.Object, Times.Never);
            npc.Verify(e => e.FinishLoad(-1), Times.Never);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_UpdateUnitTests()
        {
            Assert.AreEqual(1, 2);
        }
    }
}