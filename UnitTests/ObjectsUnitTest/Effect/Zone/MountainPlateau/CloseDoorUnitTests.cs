using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Effect.Zone.MountainPlateau;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Effect.Zone.MountainPlateau
{
    [TestClass]
    public class CloseDoorUnitTests
    {
        CloseDoor closeDoor;
        Mock<IEffectParameter> effectParameter;
        Mock<IBaseObjectId> chestId;
        Mock<IBaseObjectId> statueId;
        Mock<IBaseObjectId> doorId;
        Mock<IContainer> container;
        Mock<IBaseObject> baseObjectContainer;
        Mock<IItem> statue;
        Mock<IDoor> door;
        Mock<IRoom> room;
        Mock<IExit> exit;
        Mock<ISerialization> serialize;
        Mock<INotify> notify;
        Mock<ISound> sound;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            closeDoor = new CloseDoor();
            effectParameter = new Mock<IEffectParameter>();
            chestId = new Mock<IBaseObjectId>();
            statueId = new Mock<IBaseObjectId>();
            doorId = new Mock<IBaseObjectId>();
            container = new Mock<IContainer>();
            baseObjectContainer = container.As<IBaseObject>();
            statue = new Mock<IItem>();
            door = new Mock<IDoor>();
            room = new Mock<IRoom>();
            exit = new Mock<IExit>();
            serialize = new Mock<ISerialization>();
            notify = new Mock<INotify>();
            sound = new Mock<ISound>();
            tagWrapper = new Mock<ITagWrapper>();

            effectParameter.Setup(e => e.Container).Returns(container.Object);
            effectParameter.Setup(e => e.Item).Returns(statue.Object);
            effectParameter.Setup(e => e.ObjectRoom).Returns(room.Object);
            closeDoor.Chest = chestId.Object;
            closeDoor.Statue = statueId.Object;
            closeDoor.Door = doorId.Object;
            closeDoor.Sound = sound.Object;
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.South).Returns(exit.Object);
            room.Setup(e => e.West).Returns(exit.Object);
            exit.Setup(e => e.Door).Returns(door.Object);
            door.Setup(e => e.Opened).Returns(true);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            serialize.Setup(e => e.Serialize(It.IsAny<List<ISound>>())).Returns("sound");

            GlobalReference.GlobalValues.Serialization = serialize.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void CloseDoor_ProcessEffect_NoContainer()
        {
            effectParameter.Setup(e => e.Container).Returns((IContainer)null);

            closeDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = true, Times.Never);
        }

        [TestMethod]
        public void CloseDoor_ProcessEffect_NoStatue()
        {
            effectParameter.Setup(e => e.Item).Returns((IItem)null);

            closeDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = true, Times.Never);
        }

        [TestMethod]
        public void CloseDoor_ProcessEffect()
        {
            closeDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = false, Times.Exactly(4));
            serialize.Verify(e => e.Serialize(It.IsAny<List<ISound>>()), Times.Exactly(4));
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Exactly(4));
        }
    }
}
