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

namespace ObjectsUnitTest.Effect.Zone.GrandViewGarden
{
    [TestClass]
    public class MoveToOtherDimensionUnitTest
    {
        IEffectParameter param;
        List<INonPlayerCharacter> lNpc;
        List<IPlayerCharacter> lPc;
        List<INonPlayerCharacter> lNpc2;
        List<IPlayerCharacter> lPc2;
        INonPlayerCharacter npc;
        IPlayerCharacter pc;
        INonPlayerCharacter npc2;
        IPlayerCharacter pc2;
        INonPlayerCharacter performerNpc;
        IPlayerCharacter performerPc;
        Mock<INonPlayerCharacter> mockNpc;
        Mock<IPlayerCharacter> mockPc;
        Mock<INonPlayerCharacter> mockNpc2;
        Mock<IPlayerCharacter> mockPc2;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<INotify> notify;
        Mock<IEffectParameter> effect;
        Mock<INonPlayerCharacter> mockPerformerNpc;
        Mock<IPlayerCharacter> mockPerformerPc;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<ITagWrapper> tagWrapper;

        MoveToOtherDimension moveToOtherDimension;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            mockNpc = new Mock<INonPlayerCharacter>();
            mockPc = new Mock<IPlayerCharacter>();
            mockNpc2 = new Mock<INonPlayerCharacter>();
            mockPc2 = new Mock<IPlayerCharacter>();
            notify = new Mock<INotify>();
            effect = new Mock<IEffectParameter>();
            mockPerformerNpc = new Mock<INonPlayerCharacter>();
            mockPerformerPc = new Mock<IPlayerCharacter>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            tagWrapper = new Mock<ITagWrapper>();


            Dictionary<int, IZone> zoneDict = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> roomDict = new Dictionary<int, IRoom>();

            lNpc = new List<INonPlayerCharacter>();
            lPc = new List<IPlayerCharacter>();
            lNpc2 = new List<INonPlayerCharacter>();
            lPc2 = new List<IPlayerCharacter>();

            zoneDict.Add(11, zone.Object);
            roomDict.Add(1, room.Object);
            roomDict.Add(11, room2.Object);

            room.Setup(e => e.Zone).Returns(11);
            room.Setup(e => e.Id).Returns(1);
            room.Setup(e => e.NonPlayerCharacters).Returns(lNpc);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room2.Setup(e => e.Zone).Returns(11);
            room2.Setup(e => e.Id).Returns(11);
            room2.Setup(e => e.NonPlayerCharacters).Returns(lNpc2);
            room2.Setup(e => e.PlayerCharacters).Returns(lPc2);
            mockPerformerNpc.Setup(e => e.Room).Returns(room.Object);
            mockPerformerPc.Setup(e => e.Room).Returns(room.Object);
            world.Setup(e => e.Zones).Returns(zoneDict);
            zone.Setup(e => e.Rooms).Returns(roomDict);
            mockPerformerNpc.Setup(e => e.SentenceDescription).Returns("npc");
            mockPerformerPc.Setup(e => e.SentenceDescription).Returns("pc");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            performerNpc = mockPerformerNpc.Object;
            performerPc = mockPerformerPc.Object;
            param = effect.Object;
            npc = mockNpc.Object;
            pc = mockPc.Object;
            npc2 = mockNpc2.Object;
            pc2 = mockPc2.Object;

            lNpc.Add(npc);
            lPc.Add(pc);
            lNpc2.Add(npc2);
            lPc2.Add(pc2);

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;


            moveToOtherDimension = new MoveToOtherDimension();
        }

        [TestMethod]
        public void MoveToOtherDimension_ProcessEffect_Npc()
        {
            effect.Setup(e => e.Target).Returns(performerNpc);
            lNpc.Add(performerNpc);

            moveToOtherDimension.ProcessEffect(param);

            room.Verify(e => e.RemoveMobileObjectFromRoom(performerNpc));
            room2.Verify(e => e.AddMobileObjectToRoom(performerNpc));
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), new List<IMobileObject>() { performerNpc }, true, false), Times.Once);
        }

        [TestMethod]
        public void MoveToOtherDimension_ProcessEffect_Pc()
        {
            effect.Setup(e => e.Target).Returns(performerPc);
            lPc.Add(performerPc);

            moveToOtherDimension.ProcessEffect(param);

            room.Verify(e => e.RemoveMobileObjectFromRoom(performerPc));
            room2.Verify(e => e.AddMobileObjectToRoom(performerPc));
            notify.Verify(e => e.Room(null, null, room.Object, It.IsAny<ITranslationMessage>(), new List<IMobileObject>() { performerPc }, true, false), Times.Once);
        }
    }
}
