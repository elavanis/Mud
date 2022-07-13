using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Logging.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using Objects.Global.Stats;
using Objects.Global.ValidateAsset.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language.Interface;
using Objects.LevelRange.Interface;
using Objects.Magic.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Race.Interface;
using Objects.Race.Races;
using Objects.Room.Interface;
using Objects.Skill.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using Shared.TelnetItems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Objects.Damage.Damage;
using static Objects.Global.Logging.LogSettings;
using static Objects.Global.Stats.Stats;
using static Objects.Mob.MobileObject;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class MobileObjectUnitTest
    {
        UnitTestMobileObject mob;

        Mock<ITagWrapper> tagWrapper;
        Mock<IEvent> evnt;
        Mock<IEngine> engine;
        Mock<IItem> item;
        Mock<IMobileObject> mob2;
        Mock<ICombat> combat;
        Mock<ISettings> settings;
        Mock<ILogger> logger;
        Mock<IValidateAsset> validateAsset;
        Mock<ISerialization> serilization;
        Mock<IFileIO> fileIO;
        Mock<IPlayerCharacter> pc;
        Mock<IArmor> armor;
        Mock<IDice> dice;
        Mock<IRace> race;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<IEnchantment> enchantment;
        Mock<IRandomDropGenerator> randomDropGenerator;
        Mock<IRandom> random;
        Mock<IDamage> damage;
        Mock<IShield> shield;
        Mock<IWeapon> weapon;
        Mock<IEquipment> equipment;
        Mock<IParty> party;
        Mock<INotify> notify;
        Mock<IMobileObject> attacker;
        Mock<IMount> mount;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            evnt = new Mock<IEvent>();
            engine = new Mock<IEngine>();
            item = new Mock<IItem>();
            mob2 = new Mock<IMobileObject>();
            combat = new Mock<ICombat>();
            settings = new Mock<ISettings>();
            logger = new Mock<ILogger>();
            validateAsset = new Mock<IValidateAsset>();
            serilization = new Mock<ISerialization>();
            fileIO = new Mock<IFileIO>();
            pc = new Mock<IPlayerCharacter>();
            armor = new Mock<IArmor>();
            dice = new Mock<IDice>();
            race = new Mock<IRace>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            enchantment = new Mock<IEnchantment>();
            randomDropGenerator = new Mock<IRandomDropGenerator>();
            random = new Mock<IRandom>();
            damage = new Mock<IDamage>();
            shield = new Mock<IShield>();
            weapon = new Mock<IWeapon>();
            equipment = new Mock<IEquipment>();
            party = new Mock<IParty>();
            notify = new Mock<INotify>();
            attacker = new Mock<IMobileObject>();
            mount = new Mock<IMount>();
            room = new Mock<IRoom>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Health)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Mana)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Stamina)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Data)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.FileValidation)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.MountStamina)).Returns((string x, TagType y) => (x));
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), It.IsAny<string>())).Returns((IMobileObject x, string y) => (y));
            engine.Setup(e => e.Event).Returns(evnt.Object);
            engine.Setup(e => e.Combat).Returns(combat.Object);
            engine.Setup(e => e.Party).Returns(party.Object);
            settings.Setup(e => e.AssetsDirectory).Returns(@"c:\");
            settings.Setup(e => e.Multiplier).Returns(1.1d);
            validateAsset.Setup(e => e.GetCheckSum("validateAsset")).Returns("abc");
            serilization.Setup(e => e.Serialize(It.IsAny<Data>())).Returns("serialization");
            fileIO.Setup(e => e.Exists("c:\\test")).Returns(true);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "pc" });
            armor.Setup(e => e.AttributesForMobileObjectsWhenEquiped).Returns(new List<MobileAttribute>() { MobileAttribute.Fly });
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            dice.Setup(e => e.RollDice()).Returns(1);
            race.Setup(e => e.RaceAttributes).Returns(new List<MobileAttribute>() { MobileAttribute.Fly });
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("10 coins");
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0 coin");
            randomDropGenerator.Setup(e => e.GenerateRandomDrop(It.IsAny<INonPlayerCharacter>())).Returns(item.Object);
            random.Setup(e => e.Next(It.IsAny<int>())).Returns((int x) => (x));
            random.Setup(e => e.Next(1)).Returns(0);
            random.Setup(e => e.Next(2)).Returns(1);
            random.Setup(e => e.Next(101)).Returns(0);
            damage.Setup(e => e.Dice).Returns(dice.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Slash);
            shield.Setup(e => e.Dice).Returns(dice.Object);
            shield.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            shield.Setup(e => e.NegateDamagePercent).Returns(100);
            mount.Setup(e => e.Stamina).Returns(4);
            mount.Setup(e => e.MaxStamina).Returns(40);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.ValidateAsset = validateAsset.Object;
            GlobalReference.GlobalValues.Serialization = serilization.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            mob = new UnitTestMobileObject(room.Object, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription", "corpseLookDescription");
            mob.Items.Add(item.Object);
        }

        [TestMethod]
        public void MobileObject_Constructor()
        {
            Assert.AreEqual(room.Object, mob.Room);
            Assert.AreEqual("corpseLookDescription", mob.CorpseDescription);
            Assert.AreEqual("examineDescription", mob.ExamineDescription);
            Assert.AreEqual("lookDescription", mob.LookDescription);
            Assert.AreEqual("sentenceDescription", mob.SentenceDescription);
            Assert.AreEqual("shortDescription", mob.ShortDescription);
        }

        [TestMethod]
        public void MobileObject_IsAlive()
        {
            Assert.IsTrue(mob.IsAlive);
        }

        [TestMethod]
        public void MobileObject_BaseRace()
        {
            Assert.AreEqual(typeof(Human), mob.Race.GetType());
        }

        [TestMethod]
        public void MobileObject_Items_Blank()
        {
            mob.Items.Clear();

            Assert.IsNotNull(mob.Items);
            Assert.AreEqual(0, mob.Items.Count);
        }

        [TestMethod]
        public void MobileObject_Items_Populated()
        {
            Assert.AreEqual(1, mob.Items.Count);
            Assert.AreSame(item.Object, mob.Items[0]);
        }

        [TestMethod]
        public void MobileObject_EquipedEquipment_Blank()
        {
            Assert.IsNotNull(mob.EquipedEquipment);
            Assert.AreEqual(0, mob.EquipedEquipment.Count());
        }

        [TestMethod]
        public void MobileObject_EquipedEquipment_Populated()
        {
            EquipedEquipment(mob).Add(equipment.Object);

            Assert.AreEqual(1, mob.EquipedEquipment.Count());
            Assert.IsTrue(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_StrengthEffective()
        {
            equipment.Setup(e => e.Strength).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.StrengthStat = 1;

            Assert.AreEqual(3, mob.StrengthEffective);
        }

        [TestMethod]
        public void MobileObject_DexterityEffective()
        {
            equipment.Setup(e => e.Dexterity).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.DexterityStat = 1;

            Assert.AreEqual(3, mob.DexterityEffective);
        }

        [TestMethod]
        public void MobileObject_ConstitutionEffective()
        {
            equipment.Setup(e => e.Constitution).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.ConstitutionStat = 1;

            Assert.AreEqual(3, mob.ConstitutionEffective);
        }

        [TestMethod]
        public void MobileObject_IntelligenceEffective()
        {
            equipment.Setup(e => e.Intelligence).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.IntelligenceStat = 1;

            Assert.AreEqual(3, mob.IntelligenceEffective);
        }

        [TestMethod]
        public void MobileObject_WisdomEffective()
        {
            equipment.Setup(e => e.Wisdom).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.WisdomStat = 1;

            Assert.AreEqual(3, mob.WisdomEffective);
        }

        [TestMethod]
        public void MobileObject_CharismaEffective()
        {
            equipment.Setup(e => e.Charisma).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.CharismaStat = 1;

            Assert.AreEqual(3, mob.CharismaEffective);
        }

        [TestMethod]
        public void MobileObject_MaxHealthEffective_CaluculateValue()
        {
            equipment.Setup(e => e.MaxHealth).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.ConstitutionStat = 1;

            Assert.AreEqual(12, mob.MaxHealth);
        }

        [TestMethod]
        public void MobileObject_MaxHealthEffective_SetValue()
        {
            mob.MaxHealth = 1;
            Assert.AreEqual(1, mob.MaxHealth);
        }

        [TestMethod]
        public void MobileObject_MaxManaEffective_CaluculateValue()
        {
            equipment.Setup(e => e.MaxMana).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.IntelligenceStat = 1;

            Assert.AreEqual(12, mob.MaxMana);
        }

        [TestMethod]
        public void MobileObject_MaxManaEffective_SetValue()
        {
            mob.MaxMana = 1;
            Assert.AreEqual(1, mob.MaxMana);
        }

        [TestMethod]
        public void MobileObject_MaxStaminaEffective_CaluculateValue()
        {
            equipment.Setup(e => e.MaxStamina).Returns(1);
            EquipedEquipment(mob).Add(equipment.Object);
            EquipedEquipment(mob).Add(equipment.Object);
            mob.ConstitutionStat = 1;

            Assert.AreEqual(12, mob.MaxStamina);
        }

        [TestMethod]
        public void MobileObject_MaxStaminaEffective_SetValue()
        {
            mob.MaxStamina = 1;
            Assert.AreEqual(1, mob.MaxStamina);
        }

        [TestMethod]
        public void MobileObject_AddEquipment()
        {
            mob.AddEquipment(equipment.Object);

            Assert.IsTrue(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_RemoveEquipment_ItemIsEquipped()
        {
            EquipedEquipment(mob).Add(equipment.Object);

            mob.RemoveEquipment(equipment.Object);

            Assert.IsFalse(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_RemoveEquipment_ItemIsNotEquipped()
        {
            mob.RemoveEquipment(equipment.Object);

            Assert.IsFalse(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_EquipedArmor_ArmorIsEquipped()
        {
            EquipedEquipment(mob).Add(armor.Object);

            Assert.IsTrue(mob.EquipedArmor.Contains(armor.Object));
        }

        [TestMethod]
        public void MobileObject_EquipedWeapon_WeaponIsEquipped()
        {
            EquipedEquipment(mob).Add(weapon.Object);

            Assert.IsTrue(mob.EquipedWeapon.Contains(weapon.Object));
        }

        [TestMethod]
        public void MobileObject_EquipedWeapon_NoWeaponIsEquippedReturnBareFists()
        {
            mob.StrengthStat = 1;

            IWeapon weapon = mob.EquipedWeapon.FirstOrDefault();
            Assert.AreEqual(1, mob.EquipedWeapon.Count());
            Assert.AreEqual(1, weapon.DamageList.FirstOrDefault().Dice.Die);
            Assert.AreEqual(1, weapon.DamageList.FirstOrDefault().Dice.Sides);
            Assert.AreEqual(Stats.Stat.Dexterity, weapon.AttackerStat);
            Assert.AreEqual(Stats.Stat.Dexterity, weapon.DeffenderStat);
            Assert.AreEqual(1, weapon.Speed);
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Strength()
        {
            mob.StrengthStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Strength));
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Dexterity()
        {
            mob.DexterityStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Dexterity));
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Constitution()
        {
            mob.ConstitutionStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Constitution));
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Intelligence()
        {
            mob.IntelligenceStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Intelligence));
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Wisdom()
        {
            mob.WisdomStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Wisdom));
        }

        [TestMethod]
        public void MobileObject_GetStatEffective_Charisma()
        {
            mob.CharismaStat = 1;

            Assert.AreEqual(1, mob.GetStatEffective(Stats.Stat.Charisma));
        }

        [TestMethod]
        public void MobileObject_CalculateToHitRoll()
        {
            mob.StrengthStat = 2;

            Assert.AreEqual(1, mob.CalculateToHitRoll(Stats.Stat.Strength));
        }

        [TestMethod]
        public void MobileObject_CalculateToDodgeRoll()
        {
            mob.StrengthStat = 8;

            Assert.AreEqual(8, mob.CalculateToDodgeRoll(Stats.Stat.Strength, 1, 1));
            Assert.AreEqual(4, mob.CalculateToDodgeRoll(Stats.Stat.Strength, 2, 1));
            Assert.AreEqual(2, mob.CalculateToDodgeRoll(Stats.Stat.Strength, 3, 1));
        }

        [TestMethod]
        public void MobileObject_CalculateToDodgeRoll_CombatRoundResetsDodgeRate()
        {
            mob.StrengthStat = 8;

            Assert.AreEqual(8, mob.CalculateToDodgeRoll(Stats.Stat.Strength, 1, 1));
            Assert.AreEqual(8, mob.CalculateToDodgeRoll(Stats.Stat.Strength, 1, 2));
        }

        [TestMethod]
        public void MobileObject_TakeDamage_1Armor()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;

            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(1, mob.Health);
            Assert.AreEqual(9, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_2Armor()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;

            EquipedEquipment(mob).Add(armor.Object);
            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(2, mob.Health);
            Assert.AreEqual(8, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_OnlyDieOnce()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);

            evnt.Verify(e => e.OnDeath(mob), Times.Once);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_Shield()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;

            EquipedEquipment(mob).Add(shield.Object);

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(0, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_AbsorbHealth()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 1;
            dice.Setup(e => e.RollDice()).Returns(10);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(-1);

            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-10, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_BlockMoreDamageThanDealt()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 1;
            dice.Setup(e => e.RollDice()).Returns(10);

            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(0, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_ToMuchHealth()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;
            dice.Setup(e => e.RollDice()).Returns(10);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(-1);
            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-10, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceDamageMultiplier()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 5;
            mob.Race.Slash = -1;
            dice.Setup(e => e.RollDice()).Returns(10);

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-5, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_Die()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 1;
            EquipedEquipment(mob).Add(armor.Object);

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(-8, mob.Health);
            Assert.AreEqual(9, damageDealt);
            evnt.Verify(e => e.OnDeath(mob), Times.Once);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierAcid()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Acid);
            mob.Race.Acid = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierBludgeon()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Bludgeon);
            mob.Race.Bludgeon = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierCold()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Cold);
            mob.Race.Cold = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierFire()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Fire);
            mob.Race.Fire = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierForce()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Force);
            mob.Race.Force = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierLightning()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Lightning);
            mob.Race.Lightning = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierNecrotic()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Necrotic);
            mob.Race.Necrotic = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPierce()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Pierce);
            mob.Race.Pierce = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPoison()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Poison);
            mob.Race.Poison = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPsychic()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Psychic);
            mob.Race.Psychic = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierRadiant()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Radiant);
            mob.Race.Radiant = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierSlash()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Slash);
            mob.Race.Slash = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierThunder()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 100;
            damage.Setup(e => e.Type).Returns(DamageType.Thunder);
            mob.Race.Thunder = 2;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_GiveExp_InParty()
        {
            party.Setup(e => e.CurrentPartyMembers(pc.Object)).Returns(new List<IMobileObject>() { pc.Object, mob2.Object });
            mob.Health = 3;
            mob.EXP = 100;
            mob.Money = 20;
            mob.KeyWords.Add("mob");

            mob.TakeDamage(10, damage.Object, pc.Object);

            pc.VerifySet(e => e.Experience = 50);
            pc.VerifySet(e => e.Money = 10);
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "pc killed mob.  You receive 50 exp and 10 coins.")), Times.Once);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_GiveExp_NotInParty()
        {
            mob.Health = 3;
            mob.EXP = 100;
            mob.Money = 10;
            mob.KeyWords.Add("mob");

            mob.TakeDamage(10, damage.Object, pc.Object);

            pc.VerifySet(e => e.Experience = 100);
            pc.VerifySet(e => e.Money = 10, Times.Never);
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()), Times.Never);
        }

        [TestMethod]
        public void MobileObject_TakeCombatDamage()
        {
            mob.ConstitutionStat = 10;
            mob.Health = 100;
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(10);

            EquipedEquipment(mob).Add(armor.Object);

            //can block perfectly on 1st attack
            int damageDealt = mob.TakeCombatDamage(10, damage.Object, pc.Object, 1);
            Assert.AreEqual(100, mob.Health);
            Assert.AreEqual(0, damageDealt);

            //can block half as well
            damageDealt = mob.TakeCombatDamage(10, damage.Object, pc.Object, 1);
            Assert.AreEqual(95, mob.Health);
            Assert.AreEqual(5, damageDealt);

            //can block half as well as last time
            damageDealt = mob.TakeCombatDamage(10, damage.Object, pc.Object, 1);
            Assert.AreEqual(87, mob.Health);
            Assert.AreEqual(8, damageDealt);

            //new combat round, can block perfectly on 1st attack
            damageDealt = mob.TakeCombatDamage(10, damage.Object, pc.Object, 2);
            Assert.AreEqual(87, mob.Health);
            Assert.AreEqual(0, damageDealt);
        }

        [TestMethod]
        public void MobileObject_AddDefenseStatBonus()
        {
            damage.Setup(e => e.BonusDefenseStat).Returns(Stat.Dexterity);
            mob.ConstitutionStat = 1;
            mob.Health = 10;
            mob.DexterityStat = 2;

            EquipedEquipment(mob).Add(armor.Object);
            EquipedEquipment(mob).Add(armor.Object);

            mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(4, mob.Health);
        }

        [TestMethod]
        public void MobileObject_CalculateDamage()
        {
            int result = mob.CalculateDamage(damage.Object);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MobileObject_AddDamageStatBonus()
        {
            damage.Setup(e => e.BonusDamageStat).Returns(Stat.Dexterity);
            mob.DexterityStat = 2;

            int result = mob.CalculateDamage(damage.Object);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void MobileObject_CalculateAttackOrderRoll()
        {
            mob.DexterityStat = 2;

            int result = mob.CalculateAttackOrderRoll();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MobileObject_Die()
        {
            EquipedEquipment(mob).Add(armor.Object);
            mob.Money = 10;
            mob.Enchantments.Add(enchantment.Object);

            ICorpse corpse = mob.Die(attacker.Object);

            Assert.IsFalse(mob.IsAlive);
            Assert.AreEqual("A corpse lies here.", corpse.ShortDescription);
            Assert.AreEqual("corpseLookDescription", corpse.LookDescription);
            Assert.AreEqual("corpseLookDescription", corpse.ExamineDescription);
            Assert.IsTrue(corpse.KeyWords.Contains("Corpse"));
            Assert.AreEqual("corpse", corpse.SentenceDescription);
            Assert.AreEqual(3, corpse.Items.Count);
            Assert.IsTrue(corpse.Items.Contains(item.Object));
            Assert.IsTrue(corpse.Items.Contains(armor.Object));
            Assert.AreSame(attacker.Object, corpse.Killer);

            IMoney corpseMoney = null;
            foreach (IItem localItem in corpse.Items)
            {
                IMoney money = (localItem as IMoney);
                if (money != null)
                {
                    corpseMoney = money;
                    break;
                }
            }

            Assert.AreEqual(10ul, corpseMoney.Value);
            Assert.AreEqual(0, mob.Items.Count);
            Assert.AreEqual(0, mob.EquipedEquipment.Count());
            Assert.AreEqual(0ul, mob.Money);
            Assert.IsTrue(corpse.Items.Contains(item.Object));
            evnt.Verify(e => e.OnDeath(mob), Times.Once);
            enchantment.VerifySet(e => e.EnchantmentEndingDateTime = new DateTime());
        }

        [TestMethod]
        public void MobileObject_DieWhilePossessed_PossesserLooks()
        {
            mob.PossingMob = pc.Object;
            mob.Money = 10;

            mob.Die(null);

            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
            pc.VerifySet(e => e.PossedMob = null);
            Assert.IsNull(mob.PossingMob);
        }

        [TestMethod]
        public void MobileObject_Die_CorpseDescriptionSet()
        {
            mob.CorpseDescription = "corp desc";
            EquipedEquipment(mob).Add(armor.Object);
            mob.Money = 10;

            ICorpse corpse = mob.Die(null);

            Assert.IsFalse(mob.IsAlive);
            Assert.AreEqual("A corpse lies here.", corpse.ShortDescription);
            Assert.AreEqual("corp desc", corpse.LookDescription);
            Assert.AreEqual("corp desc", corpse.ExamineDescription);
            Assert.IsTrue(corpse.KeyWords.Contains("Corpse"));
            Assert.AreEqual("corpse", corpse.SentenceDescription);
            Assert.AreEqual(3, corpse.Items.Count);
            Assert.IsTrue(corpse.Items.Contains(item.Object));
            Assert.IsTrue(corpse.Items.Contains(armor.Object));

            IMoney corpseMoney = null;
            foreach (IItem localItem in corpse.Items)
            {
                corpseMoney = localItem as IMoney;
                if (corpseMoney != null)
                {
                    break;
                }
            }

            Assert.AreEqual(10ul, corpseMoney.Value);
            Assert.AreEqual(0, mob.Items.Count);
            Assert.AreEqual(0, mob.EquipedEquipment.Count());
            Assert.AreEqual(0ul, mob.Money);
            evnt.Verify(e => e.OnDeath(mob), Times.Once);
        }

        [TestMethod]
        public void MobileObject_LevelMobileObject()
        {
            mob.StrengthStat = 15;
            mob.DexterityStat = 25;
            mob.ConstitutionStat = 35;
            mob.IntelligenceStat = 45;
            mob.WisdomStat = 55;
            mob.CharismaStat = 65;
            mob.LevelMobileObject();

            Assert.AreEqual(5, mob.LevelPoints);
            Assert.AreEqual(2, mob.Level);
            Assert.AreEqual(390, mob.MaxHealth);
            Assert.AreEqual(500, mob.MaxMana);
            Assert.AreEqual(390, mob.MaxStamina);
        }

        [TestMethod]
        public void MobileObject_AttributesCurrent_Blank()
        {
            IEnumerable<MobileAttribute> result = mob.AttributesCurrent;
            Assert.IsInstanceOfType(result, typeof(IEnumerable<MobileAttribute>));
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void MobileObject_AttributesCurrent_RaceAttributes()
        {
            mob.Race = race.Object;

            IEnumerable<MobileAttribute> result = mob.AttributesCurrent;
            Assert.IsInstanceOfType(result, typeof(IEnumerable<MobileAttribute>));
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(MobileAttribute.Fly));
        }

        [TestMethod]
        public void MobileObject_AttributesCurrent_EquipmentAtributes()
        {
            EquipedEquipment(mob).Add(armor.Object);

            IEnumerable<MobileAttribute> result = mob.AttributesCurrent;
            Assert.IsInstanceOfType(result, typeof(IEnumerable<MobileAttribute>));
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(MobileAttribute.Fly));
        }

        [TestMethod]
        public void MobileObject_FollowTarget_Blank()
        {
            Assert.IsNull(mob.FollowTarget);
        }

        [TestMethod]
        public void MobileObject_FollowTarget_Set()
        {
            mob2.Setup(e => e.IsAlive).Returns(true);

            mob.FollowTarget = mob2.Object;

            Assert.AreSame(mob2.Object, mob.FollowTarget);
        }

        [TestMethod]
        public void MobileObject_FollowTarget_TargetDead()
        {
            mob2.Setup(e => e.IsAlive).Returns(false);

            mob.FollowTarget = mob2.Object;

            Assert.IsNull(mob.FollowTarget);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage()
        {
            mob.Health = 1;
            mob.MaxHealth = 10;
            mob.Mana = 2;
            mob.MaxMana = 20;
            mob.Stamina = 3;
            mob.MaxStamina = 30;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(2, queue.Count());
            queue.TryDequeue(out string message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("\r\n1/10 2/20 3/30\r\n", message);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_Possessed()
        {
            mob.Health = 1;
            mob.MaxHealth = 10;
            mob.Mana = 2;
            mob.MaxMana = 20;
            mob.Stamina = 3;
            mob.MaxStamina = 30;
            mob.PossingMob = pc.Object;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(2, queue.Count());
            string message;
            queue.TryDequeue(out message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("\r\n1/10 2/20 3/30\r\n", message);
            pc.Verify(e => e.EnqueueMessage("test"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("\r\n1/10 2/20 3/30\r\n"), Times.Once);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_LevelPoints()
        {
            mob.LevelPoints = 10;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(3, queue.Count());
            string message;
            queue.TryDequeue(out message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("\r\n0/0 0/0 0/0\r\n", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("You have 10 level points to spend.", message);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_RemoveMessageOver100()
        {
            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            for (int i = 0; i < 101; i++)
            {
                queue.Enqueue("a");
            }

            mob.EnqueueMessage("test");

            Assert.AreEqual(99, queue.Count());
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_MountStamina()
        {
            mob.Health = 1;
            mob.MaxHealth = 10;
            mob.Mana = 2;
            mob.MaxMana = 20;
            mob.Stamina = 3;
            mob.MaxStamina = 30;
            mob.Mount = mount.Object;
            mount.Setup(e => e.Riders).Returns(new List<IMobileObject>() { mob });

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(2, queue.Count());
            queue.TryDequeue(out string message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("\r\n1/10 2/20 3/30 4/40\r\n", message);
        }

        [TestMethod]
        public void MobileObject_DequeueMessage()
        {
            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);
            queue.Enqueue("test");

            Assert.AreEqual("test", mob.DequeueMessage());
        }

        [TestMethod]
        public void MobileObject_DequeueMessage_NoMessage()
        {
            Assert.AreEqual(null, mob.DequeueMessage());
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_RequestAsset()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "requestasset|sound|test";
            mob.EnqueueCommand(message);
            string result;
            outQueue.TryDequeue(out result);
            Assert.AreEqual("serialization", result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_ValidateAsset()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "validateAsset";
            mob.EnqueueCommand(message);
            string result;
            outQueue.TryDequeue(out result);
            Assert.AreEqual("validateAsset|abc", result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_RequestAssetLogError()
        {
            fileIO.Setup(e => e.Exists("c:\\test")).Returns(false);

            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "requestasset|sound|test";
            mob.EnqueueCommand(message);
            logger.Verify(e => e.Log(LogLevel.ERROR, It.Is<string>(f => f == "File c:\\test does not exit.")), Times.Once);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_Say()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);

            string message = "say test";
            mob.EnqueueCommand(message);
            string result;
            queue.TryDequeue(out result);
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_Shout()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);

            string message = "shout test";
            mob.EnqueueCommand(message);
            string result;
            queue.TryDequeue(out result);
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_Tell()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);

            string message = "tell test";
            mob.EnqueueCommand(message);
            string result;
            queue.TryDequeue(out result);
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_Emote()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);

            string message = "emote test";
            mob.EnqueueCommand(message);
            string result;
            queue.TryDequeue(out result);
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand()
        {
            ConcurrentQueue<string> queue = GetMobCommandQueue(mob);

            string message = "test";
            mob.EnqueueCommand(message);
            string result;
            queue.TryDequeue(out result);
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void MobileObject_DequeueCommunication()
        {
            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            string message = "test";
            queue.Enqueue(message);

            Assert.AreEqual(message, mob.DequeueCommunication());
        }

        [TestMethod]
        public void MobileObject_DequeueCommunication_NoMessage()
        {
            Assert.AreEqual(null, mob.DequeueCommunication());
        }

        [TestMethod]
        public void MobileObject_DequeueCommand()
        {
            ConcurrentQueue<string> queue = GetMobCommandQueue(mob);
            string message = "test";
            queue.Enqueue(message);

            Assert.AreEqual(message, mob.DequeueCommand());
        }

        [TestMethod]
        public void MobileObject_DequeueCommand_NoMessage()
        {
            Assert.AreEqual(null, mob.DequeueCommand());
        }

        [TestMethod]
        public void MobileObject_IsInCombat_True()
        {
            combat.Setup(e => e.IsInCombat(mob)).Returns(true);

            Assert.IsTrue(mob.IsInCombat);
        }

        [TestMethod]
        public void MobileObject_IsInCombat_False()
        {
            combat.Setup(e => e.IsInCombat(mob)).Returns(false);

            Assert.IsFalse(mob.IsInCombat);
        }

        [TestMethod]
        public void MobileObject_AreFigthing_True()
        {
            combat.Setup(e => e.AreFighting(mob, mob2.Object)).Returns(true);

            Assert.IsTrue(mob.AreFighting(mob2.Object));
        }

        [TestMethod]
        public void MobileObject_AreFigthing_False()
        {
            combat.Setup(e => e.AreFighting(mob, mob2.Object)).Returns(false);

            Assert.IsFalse(mob.AreFighting(mob2.Object));
        }

        [TestMethod]
        public void MobileObject_KnownSkills()
        {
            Assert.IsInstanceOfType(mob.KnownSkills, typeof(Dictionary<string, ISkill>));
        }

        [TestMethod]
        public void MobileObject_KnownSpells()
        {
            Assert.IsInstanceOfType(mob.SpellBook, typeof(Dictionary<string, ISpell>));
        }

        [TestMethod]
        public void MobileObject_HealthDescription()
        {
            mob.MaxHealth = 100;
            mob.SentenceDescription = "Mob";
            mob.ExamineDescription = "Examine";

            mob.Health = 100;
            Assert.AreEqual("Mob is in perfect health.", mob.HealthDescription);

            mob.Health = 80;
            Assert.AreEqual("Mob has some light scratches on them but nothing bad.", mob.HealthDescription);

            mob.Health = 60;
            Assert.AreEqual("Mob has some minor cuts with traces of blood.", mob.HealthDescription);

            mob.Health = 40;
            Assert.AreEqual("Mob has deep lacerations that will leave scars.", mob.HealthDescription);

            mob.Health = 20;
            Assert.AreEqual("Mob has been badly beaten.  They have bruises on bruises that are covered in blood from their many open wounds.", mob.HealthDescription);

            mob.Health = 0;
            Assert.AreEqual("Mob has begun to grow pale from loss of blood.  They may soon be riding on Charon's boat to the underworld.", mob.HealthDescription);

            mob.Health = -10;
            Assert.AreEqual("Examine", mob.HealthDescription);

        }

        private class UnitTestMobileObject : MobileObject, INonPlayerCharacter //needed for exp testing
        {
            public UnitTestMobileObject(IRoom room, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription, string corpseDescription) : base(room, examineDescription, lookDescription, sentenceDescription, shortDescription, corpseDescription)
            {
            }

            public int EXP { get; set; }


            public int CharismaMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int CharismaMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int ConstitutionMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int ConstitutionMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int DexterityMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int DexterityMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int IntelligenceMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int IntelligenceMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ILevelRange LevelRange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public List<IEquipment> NpcEquipedEquipment => throw new NotImplementedException();
            public List<IPersonality> Personalities => throw new NotImplementedException();
            public int StrengthMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int StrengthMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int WisdomMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int WisdomMin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public NonPlayerCharacter.MobType? TypeOfMob { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public object Clone()
            {
                throw new NotImplementedException();
            }
        }

        private ConcurrentQueue<string> GetMobMessageQueue(IMobileObject mob)
        {
            ConcurrentQueue<string> queue;
            PropertyInfo info = mob.GetType().GetProperty("_messageQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            queue = (ConcurrentQueue<string>)info.GetValue(mob);
            return queue;
        }

        private ConcurrentQueue<string> GetMobCommunicationQueue(IMobileObject mob)
        {
            ConcurrentQueue<string> queue;
            PropertyInfo info = mob.GetType().GetProperty("_communicationQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            queue = (ConcurrentQueue<string>)info.GetValue(mob);
            return queue;
        }

        private ConcurrentQueue<string> GetMobCommandQueue(IMobileObject mob)
        {
            ConcurrentQueue<string> queue;
            PropertyInfo info = mob.GetType().GetProperty("_commandQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            queue = (ConcurrentQueue<string>)info.GetValue(mob);
            return queue;
        }

        private List<IEquipment> EquipedEquipment(IMobileObject mob)
        {
            return mob.EquipedEquipment as List<IEquipment>;
        }
    }
}