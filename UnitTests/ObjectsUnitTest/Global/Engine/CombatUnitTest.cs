using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Mob;
using Shared.TagWrapper.Interface;
using Moq;
using Objects.Global;
using static Objects.Global.Stats.Stats;
using Objects.Mob.Interface;
using Objects.Damage.Interface;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Concurrent;
using Objects.Item.Items.Interface;
using Objects.Die.Interface;
using Objects.Command.Interface;
using Objects.Global.Engine.Engines;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Room.Interface;
using static Objects.Room.Room;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global.Engine.Engines.AdditionalCombat;
using Objects.Global.Damage.Interface;

namespace ObjectsUnitTest.Engine
{
    [TestClass]
    public class CombatUnitTest
    {
        Combat combat;
        ConcurrentDictionary<IMobileObject, CombatPair> combatants;
        List<string> keywords;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> defender;
        Mock<IMobileObject> attacker;
        Mock<IMobileObject> nonCombatant;
        Mock<IMobileObject> attacker2;
        Mock<IRoom> room;
        Mock<IRoom> room2;
        Mock<IWeapon> weapon;
        Mock<IDamage> damage;
        Mock<IDamageId> damageId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            keywords = new List<string>();
            tagWrapper = new Mock<ITagWrapper>();
            defender = new Mock<IMobileObject>();
            attacker = new Mock<IMobileObject>();
            nonCombatant = new Mock<IMobileObject>();
            attacker2 = new Mock<IMobileObject>();
            room = new Mock<IRoom>();
            room2 = new Mock<IRoom>();
            weapon = new Mock<IWeapon>();
            damage = new Mock<IDamage>();
            damageId = new Mock<IDamageId>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            defender.Setup(e => e.KeyWords).Returns(keywords);
            defender.Object.KeyWords.Add("target");
            defender.Setup(e => e.Health).Returns(1);
            defender.Setup(e => e.Room).Returns(room.Object);
            defender.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            attacker.Setup(e => e.Health).Returns(1);
            attacker.Setup(e => e.Room).Returns(room.Object);
            attacker.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            weapon.Setup(e => e.Speed).Returns(1);
            weapon.Setup(e => e.DamageList).Returns(new List<IDamage>() { damage.Object });
            damageId.Setup(e => e.Id).Returns(1);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.DamageId = damageId.Object;

            combat = new Combat();
            PropertyInfo propertyInfoCombatants = combat.GetType().GetProperty("Combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)propertyInfoCombatants.GetValue(combat);
        }

        [TestMethod]
        public void Combat_AddCombatPair()
        {
            IResult result = combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("You begin to attack target.", result.ResultMessage);
        }

        [TestMethod]
        public void Combat_AddCombatPair_AllReadyFighting()
        {
            combat.AddCombatPair(attacker.Object, defender.Object);
            IResult result = combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You are already attacking target.", result.ResultMessage);
        }

        [TestMethod]
        public void Combat_IsInCombat_Attacker()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            Assert.IsTrue(combat.IsInCombat(attacker.Object));
        }

        [TestMethod]
        public void Combat_IsInCombat_Defender()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            Assert.IsTrue(combat.IsInCombat(defender.Object));
        }

        [TestMethod]
        public void Combat_IsInCombat_NonCombatant()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            Assert.IsFalse(combat.IsInCombat(nonCombatant.Object));
        }

        [TestMethod]
        public void Combat_AreFighting_Mob1()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            Assert.IsTrue(combat.AreFighting(attacker.Object, defender.Object));
        }

        [TestMethod]
        public void Combat_AreFighting_Mob2()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });
            combatants.TryAdd(attacker2.Object, new CombatPair() { Attacker = attacker2.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker2.Object });

            Assert.IsTrue(combat.AreFighting(defender.Object, attacker2.Object));
        }

        [TestMethod]
        public void Combat_AreFighting_NotFighting()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            Assert.IsFalse(combat.AreFighting(attacker.Object, nonCombatant.Object));
        }

        [TestMethod]
        public void Combat_DetermineIfHit_Hit()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });

            combat.ProcessCombatRound();

            defender.Verify(e => e.TakeCombatDamage(0, damage.Object, attacker.Object, 1), Times.Once);
        }

        [TestMethod]
        public void Combat_DetermineIfHit_Miss()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });
            defender.Setup(e => e.CalculateToDodgeRoll(Stat.Strength, 0, 1)).Returns(1);

            combat.ProcessCombatRound();

            defender.Verify(e => e.TakeCombatDamage(0, damage.Object, attacker.Object, 1), Times.Never);
        }

        [TestMethod]
        public void Combat_ProcessCombatRound_RountIncrement()
        {
            PropertyInfo info = combat.GetType().GetProperty("_combatRound", BindingFlags.Instance | BindingFlags.NonPublic);

            combat.ProcessCombatRound();
            Assert.AreEqual(1u, info.GetValue(combat));
        }


        [TestMethod]
        public void Combat_ProcessCombatRound_SetWeaponId()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });
            combatants.TryAdd(defender.Object, new CombatPair() { Attacker = defender.Object, Defender = attacker.Object });
            weapon.SetupSequence(e => e.WeaponId).Returns(0).Returns(1);

            combat.ProcessCombatRound();

            weapon.VerifySet(e => e.WeaponId = 1);
        }


        [TestMethod]
        public void Combat_ProcessAttack_AddDefendingMobToCombat()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            combat.ProcessCombatRound();

            Assert.AreEqual(2, combatants.Count);
        }

        [TestMethod]
        public void Combat_ProcessAttack_RemoveFightingInPeacefulRoom()
        {
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.Peaceful });
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            combat.ProcessCombatRound();

            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_RemoveDeadCombatant()
        {
            defender.Setup(e => e.Health).Returns(0);
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            combat.ProcessCombatRound();
            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_RemoveCombatantInDifferentRoom()
        {
            defender.Setup(e => e.Room).Returns(room2.Object);
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            combat.ProcessCombatRound();
            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_Opponet_InCombat()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            IMobileObject result = combat.Opponet(attacker.Object);
            Assert.AreSame(defender.Object, result);
        }

        [TestMethod]
        public void Combat_Opponet_NotInCombat()
        {
            combatants.TryAdd(attacker.Object, new CombatPair() { Attacker = attacker.Object, Defender = defender.Object });

            IMobileObject result = combat.Opponet(defender.Object);
            Assert.IsNull(result);
        }
    }
}
