using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Random.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System.Linq;
using System.Reflection;
using static Objects.Damage.Damage;
using static Objects.Mob.MobileObject;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Mob.SpecificNPC
{
    [TestClass]
    public class HydraUnitTest
    {
        Hydra hydra;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> level1Dice;
        Mock<IDice> level5Dice;
        Mock<IDamage> damageNonFire;
        Mock<IDamage> damageFire;
        Mock<IMobileObject> attacker1;
        Mock<IMobileObject> attacker2;
        Mock<IEvent> evnt;
        Mock<IEngine> engine;
        PropertyInfo newHeadsToGrow;
        PropertyInfo tookFireDamage;
        PropertyInfo roundOfDamage;
        Mock<INotify> notify;
        Mock<ITagWrapper> tagWrapper;
        Mock<IRoom> room;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValues = new Mock<IDefaultValues>();
            level1Dice = new Mock<IDice>();
            level5Dice = new Mock<IDice>();
            damageNonFire = new Mock<IDamage>();
            damageFire = new Mock<IDamage>();
            attacker1 = new Mock<IMobileObject>();
            attacker2 = new Mock<IMobileObject>();
            evnt = new Mock<IEvent>();
            engine = new Mock<IEngine>();
            notify = new Mock<INotify>();
            tagWrapper = new Mock<ITagWrapper>();
            room = new Mock<IRoom>();
            random = new Mock<IRandom>();

            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(level1Dice.Object);
            defaultValues.Setup(e => e.DiceForWeaponLevel(5)).Returns(level5Dice.Object);
            damageNonFire.Setup(e => e.Type).Returns(DamageType.Acid);
            damageFire.Setup(e => e.Type).Returns(DamageType.Fire);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            random.Setup(e => e.Next(1)).Returns(0);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Random = random.Object;

            hydra = new Hydra(room.Object, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription", "corpseLookDescription");
            hydra.Level = 20;
            hydra.ConstitutionStat = 10; //needs to be set so when max stats are reset it will calculate correctly

            newHeadsToGrow = hydra.GetType().GetProperty("NewHeadsToGrow", BindingFlags.Instance | BindingFlags.NonPublic);
            tookFireDamage = hydra.GetType().GetProperty("TookFireDamage", BindingFlags.Instance | BindingFlags.NonPublic);
            roundOfDamage = hydra.GetType().GetProperty("RoundOfDamage", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [TestMethod]
        public void Hydra_Constructor()
        {
            Assert.AreEqual(room.Object, hydra.Room);
            Assert.AreEqual("corpseLookDescription", hydra.CorpseDescription);
            Assert.AreEqual("examineDescription", hydra.ExamineDescription);
            Assert.AreEqual("lookDescription", hydra.LookDescription);
            Assert.AreEqual("sentenceDescription", hydra.SentenceDescription);
            Assert.AreEqual("shortDescription", hydra.ShortDescription);
            Assert.AreEqual(1, hydra.Personalities.Count);
            Assert.IsTrue(hydra.Personalities[0] is Objects.Personality.Hydra);
            Assert.IsTrue(hydra.AttributesCurrent.Contains(MobileAttribute.NoDisarm));
        }

        [TestMethod]
        public void Hydra_HasHydraPersonality()
        {
            Assert.AreEqual(1, hydra.Personalities.Count);
            Assert.IsNotNull(hydra.Personalities[0] as Objects.Personality.Hydra);
        }

        [TestMethod]
        public void Hydra_NotDisarmable()
        {
            Assert.IsTrue(hydra.AttributesCurrent.Contains(Objects.Mob.MobileObject.MobileAttribute.NoDisarm));
        }

        [TestMethod]
        public void Hydra_SetLevel1()
        {
            hydra.Level = 1;

            //verify 5 heads
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            //verify min level 1 dice
            Assert.AreEqual(level1Dice.Object, hydra.EquipedWeapon.FirstOrDefault().DamageList[0].Dice);
        }

        [TestMethod]
        public void Hydra_SetLevel10()
        {
            hydra.Level = 10;

            //verify 5 heads
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            //verify level 5 dice
            Assert.AreEqual(level5Dice.Object, hydra.EquipedWeapon.FirstOrDefault().DamageList[0].Dice);
        }

        [TestMethod]
        public void Hydra_GetLevel()
        {
            Assert.AreEqual(20, hydra.Level);
        }

        [TestMethod]
        public void Hydra_TakeCombatDamage()
        {
            hydra.TakeCombatDamage(20, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeCombatDamageTwiceSamePerson()
        {
            hydra.TakeCombatDamage(20, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);

            hydra.TakeCombatDamage(20, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(40, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeCombatDamageCumilativeDamage()
        {
            hydra.TakeCombatDamage(5, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(0, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(5, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsFalse(rndOfDamage.HeadCut);

            hydra.TakeCombatDamage(5, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(10, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeCombatDamageDiffertPerson()
        {
            hydra.TakeCombatDamage(20, damageNonFire.Object, attacker1.Object, 1);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);

            hydra.TakeCombatDamage(20, damageNonFire.Object, attacker2.Object, 1);

            Assert.AreEqual(4, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(3, hydra.EquipedWeapon.Count());
            rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker2.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker2.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker2.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeCombatDamageFireDamage()
        {
            hydra.TakeCombatDamage(20, damageFire.Object, attacker1.Object, 1);

            Assert.AreEqual(0, (int)newHeadsToGrow.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            Assert.IsTrue((bool)tookFireDamage.GetValue(hydra));
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(1u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeDamageTwoTimesNotEnoughDamageToGrowAHead()
        {
            hydra.TakeDamage(5, damageNonFire.Object, attacker1.Object);

            Assert.AreEqual(0, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(5, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(0u, rndOfDamage.CombatRound);
            Assert.IsFalse(rndOfDamage.HeadCut);


            hydra.TakeDamage(5, damageNonFire.Object, attacker2.Object);

            Assert.AreEqual(0, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage2 = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(5, rndOfDamage2.TotalDamage);
            Assert.AreEqual(attacker2.Object, rndOfDamage2.LastAttacker);
            Assert.AreEqual(0u, rndOfDamage2.CombatRound);
            Assert.IsFalse(rndOfDamage2.HeadCut);
            Assert.AreNotSame(rndOfDamage, rndOfDamage2);
        }

        [TestMethod]
        public void Hydra_TakeDamageTwoTimesEnoughDamageToGrowAHead()
        {
            hydra.TakeDamage(20, damageNonFire.Object, attacker1.Object);

            Assert.AreEqual(2, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(0u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);


            hydra.TakeDamage(20, damageNonFire.Object, attacker2.Object);

            Assert.AreEqual(4, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(3, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage2 = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage2.TotalDamage);
            Assert.AreEqual(attacker2.Object, rndOfDamage2.LastAttacker);
            Assert.AreEqual(0u, rndOfDamage2.CombatRound);
            Assert.IsTrue(rndOfDamage2.HeadCut);
            Assert.AreNotSame(rndOfDamage, rndOfDamage2);
            notify.Verify(e => e.Mob(attacker2.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker2.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_TakeDamageFireDamage()
        {
            hydra.TakeDamage(20, damageFire.Object, attacker1.Object);

            Assert.AreEqual(0, (int)newHeadsToGrow.GetValue(hydra));
            Assert.IsTrue((bool)tookFireDamage.GetValue(hydra));
            Assert.AreEqual(4, hydra.EquipedWeapon.Count());
            RoundOfDamage rndOfDamage = (RoundOfDamage)roundOfDamage.GetValue(hydra);
            Assert.AreEqual(20, rndOfDamage.TotalDamage);
            Assert.AreEqual(attacker1.Object, rndOfDamage.LastAttacker);
            Assert.AreEqual(0u, rndOfDamage.CombatRound);
            Assert.IsTrue(rndOfDamage.HeadCut);
            notify.Verify(e => e.Mob(attacker1.Object, It.Is<ITranslationMessage>(f => f.Message == "You cut off on of the hydras heads.")), Times.Once);
            notify.Verify(e => e.Room(attacker1.Object, hydra, room.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} cut off on of the hydras heads."), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_RegrowHeads()
        {
            newHeadsToGrow.SetValue(hydra, 4);
            hydra.RegrowHeads();

            Assert.AreEqual(9, hydra.EquipedWeapon.Count());
            Assert.AreEqual(0, newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            notify.Verify(e => e.Room(hydra, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Once);
        }

        [TestMethod]
        public void Hydra_RegrowHeadsAfterFire()
        {
            tookFireDamage.SetValue(hydra, true);
            newHeadsToGrow.SetValue(hydra, 4);
            hydra.RegrowHeads();

            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            Assert.AreEqual(0, newHeadsToGrow.GetValue(hydra));
            Assert.IsFalse((bool)tookFireDamage.GetValue(hydra));
            notify.Verify(e => e.Room(hydra, null, room.Object, It.IsAny<ITranslationMessage>(), null, false, false), Times.Never);
        }
    }
}
