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
    public class OpenDoorUnitTest
    {
        OpenDoor openDoor;
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
        Mock<ISound> sound;
        Mock<ISerialization> serialization;
        Mock<INotify> notify;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            openDoor = new OpenDoor();
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
            sound = new Mock<ISound>();
            serialization = new Mock<ISerialization>();
            notify = new Mock<INotify>();
            tagWrapper = new Mock<ITagWrapper>();

            openDoor.Sound = sound.Object;
            effectParameter.Setup(e => e.Container).Returns(container.Object);
            effectParameter.Setup(e => e.Item).Returns(statue.Object);
            effectParameter.Setup(e => e.ObjectRoom).Returns(room.Object);
            openDoor.Chest = chestId.Object;
            openDoor.Statue = statueId.Object;
            openDoor.Door = doorId.Object;
            room.Setup(e => e.North).Returns(exit.Object);
            room.Setup(e => e.East).Returns(exit.Object);
            room.Setup(e => e.South).Returns(exit.Object);
            room.Setup(e => e.West).Returns(exit.Object);
            exit.Setup(e => e.Door).Returns(door.Object);
            serialization.Setup(e => e.Serialize(new List<ISound>() { sound.Object })).Returns("serializedSound");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Sound)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.Serialization = serialization.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }


        [TestMethod]
        public void OpenDoor_ProcessEffect_NoContainer()
        {
            effectParameter.Setup(e => e.Container).Returns((IContainer)null);

            openDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = true, Times.Never);
        }

        [TestMethod]
        public void OpenDoor_ProcessEffect_NoStatue()
        {
            effectParameter.Setup(e => e.Item).Returns((IItem)null);

            openDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = true, Times.Never);
        }

        [TestMethod]
        public void OpenDoor_ProcessEffect()
        {
            openDoor.ProcessEffect(effectParameter.Object);

            door.VerifySet(e => e.Opened = true, Times.Exactly(4));
            notify.Verify(e => e.Room(null, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "serializedSound"), null, false, false), Times.Exactly(4));
        }
    }
}
