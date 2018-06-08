using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Logging.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings;
using Objects.Global.Settings.Interface;
using Objects.Global.Stats;
using Objects.Global.ValidateAsset.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Magic;
using Objects.Magic.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Race.Interface;
using Objects.Race.Races;
using Objects.Skill.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using Shared.TelnetItems;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Objects.Damage.Damage;
using static Objects.Global.Logging.LogSettings;
using static Objects.Mob.MobileObject;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class MobileObjectUnitTest
    {
        UnitTestMobileObject mob;

        [TestInitialize]
        public void Setup()
        {
            mob = new UnitTestMobileObject();
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
            Assert.IsNotNull(mob.Items);
            Assert.AreEqual(0, mob.Items.Count);
        }

        [TestMethod]
        public void MobileObject_Items_Populated()
        {
            Mock<IItem> item = new Mock<IItem>();
            mob.Items.Add(item.Object);

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
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            mob.AddEquipment(equipment.Object);


            Assert.AreEqual(1, mob.EquipedEquipment.Count());
            Assert.IsTrue(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_StrengthEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Strength).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.StrengthStat = 1;

            Assert.AreEqual(3, mob.StrengthEffective);
        }

        [TestMethod]
        public void MobileObject_DexterityEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Dexterity).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.DexterityStat = 1;

            Assert.AreEqual(3, mob.DexterityEffective);
        }

        [TestMethod]
        public void MobileObject_ConstitutionEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Constitution).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.ConstitutionStat = 1;

            Assert.AreEqual(3, mob.ConstitutionEffective);
        }

        [TestMethod]
        public void MobileObject_IntelligenceEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Intelligence).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.IntelligenceStat = 1;

            Assert.AreEqual(3, mob.IntelligenceEffective);
        }

        [TestMethod]
        public void MobileObject_WisdomEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Wisdom).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.WisdomStat = 1;

            Assert.AreEqual(3, mob.WisdomEffective);
        }

        [TestMethod]
        public void MobileObject_CharismaEffective()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.Charisma).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
            mob.CharismaStat = 1;

            Assert.AreEqual(3, mob.CharismaEffective);
        }

        [TestMethod]
        public void MobileObject_MaxHealthEffective_CaluculateValue()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.MaxHealth).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
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
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.MaxMana).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
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
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.MaxStamina).Returns(1);
            mob.AddEquipment(equipment.Object);
            mob.AddEquipment(equipment.Object);
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
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            mob.AddEquipment(equipment.Object);

            Assert.IsTrue(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_RemoveEquipment_ItemIsEquipped()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();
            mob.AddEquipment(equipment.Object);

            mob.RemoveEquipment(equipment.Object);

            Assert.IsFalse(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_RemoveEquipment_ItemIsNotEquipped()
        {
            Mock<IEquipment> equipment = new Mock<IEquipment>();

            mob.RemoveEquipment(equipment.Object);

            Assert.IsFalse(mob.EquipedEquipment.Contains(equipment.Object));
        }

        [TestMethod]
        public void MobileObject_EquipedArmor_ArmorIsEquipped()
        {
            Mock<IArmor> equipment = new Mock<IArmor>();

            mob.AddEquipment(equipment.Object);

            Assert.IsTrue(mob.EquipedArmor.Contains(equipment.Object));
        }


        [TestMethod]
        public void MobileObject_EquipedWeapon_WeaponIsEquipped()
        {
            Mock<IWeapon> equipment = new Mock<IWeapon>();

            mob.AddEquipment(equipment.Object);

            Assert.IsTrue(mob.EquipedWeapon.Contains(equipment.Object));
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
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;
            mob.StrengthStat = 1;

            Assert.AreEqual(1, mob.CalculateToHitRoll(Stats.Stat.Strength));
        }

        [TestMethod]
        public void MobileObject_CalculateToDodgeRoll()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;
            mob.StrengthStat = 1;

            Assert.AreEqual(1, mob.CalculateToDodgeRoll(Stats.Stat.Strength));
        }

        [TestMethod]
        public void MobileObject_TakeDamage_1Armor()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 10;
            random.Setup(e => e.Next(1)).Returns(1);
            dice.Setup(e => e.RollDice()).Returns(1);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(1, mob.Health);
            Assert.AreEqual(9, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_2Armor()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 10;
            random.Setup(e => e.Next(1)).Returns(1);
            dice.Setup(e => e.RollDice()).Returns(1);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            mob.AddEquipment(armor.Object);
            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(2, mob.Health);
            Assert.AreEqual(8, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_OnlyDieOnce()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            mob.ConstitutionStat = 1;
            mob.Health = 10;
            random.Setup(e => e.Next(1)).Returns(1);
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0 coin");

            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);

            evnt.Verify(e => e.OnDeath(mob), Times.Once);
        }


        [TestMethod]
        public void MobileObject_TakeDamage_Shield()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IShield> armor = new Mock<IShield>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 10;
            dice.Setup(e => e.RollDice()).Returns(1);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            armor.Setup(e => e.NegateDamagePercent).Returns(100);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(0, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_AbsorbHealth()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 1;
            dice.Setup(e => e.RollDice()).Returns(10);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(-1);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-10, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_BlockMoreDamageThanDealt()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 1;
            dice.Setup(e => e.RollDice()).Returns(10);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(0, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_ToMuchHealth()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 10;
            dice.Setup(e => e.RollDice()).Returns(10);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(-1);

            mob.AddEquipment(armor.Object);
            GlobalReference.GlobalValues.Random = random.Object;

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-10, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceDamageMultiplier()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.ConstitutionStat = 1;
            mob.Health = 5;
            mob.Race.Slash = -1;
            dice.Setup(e => e.RollDice()).Returns(10);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(5, damage.Object, pc.Object);
            Assert.AreEqual(10, mob.Health);
            Assert.AreEqual(-5, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_Die()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 1;
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("10 coins");
            dice.Setup(e => e.RollDice()).Returns(1);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            mob.AddEquipment(armor.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(-8, mob.Health);
            Assert.AreEqual(9, damageDealt);
            evnt.Verify(e => e.OnDeath(mob), Times.Once);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierAcid()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Acid);
            mob.Race.Acid = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierBludgeon()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Bludgeon);
            mob.Race.Bludgeon = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierCold()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Cold);
            mob.Race.Cold = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierFire()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Fire);
            mob.Race.Fire = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierForce()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Force);
            mob.Race.Force = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierLightning()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Lightning);
            mob.Race.Lightning = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierNecrotic()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Necrotic);
            mob.Race.Necrotic = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPierce()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Pierce);
            mob.Race.Pierce = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPoison()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Poison);
            mob.Race.Poison = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierPsychic()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Psychic);
            mob.Race.Psychic = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierRadiant()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Radiant);
            mob.Race.Radiant = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierSlash()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Slash);
            mob.Race.Slash = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_TakeDamage_RaceModifierThunder()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            mob.ConstitutionStat = 1;
            mob.Health = 100;
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Thunder);
            mob.Race.Thunder = 2;

            GlobalReference.GlobalValues.Engine = engine.Object;

            int damageDealt = mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(20, damageDealt);
        }

        [TestMethod]
        public void MobileObject_AddDefenseStatBonus()
        {
            mob.ConstitutionStat = 1;
            mob.Health = 10;
            mob.DexterityStat = 2;

            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IRandom> random = new Mock<IRandom>();

            damage.Setup(e => e.BonusDefenseStat).Returns(Stats.Stat.Dexterity);
            dice.Setup(e => e.RollDice()).Returns(1);
            armor.Setup(e => e.Dice).Returns(dice.Object);
            armor.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(1);
            mob.AddEquipment(armor.Object);
            mob.AddEquipment(armor.Object);
            random.Setup(e => e.Next(2)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

            mob.TakeDamage(10, damage.Object, pc.Object);
            Assert.AreEqual(4, mob.Health);
        }

        [TestMethod]
        public void MobileObject_CalculateDamage()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.RollDice()).Returns(1);
            damage.Setup(e => e.Dice).Returns(dice.Object);


            int result = mob.CalculateDamage(damage.Object);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MobileObject_AddDamageStatBonus()
        {
            mob.DexterityStat = 2;

            Mock<IDamage> damage = new Mock<IDamage>();
            damage.Setup(e => e.BonusDamageStat).Returns(Stats.Stat.Dexterity);
            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.RollDice()).Returns(1);
            damage.Setup(e => e.Dice).Returns(dice.Object);
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(2)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;


            int result = mob.CalculateDamage(damage.Object);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void MobileObject_CalculateAttackOrderRoll()
        {
            mob.DexterityStat = 2;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(2)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;


            int result = mob.CalculateAttackOrderRoll();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MobileObject_Die()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("10 coins");
            mob.Items.Add(item.Object);
            mob.AddEquipment(armor.Object);
            mob.Money = 10;
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            ICorpse corpse = mob.Die();

            Assert.IsFalse(mob.IsAlive);
            Assert.AreEqual("A corpse lies here.", corpse.ShortDescription);
            Assert.AreEqual("This corpse once was living but no life exists here now.", corpse.LongDescription);
            Assert.AreEqual("This corpse once was living but no life exists here now.", corpse.ExamineDescription);
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
        public void MobileObject_Die_CorpseDescriptionSet()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            Mock<IItem> item = new Mock<IItem>();
            Mock<IArmor> armor = new Mock<IArmor>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob.CorpseLongDescription = "corp desc";
            mob.Items.Add(item.Object);
            mob.AddEquipment(armor.Object);
            mob.Money = 10;
            moneyToCoins.Setup(e => e.FormatedAsCoins(10)).Returns("10 coins");
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            ICorpse corpse = mob.Die();

            Assert.IsFalse(mob.IsAlive);
            Assert.AreEqual("A corpse lies here.", corpse.ShortDescription);
            Assert.AreEqual("corp desc", corpse.LongDescription);
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

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.Multiplier).Returns(1.1d);
            GlobalReference.GlobalValues.Settings = settings.Object;

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
            Mock<IRace> race = new Mock<IRace>();
            race.Setup(e => e.RaceAttributes).Returns(new List<MobileAttribute>() { MobileAttribute.Fly });
            mob.Race = race.Object;

            IEnumerable<MobileAttribute> result = mob.AttributesCurrent;
            Assert.IsInstanceOfType(result, typeof(IEnumerable<MobileAttribute>));
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(MobileAttribute.Fly));
        }

        [TestMethod]
        public void MobileObject_AttributesCurrent_EquipmentAtributes()
        {
            Mock<IArmor> armor = new Mock<IArmor>();
            armor.Setup(e => e.AttributesForMobileObjectsWhenEquiped).Returns(new List<MobileAttribute>() { MobileAttribute.Fly });
            mob.AddEquipment(armor.Object);

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
            UnitTestMobileObject unitTestMob = new UnitTestMobileObject();

            mob.FollowTarget = unitTestMob;

            Assert.AreSame(unitTestMob, mob.FollowTarget);
        }

        [TestMethod]
        public void MobileObject_FollowTarget_TargetDead()
        {
            UnitTestMobileObject unitTestMob = new UnitTestMobileObject();
            unitTestMob.IsAlive = false;

            mob.FollowTarget = unitTestMob;

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

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("1/10 ", TagType.Health)).Returns("Health");
            tagWrapper.Setup(e => e.WrapInTag("2/20 ", TagType.Mana)).Returns("Mana");
            tagWrapper.Setup(e => e.WrapInTag("3/30", TagType.Stamina)).Returns("Stamina");

            Mock<IEvent> evnt = new Mock<IEvent>();
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "test")).Returns("test");
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "\r\nHealthManaStamina\r\n")).Returns("AAA");
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(2, queue.Count());
            string message;
            queue.TryDequeue(out message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual("AAA", message);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_LevelPoints()
        {
            mob.LevelPoints = 10;

            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You have 10 level points to spend.", TagType.Info)).Returns("Level");

            Mock<IEvent> evnt = new Mock<IEvent>();
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "test")).Returns("test");
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "Level")).Returns("Level");
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            mob.EnqueueMessage("test");

            Assert.AreEqual(3, queue.Count());
            string message;
            queue.TryDequeue(out message);
            Assert.AreEqual("test", message);
            queue.TryDequeue(out message);
            Assert.AreEqual(null, message);
            queue.TryDequeue(out message);
            Assert.AreEqual("Level", message);
        }

        [TestMethod]
        public void MobileObject_EnqueueMessage_RemoveMessageOver100()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You have 10 level points to spend.", TagType.Info)).Returns("Level");

            Mock<IEvent> evnt = new Mock<IEvent>();
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "test")).Returns("test");
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), "Level")).Returns("Level");
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Event).Returns(evnt.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            ConcurrentQueue<string> queue = GetMobMessageQueue(mob);

            for (int i = 0; i < 101; i++)
            {
                queue.Enqueue("a");
            }

            mob.EnqueueMessage("test");

            Assert.AreEqual(99, queue.Count());
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
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIo = new Mock<IFileIO>();
            Mock<ISerialization> serilization = new Mock<ISerialization>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();

            settings.Setup(e => e.AssetsDirectory).Returns(@"c:\");
            fileIo.Setup(e => e.ReadFileBase64(@"c:\test")).Returns("abc123");
            serilization.Setup(e => e.Serialize(It.IsAny<Data>())).Returns("serilization");
            tagWrapper.Setup(e => e.WrapInTag("serilization", TagType.Data)).Returns("test");
            engine.Setup(e => e.Event).Returns(evnt.Object);
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), It.IsAny<string>())).Returns((IMobileObject a, string b) => b);
            fileIO.Setup(e => e.Exists("c:\\test")).Returns(true);

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIo.Object;
            GlobalReference.GlobalValues.Serialization = serilization.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;

            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "requestasset|sound|test";
            mob.EnqueueCommand(message);
            string result;
            outQueue.TryDequeue(out result);
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_ValidateAsset()
        {
            Mock<IValidateAsset> validateAsset = new Mock<IValidateAsset>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();

            validateAsset.Setup(e => e.GetCheckSum("validateAsset")).Returns("abc");
            tagWrapper.Setup(e => e.WrapInTag("validateAsset|abc", TagType.FileValidation)).Returns("test");

            GlobalReference.GlobalValues.ValidateAsset = validateAsset.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "validateAsset";
            mob.EnqueueCommand(message);
            string result;
            outQueue.TryDequeue(out result);
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void MobileObject_EnqueueCommand_RequestAssetLogError()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<ILogger> logger = new Mock<ILogger>();

            settings.Setup(e => e.AssetsDirectory).Returns(@"c:\");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.FileIO = null; //needed to make the test fail so it logs

            ConcurrentQueue<string> queue = GetMobCommunicationQueue(mob);
            ConcurrentQueue<string> outQueue = GetMobMessageQueue(mob);

            string message = "requestasset|sound|test";
            mob.EnqueueCommand(message);
            logger.Verify(e => e.Log(LogLevel.ERROR, It.IsAny<string>()), Times.Once);
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
            Mock<ICombat> combat = new Mock<ICombat>();
            combat.Setup(e => e.IsInCombat(mob)).Returns(true);
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Combat).Returns(combat.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            Assert.IsTrue(mob.IsInCombat);
        }

        [TestMethod]
        public void MobileObject_IsInCombat_False()
        {
            Mock<ICombat> combat = new Mock<ICombat>();
            combat.Setup(e => e.IsInCombat(mob)).Returns(false);
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Combat).Returns(combat.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            Assert.IsFalse(mob.IsInCombat);
        }

        [TestMethod]
        public void MobileObject_AreFigthing_True()
        {
            MobileObject mob2 = new NonPlayerCharacter();
            Mock<ICombat> combat = new Mock<ICombat>();
            combat.Setup(e => e.AreFighting(mob, mob2)).Returns(true);
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Combat).Returns(combat.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            Assert.IsTrue(mob.AreFighting(mob2));
        }

        [TestMethod]
        public void MobileObject_AreFigthing_False()
        {
            MobileObject mob2 = new NonPlayerCharacter();
            Mock<ICombat> combat = new Mock<ICombat>();
            combat.Setup(e => e.AreFighting(mob, mob2)).Returns(false);
            Mock<IEngine> engine = new Mock<IEngine>();
            engine.Setup(e => e.Combat).Returns(combat.Object);
            GlobalReference.GlobalValues.Engine = engine.Object;

            Assert.IsFalse(mob.AreFighting(mob2));
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

        private class UnitTestMobileObject : MobileObject
        {

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
    }
}
