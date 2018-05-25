using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Interface;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class TellUnitTest
    {
        Objects.Effect.Tell tell;
        Mock<IPlayerCharacter> pc;
        Mock<INonPlayerCharacter> npc;
        Mock<IEffectParameter> parameter;
        Mock<ITranslationMessage> translationMessage;

        [TestInitialize]
        public void Setup()
        {
            tell = new Objects.Effect.Tell();
            pc = new Mock<IPlayerCharacter>();
            npc = new Mock<INonPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            translationMessage = new Mock<ITranslationMessage>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IBaseObjectId> objectId = new Mock<IBaseObjectId>();
            Dictionary<int, IZone> zones = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            List<INonPlayerCharacter> listNpc = new List<INonPlayerCharacter>();
            List<string> keywords = new List<string>();

            keywords.Add("pc");
            listNpc.Add(npc.Object);
            objectId.Setup(e => e.Zone).Returns(1);
            objectId.Setup(e => e.Zone).Returns(1);
            parameter.Setup(e => e.Message).Returns(translationMessage.Object);
            parameter.Setup(e => e.ObjectId).Returns(objectId.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
            translationMessage.Setup(e => e.GetTranslatedMessage(null)).Returns("test message");
            room.Setup(e => e.Id).Returns(1);
            room.Setup(e => e.NonPlayerCharacters).Returns(listNpc);
            rooms.Add(1, room.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            zones.Add(1, zone.Object);
            world.Setup(e => e.Zones).Returns(zones);
            pc.Setup(e => e.KeyWords).Returns(keywords);

            GlobalReference.GlobalValues.World = world.Object;
        }

        [TestMethod]
        public void Tell_ProcessEffect()
        {
            tell.ProcessEffect(parameter.Object);

            npc.Verify(e => e.EnqueueCommand("Tell pc test message"), Times.Once);
        }
    }
}
