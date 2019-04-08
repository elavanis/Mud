using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Exp.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.MultiClassBonus.Interface;
using Objects.Global.Settings.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class PlayerCharacterUnitTest
    {
        PlayerCharacter pc;
        Mock<ISettings> settings;

        [TestInitialize]
        public void Setup()
        {
            pc = new PlayerCharacter();
            settings = new Mock<ISettings>();

            settings.Setup(e => e.Multiplier).Returns(1);

            GlobalReference.GlobalValues.Settings = settings.Object;
        }

        [TestMethod]
        public void PlayerCharacter_Experience_DoesNotLevel()
        {
            Mock<IExperience> exp = new Mock<IExperience>();
            exp.Setup(e => e.GetExpForLevel(0)).Returns(1000);
            GlobalReference.GlobalValues.Experience = exp.Object;

            pc.Experience += 100;
            Assert.AreEqual(100, pc.Experience);
        }

        [TestMethod]
        public void PlayerCharacter_Experience_DoesLevel()
        {
            Mock<IExperience> exp = new Mock<IExperience>();
            exp.Setup(e => e.GetExpForLevel(0)).Returns(100);
            GlobalReference.GlobalValues.Experience = exp.Object;
            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.Multiplier).Returns(1.1d);
            GlobalReference.GlobalValues.Settings = settings.Object;

            pc.Experience += 100;
            Assert.AreEqual(100, pc.Experience);
        }

        [TestMethod]
        public void PlayerCharacter_StrengthEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.StrengthEffective);
        }

        [TestMethod]
        public void PlayerCharacter_DexterityEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.DexterityEffective);
        }

        [TestMethod]
        public void PlayerCharacter_ConstitutionEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.ConstitutionEffective);
        }

        [TestMethod]
        public void PlayerCharacter_IntelligenceEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.IntelligenceEffective);
        }

        [TestMethod]
        public void PlayerCharacter_WisdomEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.WisdomEffective);
        }

        [TestMethod]
        public void PlayerCharacter_CharismaEffective()
        {
            Mock<IMultiClassBonus> multiClassBonus = new Mock<IMultiClassBonus>();
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;

            Assert.AreEqual(1, pc.CharismaEffective);
        }

        [TestMethod]
        public void PlayerCharacter_Corpses()
        {
            Assert.AreEqual(0, pc.Corpses.Count);
        }

        [TestMethod]
        public void PlayerCharacter_Die()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<IRoom> room2 = new Mock<IRoom>();
            List<IPlayerCharacter> pcs = new List<IPlayerCharacter>();
            List<IPlayerCharacter> pcs2 = new List<IPlayerCharacter>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone = new Mock<IZone>();
            Dictionary<int, IZone> zoneDictionary = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> roomDictionary = new Dictionary<int, IRoom>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IBaseObjectId> roomId = new Mock<IBaseObjectId>();

            roomId.Setup(e => e.Zone).Returns(1);
            roomId.Setup(e => e.Id).Returns(1);
            pc.Room = room.Object;
            pc.RespawnPoint = roomId.Object;
            pcs.Add(pc);
            room.Setup(e => e.PlayerCharacters).Returns(pcs);
            room2.Setup(e => e.PlayerCharacters).Returns(pcs2);
            world.Setup(e => e.Zones).Returns(zoneDictionary);
            zone.Setup(e => e.Rooms).Returns(roomDictionary);
            zoneDictionary.Add(1, zone.Object);
            roomDictionary.Add(1, room.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            pc.Die();
            room.Verify(e => e.AddItemToRoom(It.IsAny<IItem>(), 0));
            Assert.AreEqual(1, pc.Corpses.Count);
            evnt.Verify(e => e.OnDeath(pc), Times.Once);
            evnt.Verify(e => e.EnterRoom(pc), Times.Once);
        }

        [TestMethod]
        public void PlayerCharacter_RemoveOldCorpses()
        {
            Mock<ICorpse> corpse = new Mock<ICorpse>();
            corpse.Setup(e => e.TimeOfDeath).Returns(new DateTime(2000, 1, 1));
            pc.Corpses.Add(corpse.Object);

            pc.RemoveOldCorpses(new DateTime(2001, 1, 1));
            Assert.AreEqual(0, pc.Corpses.Count);
        }
    }
}
