using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect.Interface;
using Objects.Effect.Zone.MountainPlateau;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

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

        [TestInitialize]
        public void Setup()
        {
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
        }
    }
}
