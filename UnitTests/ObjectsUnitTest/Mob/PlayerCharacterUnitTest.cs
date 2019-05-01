using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Exp.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.MultiClassBonus.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.Sound.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class PlayerCharacterUnitTest
    {
        PlayerCharacter pc;
        Mock<ISettings> settings;
        Mock<IExperience> exp;
        Mock<IMultiClassBonus> multiClassBonus;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        List<IPlayerCharacter> pcs;
        List<IPlayerCharacter> pcs2;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Dictionary<int, IZone> zoneDictionary;
        Dictionary<int, IRoom> roomDictionary;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        Mock<IBaseObjectId> roomId;
        Mock<ICorpse> corpse;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<ISerialization> serializtion;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            pc = new PlayerCharacter();
            settings = new Mock<ISettings>();
            exp = new Mock<IExperience>();
            multiClassBonus = new Mock<IMultiClassBonus>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            pcs = new List<IPlayerCharacter>();
            pcs2 = new List<IPlayerCharacter>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            zoneDictionary = new Dictionary<int, IZone>();
            roomDictionary = new Dictionary<int, IRoom>();
            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            roomId = new Mock<IBaseObjectId>();
            corpse = new Mock<ICorpse>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            serializtion = new Mock<ISerialization>();
            stringManipulator = new Mock<IStringManipulator>();

            settings.Setup(e => e.Multiplier).Returns(1);
            multiClassBonus.Setup(e => e.CalculateBonus(1, 0)).Returns(1);
            roomId.Setup(e => e.Zone).Returns(1);
            roomId.Setup(e => e.Id).Returns(1);
            pc.Room = room.Object;
            pc.RespawnPoint = roomId.Object;
            pc.KeyWords.Add("pc");
            pcs.Add(pc);
            room.Setup(e => e.PlayerCharacters).Returns(pcs);
            room2.Setup(e => e.PlayerCharacters).Returns(pcs2);
            world.Setup(e => e.Zones).Returns(zoneDictionary);
            zone.Setup(e => e.Rooms).Returns(roomDictionary);
            zoneDictionary.Add(1, zone.Object);
            roomDictionary.Add(1, room.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            corpse.Setup(e => e.TimeOfDeath).Returns(new DateTime(2000, 1, 1));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            serializtion.Setup(e => e.Serialize(It.IsAny<List<ISound>>())).Returns("sound");
            stringManipulator.Setup(e => e.UpdateTargetPerformer("pc", null, "{performer} title")).Returns("pc title");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Experience = exp.Object;
            GlobalReference.GlobalValues.MultiClassBonus = multiClassBonus.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.Serialization = serializtion.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;
        }

        [TestMethod]
        public void PlayerCharacter_Experience_DoesNotLevel()
        {
            exp.Setup(e => e.GetExpForLevel(0)).Returns(1000);

            pc.Experience += 100;
            Assert.AreEqual(100, pc.Experience);
            serializtion.Verify(e => e.Serialize(It.IsAny<List<ISound>>()), Times.Once);
        }

        [TestMethod]
        public void PlayerCharacter_Experience_DoesLevel()
        {
            exp.Setup(e => e.GetExpForLevel(0)).Returns(100);
            settings.Setup(e => e.Multiplier).Returns(1.1d);

            pc.Experience += 100;
            Assert.AreEqual(100, pc.Experience);
            notify.Verify(e => e.Mob(null, null, pc, It.Is<ITranslationMessage>(f => f.Message == "You gain a level."), false, false), Times.Once);
            serializtion.Verify(e => e.Serialize(It.IsAny<List<ISound>>()), Times.Once);
        }

        [TestMethod]
        public void PlayerCharacter_StrengthEffective()
        {
            Assert.AreEqual(1, pc.StrengthEffective);
        }

        [TestMethod]
        public void PlayerCharacter_DexterityEffective()
        {
            Assert.AreEqual(1, pc.DexterityEffective);
        }

        [TestMethod]
        public void PlayerCharacter_ConstitutionEffective()
        {
            Assert.AreEqual(1, pc.ConstitutionEffective);
        }

        [TestMethod]
        public void PlayerCharacter_IntelligenceEffective()
        {
            Assert.AreEqual(1, pc.IntelligenceEffective);
        }

        [TestMethod]
        public void PlayerCharacter_WisdomEffective()
        {

            Assert.AreEqual(1, pc.WisdomEffective);
        }

        [TestMethod]
        public void PlayerCharacter_CharismaEffective()
        {
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
            pc.Die();
            room.Verify(e => e.AddItemToRoom(It.IsAny<IItem>(), 0));
            Assert.AreEqual(1, pc.Corpses.Count);
            evnt.Verify(e => e.OnDeath(pc), Times.Once);
            evnt.Verify(e => e.EnterRoom(pc), Times.Once);
        }

        [TestMethod]
        public void PlayerCharacter_RemoveOldCorpses()
        {
            pc.Corpses.Add(corpse.Object);

            pc.RemoveOldCorpses(new DateTime(2001, 1, 1));
            Assert.AreEqual(0, pc.Corpses.Count);
        }

        [TestMethod]
        public void PlayerCharacter_AddTitle_NotAlreadyInList()
        {
            pc.AddTitle("{performer} title");

            Assert.IsTrue(pc.AvailableTitles.Contains("pc title"));
            notify.Verify(e => e.Mob(pc, It.Is<ITranslationMessage>(f => f.Message == "New title available: pc title")), Times.Once);
        }

        [TestMethod]
        public void PlayerCharacter_AddTitle_AlreadyInList()
        {
            pc.AvailableTitles.Add("{performer} title");

            Assert.IsTrue(pc.AvailableTitles.Contains("{performer} title"));
            notify.Verify(e => e.Mob(pc, It.Is<ITranslationMessage>(f => f.Message == "pc title")), Times.Never);
        }
    }
}
