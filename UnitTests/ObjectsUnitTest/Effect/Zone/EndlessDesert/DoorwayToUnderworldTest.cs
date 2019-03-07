using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Effect.Zone.EndlessDesert;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Interface;
using Objects.Global.Notify.Interface;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Room.Room;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Effect.Zone.EndlessDesert
{
    [TestClass]
    public class DoorwayToUnderworldTest
    {
        Mock<IRoom> room;
        Mock<IEffectParameter> effectParameter;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<INonPlayerCharacter> mockNpc;
        Mock<IPlayerCharacter> mockPc;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IGameDateTime> gameDateTime;
        HashSet<RoomAttribute> roomAttrbuties;

        DoorwayToUnderworld doorwayToUnderworld;

        [TestInitialize]
        public void Setup()
        {
            doorwayToUnderworld = new DoorwayToUnderworld();
            room = new Mock<IRoom>();
            effectParameter = new Mock<IEffectParameter>();
            inGameDateTime = new Mock<IInGameDateTime>();
            mockNpc = new Mock<INonPlayerCharacter>();
            mockPc = new Mock<IPlayerCharacter>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            roomAttrbuties = new HashSet<RoomAttribute>();
            gameDateTime = new Mock<IGameDateTime>();

            room.Setup(e => e.Attributes).Returns(roomAttrbuties);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { mockNpc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { mockPc.Object });
            effectParameter.Setup(e => e.Target).Returns(room.Object);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.Hour).Returns(12);
            gameDateTime.Setup(e => e.DayName).Returns(Objects.GameDateTime.Days.Death);

            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void DoorwayToUnderworldTest_ProcessEffect_AddDoor()
        {
            doorwayToUnderworld.ProcessEffect(effectParameter.Object);

            Assert.IsTrue(roomAttrbuties.Contains(Objects.Room.Room.RoomAttribute.Light));
            Assert.AreEqual(1, roomAttrbuties.Count);
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }

        [TestMethod]
        public void DoorwayToUnderworldTest_ProcessEffect_RoomDoor()
        {
            gameDateTime.Setup(e => e.Hour).Returns(1);
            Mock<IExit> exit = new Mock<IExit>();

            room.Setup(e => e.Down).Returns(exit.Object);
            roomAttrbuties.Add(Objects.Room.Room.RoomAttribute.Light);

            doorwayToUnderworld.ProcessEffect(effectParameter.Object);

            Assert.IsFalse(roomAttrbuties.Contains(Objects.Room.Room.RoomAttribute.Light));
            Assert.AreEqual(0, roomAttrbuties.Count);
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }
    }
}