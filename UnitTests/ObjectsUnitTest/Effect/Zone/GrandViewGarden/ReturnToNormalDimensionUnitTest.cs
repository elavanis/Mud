using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Effect.Interface;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Zone.GrandViewGarden;
using Objects.Room.Interface;
using Objects.Global;
using Objects.World.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global.StringManuplation.Interface;

namespace ObjectsUnitTest.Effect.Zone.GrandViewGarden
{
    [TestClass]
    public class ReturnToNormalDimensionUnitTest
    {
        IEffectParameter param;
        List<INonPlayerCharacter> lNpc;
        List<IPlayerCharacter> lPc;
        List<INonPlayerCharacter> lNpc2;
        List<IPlayerCharacter> lPc2;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<INonPlayerCharacter> npc2;
        Mock<IPlayerCharacter> pc2;
        Mock<INonPlayerCharacter> performerNpc;
        Mock<IPlayerCharacter> performerPc;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<INotify> notify;
        Mock<IEffectParameter> effect = new Mock<IEffectParameter>();
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<ITagWrapper> tagWrapper;
        Mock<IStringManipulator> stringManipulator;

        ReturnToNormalDimension returnToNormalDimension;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            pc2 = new Mock<IPlayerCharacter>();
            performerNpc = new Mock<INonPlayerCharacter>();
            performerPc = new Mock<IPlayerCharacter>();
            notify = new Mock<INotify>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            tagWrapper = new Mock<ITagWrapper>();
            stringManipulator = new Mock<IStringManipulator>();

            Dictionary<int, IZone> zoneDict = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> roomDict = new Dictionary<int, IRoom>();

            lNpc = new List<INonPlayerCharacter>();
            lPc = new List<IPlayerCharacter>();
            lNpc2 = new List<INonPlayerCharacter>();
            lPc2 = new List<IPlayerCharacter>();

            zoneDict.Add(11, zone.Object);
            roomDict.Add(1, room.Object);
            roomDict.Add(11, room2.Object);

            room.Setup(e => e.ZoneId).Returns(11);
            room.Setup(e => e.Id).Returns(1);
            room.Setup(e => e.NonPlayerCharacters).Returns(lNpc);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room2.Setup(e => e.ZoneId).Returns(11);
            room2.Setup(e => e.Id).Returns(11);
            room2.Setup(e => e.NonPlayerCharacters).Returns(lNpc2);
            room2.Setup(e => e.PlayerCharacters).Returns(lPc2);
            performerNpc.Setup(e => e.Room).Returns(room2.Object);
            performerPc.Setup(e => e.Room).Returns(room2.Object);
            world.Setup(e => e.Zones).Returns(zoneDict);
            zone.Setup(e => e.Rooms).Returns(roomDict);
            performerNpc.Setup(e => e.SentenceDescription).Returns("npc");
            performerPc.Setup(e => e.SentenceDescription).Returns("pc");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("npc")).Returns("Npc");
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("pc")).Returns("Pc");

            param = effect.Object;

            lNpc.Add(npc.Object);
            lPc.Add(pc.Object);
            lNpc2.Add(npc2.Object);
            lPc2.Add(pc2.Object);
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            returnToNormalDimension = new ReturnToNormalDimension();
        }

        [TestMethod]
        public void ReturnToNormalDimension_ProcessEffect_Npc()
        {
            effect.Setup(e => e.Target).Returns(performerNpc.Object);
            lNpc2.Add(performerNpc.Object);

            returnToNormalDimension.ProcessEffect(param);

            room2.Verify(e => e.RemoveMobileObjectFromRoom(performerNpc.Object));
            room.Verify(e => e.AddMobileObjectToRoom(performerNpc.Object));
            notify.Verify(e => e.Room(null, null, room2.Object, It.Is<ITranslationMessage>(f => f.Message == "Npc drops a rose and then disappears."), new List<IMobileObject>() { performerNpc.Object }, true, false), Times.Once);
            notify.Verify(e => e.Room(null, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "Npc suddenly appears from thin air."), new List<IMobileObject>() { performerNpc.Object }, true, false), Times.Once);
        }

        [TestMethod]
        public void ReturnToNormalDimension_ProcessEffect_Pc()
        {
            effect.Setup(e => e.Target).Returns(performerPc.Object);
            lPc2.Add(performerPc.Object);

            returnToNormalDimension.ProcessEffect(param);

            room2.Verify(e => e.RemoveMobileObjectFromRoom(performerPc.Object));
            room.Verify(e => e.AddMobileObjectToRoom(performerPc.Object));
            notify.Verify(e => e.Room(null, null, room2.Object, It.Is<ITranslationMessage>(f => f.Message == "Pc drops a rose and then disappears."), new List<IMobileObject>() { performerPc.Object }, true, false), Times.Once);
            notify.Verify(e => e.Room(null, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "Pc suddenly appears from thin air."), new List<IMobileObject>() { performerPc.Object }, true, false), Times.Once);
        }
    }
}
