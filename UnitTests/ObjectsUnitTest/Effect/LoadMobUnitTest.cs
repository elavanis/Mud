using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Notify.Interface;
using Objects.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class LoadMobUnitTest
    {
        LoadMob loadMob;
        Mock<IEffectParameter> effectParameter;
        Mock<ITranslationMessage> translationMessage;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> npc;
        Mock<INotify> notify;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;
        Mock<IBaseObjectId> roomId;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<IRoom> room2;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            loadMob = new LoadMob();
            effectParameter = new Mock<IEffectParameter>();
            translationMessage = new Mock<ITranslationMessage>();
            room = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            notify = new Mock<INotify>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();
            roomId = new Mock<IBaseObjectId>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            room2 = new Mock<IRoom>();

            effectParameter.Setup(e => e.Target).Returns(room.Object);
            effectParameter.Setup(e => e.Performer).Returns(npc.Object);
            effectParameter.Setup(e => e.RoomMessage).Returns(translationMessage.Object);
            npc.Setup(e => e.Clone()).Returns(npc.Object);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.Hour).Returns(1);
            roomId.Setup(e => e.Id).Returns(2);
            roomId.Setup(e => e.Zone).Returns(1);
            world.Setup(e => e.Zones).Returns(new Dictionary<int, IZone>() { { 1, zone.Object } });
            zone.Setup(e => e.Rooms).Returns(new Dictionary<int, IRoom>() { { 2, room2.Object } });


            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.World = world.Object;
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
            notify.Verify(e => e.Room(npc.Object, null, room.Object, translationMessage.Object, null, false, false), Times.Never);
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
            notify.Verify(e => e.Room(npc.Object, null, room.Object, translationMessage.Object, null, false, false), Times.Never);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_HoursNull()
        {
            loadMob.ProcessEffect(effectParameter.Object);

            room.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Once);
            npc.Verify(e => e.Clone(), Times.Once);
            npc.VerifySet(e => e.Room = room.Object, Times.Once);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
            notify.Verify(e => e.Room(npc.Object, null, room.Object, translationMessage.Object, null, false, false), Times.Once);
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
            notify.Verify(e => e.Room(npc.Object, null, room.Object, translationMessage.Object, null, false, false), Times.Once);
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
            notify.Verify(e => e.Room(npc.Object, null, room.Object, translationMessage.Object, null, false, false), Times.Never);
        }

        [TestMethod]
        public void LoadMob_ProcessEffect_LoadIntoRoomId()
        {
            loadMob.RoomId = roomId.Object;

            loadMob.ProcessEffect(effectParameter.Object);

            room2.Verify(e => e.AddMobileObjectToRoom(It.IsAny<IMobileObject>()), Times.Once);
            npc.Verify(e => e.Clone(), Times.Once);
            npc.VerifySet(e => e.Room = room2.Object, Times.Once);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
            notify.Verify(e => e.Room(npc.Object, null, room2.Object, translationMessage.Object, null, false, false), Times.Once);
        }
    }
}