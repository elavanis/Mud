using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.GameDateTime.Interface;
using Moq;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using Objects.Item.Items.Interface;
using Objects.Global.Random.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Interface;
using Objects.GameDateTime.Interface;

namespace ObjectsUnitTest.Zone
{
    [TestClass]
    public class ZoneUnitTest
    {
        Objects.Zone.Zone zone;
        Mock<IRoom> room;
        Mock<IItem> item;
        Mock<INonPlayerCharacter> npc;
        Mock<IMobileObject> otherMob;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            zone = new Objects.Zone.Zone();
            room = new Mock<IRoom>();
            item = new Mock<IItem>();
            npc = new Mock<INonPlayerCharacter>();
            otherMob = new Mock<IMobileObject>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();

            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.OtherMobs).Returns(new List<IMobileObject>() { otherMob.Object });
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            gameDateTime.Setup(e => e.AddDays(1)).Returns(gameDateTime.Object);

            zone.Rooms.Add(1, room.Object);
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
        }


        [TestMethod]
        public void Zone_FinishLoad_NoZoneSync()
        {
            zone.FinishLoad();

            Assert.IsNotNull(zone.ResetTime);
            npc.Verify(e => e.FinishLoad(-1), Times.Once);
            otherMob.Verify(e => e.FinishLoad(-1), Times.Once);
            item.Verify(e => e.FinishLoad(-1), Times.Once);
            room.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void Zone_FinishLoad_RecursiveLoad()
        {
            Mock<IItem> item1 = new Mock<IItem>();
            Mock<IItem> item2 = new Mock<IItem>();
            Mock<IItem> item3 = new Mock<IItem>();
            Mock<IContainer> container1 = item1.As<IContainer>();
            Mock<IContainer> container2 = item2.As<IContainer>();
            Mock<IContainer> container3 = item3.As<IContainer>();
            Mock<IGameDateTime> gameDateTime = new Mock<IGameDateTime>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            container3.Setup(e => e.Items).Returns(new List<IItem>());
            container2.Setup(e => e.Items).Returns(new List<IItem>() { item3.Object });
            container1.Setup(e => e.Items).Returns(new List<IItem>() { item2.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { item1.Object });

            zone.FinishLoad();

            item1.Verify(e => e.FinishLoad(-1), Times.Once);
            item2.Verify(e => e.FinishLoad(-1), Times.Once);
            item3.Verify(e => e.FinishLoad(-1), Times.Once);
            item1.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
            item2.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
            item3.Verify(e => e.ZoneObjectSyncLoad(-1), Times.Once);
        }

        [TestMethod]
        public void Zone_FinishLoad_ZoneSyncOption()
        {
            Mock<IGameDateTime> gameDateTime = new Mock<IGameDateTime>();
            Mock<IRandom> random = new Mock<IRandom>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            zone.ZoneObjectSyncOptions = 1;
            random.Setup(e => e.Next(1)).Returns(0);

            GlobalReference.GlobalValues.Random = random.Object;

            zone.FinishLoad(0);

            room.Verify(e => e.FinishLoad(0), Times.Once);
        }

        [TestMethod]
        public void Zone_RecursivelySetZone_ZoneSyncOption()
        {
            Mock<IItem> npcItem = new Mock<IItem>();
            Mock<IItem> npcItem2 = new Mock<IItem>();
            Mock<IItem> npcItem3 = new Mock<IItem>();
            Mock<IContainer> container1 = npcItem.As<IContainer>();
            Mock<IContainer> container2 = npcItem2.As<IContainer>();
            Mock<IContainer> container3 = npcItem3.As<IContainer>();
            Mock<IMerchant> merchant = new Mock<IMerchant>();
            Mock<IEquipment> equippedItem = new Mock<IEquipment>();
            Mock<IItem> sellItem = new Mock<IItem>();

            zone.Id = 1;
            npc.Setup(e => e.Items).Returns(new List<IItem>() { npcItem.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { merchant.Object });
            npc.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equippedItem.Object });
            merchant.Setup(e => e.Sellables).Returns(new List<IItem>() { sellItem.Object });
            container1.Setup(e => e.Items).Returns(new List<IItem>() { npcItem2.Object });
            container2.Setup(e => e.Items).Returns(new List<IItem>() { npcItem3.Object });
            container3.Setup(e => e.Items).Returns(new List<IItem>());


            zone.RecursivelySetZone();

            room.VerifySet(e => e.Zone = 1, Times.Once);
            item.VerifySet(e => e.Zone = 1, Times.Once);
            equippedItem.VerifySet(e => e.Zone = 1, Times.Exactly(2));
            sellItem.VerifySet(e => e.Zone = 1, Times.Once);
            npcItem.VerifySet(e => e.Zone = 1, Times.Exactly(2));
            npcItem2.VerifySet(e => e.Zone = 1, Times.Exactly(2));
            npcItem3.VerifySet(e => e.Zone = 1, Times.Exactly(2));
        }
    }
}
