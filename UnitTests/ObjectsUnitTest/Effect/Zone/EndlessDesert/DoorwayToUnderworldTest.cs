using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Effect.Zone.EndlessDesert;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
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
            GlobalReference.GlobalValues = new GlobalValues();

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
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void DoorwayToUnderworldTest_ProcessEffect_AddDoor()
        {
            string expectedMessage = "As the last rays of light disappear over the horizon the crackle of water quickly freezing can be heard originating from a shimmering gray portal that has appeared in the center of the now frozen lake.  The portal gives off an eerie gray light causing everything to look pale as if touched by dead while a cold breeze can be felt emanating from it adding to the effect.";

            doorwayToUnderworld.ProcessEffect(effectParameter.Object);

            Assert.IsTrue(roomAttrbuties.Contains(RoomAttribute.Light));
            Assert.AreEqual(1, roomAttrbuties.Count);
            notify.Verify(e => e.Room(null, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == expectedMessage), null, false, false), Times.Once);
        }

        [TestMethod]
        public void DoorwayToUnderworldTest_ProcessEffect_RoomDoor()
        {
            string expectedMessage = "As the sun starts to peek over the dunes the portal in the center of the lake disappears and the lake begins to thaw.";
            gameDateTime.Setup(e => e.Hour).Returns(1);
            Mock<IExit> exit = new Mock<IExit>();
            room.Setup(e => e.Down).Returns(exit.Object);
            roomAttrbuties.Add(RoomAttribute.Light);

            doorwayToUnderworld.ProcessEffect(effectParameter.Object);

            Assert.IsFalse(roomAttrbuties.Contains(RoomAttribute.Light));
            Assert.AreEqual(0, roomAttrbuties.Count);
            notify.Verify(e => e.Room(null, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == expectedMessage), null, false, false), Times.Once);
        }
    }
}