using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Shared.TagWrapper.Interface;
using Objects.Effect;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Shared.Sound.Interface;
using Objects.Global.Serialization.Interface;
using System.Collections.Generic;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Global.Logging.Interface;
using Objects.Interface;
using Objects;
using static Objects.Global.Logging.LogSettings;
using Objects.World.Interface;
using Objects.Zone.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class MoveMobUnitTest
    {
        MoveMob effect;
        Mock<IPlayerCharacter> pc;
        Mock<IEffectParameter> parameter;
        Mock<ISound> sound;
        Mock<IRoom> oldRoom;
        Mock<IRoom> newRoom;
        Mock<ILogger> logger;

        [TestInitialize]
        public void Setup()
        {
            sound = new Mock<ISound>();
            effect = new MoveMob(sound.Object);
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            oldRoom = new Mock<IRoom>();
            newRoom = new Mock<IRoom>();
            logger = new Mock<ILogger>();
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();

            pc.Setup(e => e.Room).Returns(oldRoom.Object);
            parameter.Setup(e => e.RoomId).Returns(roomId.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
            roomId.Setup(e => e.Zone).Returns(1);
            roomId.Setup(e => e.Id).Returns(2);

            GlobalReference.GlobalValues.Logger = logger.Object;
        }

        [TestMethod]
        public void MoveMob_Constructor()
        {
            Assert.AreSame(sound.Object, effect.Sound);
        }

        [TestMethod]
        public void MoveMob_ProcessEffect_UnableToFindRoom()
        {
            effect.ProcessEffect(parameter.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.ERROR, "Mob tried to move to Zone 1 Room 2 but failed."));
        }

        [TestMethod]
        public void MoveMob_ProcessEffect_MovedToNewRoom()
        {
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> dictionaryZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dictionaryRoom = new Dictionary<int, IRoom>();

            world.Setup(e => e.Zones).Returns(dictionaryZone);
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);
            dictionaryZone.Add(1, zone.Object);
            dictionaryRoom.Add(1, oldRoom.Object);
            dictionaryRoom.Add(2, newRoom.Object);

            GlobalReference.GlobalValues.World = world.Object;

            effect.ProcessEffect(parameter.Object);

            oldRoom.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            newRoom.Verify(e => e.AddMobileObjectToRoom(pc.Object));
        }
    }
}
