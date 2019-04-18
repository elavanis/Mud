using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Interface;
using Objects.Language.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class SayUnitTest
    {
        Message say;
        Mock<IEffectParameter> effectParameter;
        Mock<IRoom> room;
        Mock<IZone> zone;
        Mock<IWorld> world;
        Mock<IBaseObjectId> baseObjectId;
        Mock<INotify> notify;
        Mock<ITranslationMessage> message;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            say = new Message();
            effectParameter = new Mock<IEffectParameter>();
            room = new Mock<IRoom>();
            zone = new Mock<IZone>();
            world = new Mock<IWorld>();
            baseObjectId = new Mock<IBaseObjectId>();
            notify = new Mock<INotify>();
            message = new Mock<ITranslationMessage>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();

            effectParameter.Setup(e => e.RoomId).Returns(baseObjectId.Object);
            effectParameter.Setup(e => e.RoomMessage).Returns(message.Object);
            room.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.Id).Returns(2);
            zone.Setup(e => e.Rooms).Returns(rooms);
            world.Setup(e => e.Zones).Returns(zones);
            baseObjectId.Setup(e => e.Zone).Returns(1);
            baseObjectId.Setup(e => e.Id).Returns(2);

            rooms.Add(2, room.Object);
            zones.Add(1, zone.Object);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void Say_ProcessEffect()
        {
            say.ProcessEffect(effectParameter.Object);

            notify.Verify(e => e.Room(null, null, room.Object, message.Object, null, false, false), Times.Once);

        }
    }
}
