using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Notify;
using Objects.Global.Notify.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.Sound.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class MessageUnitTest
    {
        Message message;
        Mock<INotify> notify;
        Mock<IEffectParameter> effectParameter;
        RoomId roomId;
        Mock<ISound> sound;
        Mock<IRandom> random;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<ITranslationMessage> translationMessage;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<IRoom> room;
        Mock<ISerialization> serialization;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            message = new Message();
            notify = new Mock<INotify>();
            effectParameter = new Mock<IEffectParameter>();
            roomId = new RoomId();
            sound = new Mock<ISound>();
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            translationMessage = new Mock<ITranslationMessage>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            room = new Mock<IRoom>();
            random = new Mock<IRandom>();
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            serialization = new Mock<ISerialization>();
            tagWrapper = new Mock<ITagWrapper>();

            roomId.Zone = 1;
            roomId.Id = 2;
            effectParameter.Setup(e => e.RoomId).Returns(roomId);
            effectParameter.Setup(e => e.Performer).Returns(performer.Object);
            effectParameter.Setup(e => e.Target).Returns(target.Object);
            effectParameter.Setup(e => e.Message).Returns(translationMessage.Object);
            random.Setup(e => e.Next(2)).Returns(0);
            world.Setup(e => e.Zones).Returns(zones);
            zone.Setup(e => e.Rooms).Returns(rooms);
            zones.Add(1, zone.Object);
            rooms.Add(2, room.Object);
            sound.Setup(e => e.RandomSounds).Returns(new List<string>());
            serialization.Setup(e => e.Serialize(new List<ISound>() { sound.Object })).Returns("a");

            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.Serialization = serialization.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void Message_ProcessEffect_NoSound()
        {
            message.ProcessEffect(effectParameter.Object);

            notify.Verify(e => e.Room(performer.Object, target.Object, room.Object, translationMessage.Object, null, false, false), Times.Once);
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Never);
        }

        [TestMethod]
        public void Message_ProcessEffect_OneSound()
        {
            message.Sound = sound.Object;

            message.ProcessEffect(effectParameter.Object);

            notify.Verify(e => e.Room(performer.Object, target.Object, room.Object, translationMessage.Object, null, false, false), Times.Once);
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Message_ProcessEffect_TwoSounds()
        {
            message.Sound = sound.Object;
            sound.Setup(e => e.RandomSounds).Returns(new List<string>() { "1", "2" });

            message.ProcessEffect(effectParameter.Object);

            notify.Verify(e => e.Room(performer.Object, target.Object, room.Object, translationMessage.Object, null, false, false), Times.Once);
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }
    }
}
