using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Mob;
using Shared.TagWrapper.Interface;
using Moq;
using Shared.TagWrapper;
using Objects.Global;
using Objects.Global.Stats;
using static Objects.Global.Stats.Stats;
using Objects.Global.Random.Interface;
using Objects.Mob.Interface;
using Objects.Damage.Interface;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Concurrent;
using Objects.Item.Items.Interface;
using Objects.Die.Interface;
using Objects.Command.Interface;
using Objects.Global.Engine.Engines;
using Objects.Global.Engine.Engines.CombatPair;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Room.Interface;
using static Objects.Room.Room;

namespace ObjectsUnitTest.Engine
{
    [TestClass]
    public class CombatUnitTest
    {
        Combat combat;

        [TestInitialize]
        public void Setup()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            combat = new Combat();
        }

        [TestMethod]
        public void Combat_AddCombatPair()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You begin to attack target.", TagWrapper.TagType.Info)).Returns("success");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            List<string> keywords = new List<string>();
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            defender.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();


            defender.Object.KeyWords.Add("target");

            IResult result = combat.AddCombatPair(attacker.Object, defender.Object);

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("success", result.ResultMessage);
        }

        [TestMethod]
        public void Combat_AddCombatPair_AllReadyFighting()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You are already attacking target.", TagWrapper.TagType.Info)).Returns("fail");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            List<string> keywords = new List<string>();
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            defender.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();

            defender.Object.KeyWords.Add("target");

            combat.AddCombatPair(attacker.Object, defender.Object);
            IResult result = combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("fail", result.ResultMessage);
        }

        [TestMethod]
        public void Combat_IsInCombat_Attacker()
        {
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsTrue(combat.IsInCombat(attacker.Object));
        }

        [TestMethod]
        public void Combat_IsInCombat_Defender()
        {
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsTrue(combat.IsInCombat(defender.Object));
        }

        [TestMethod]
        public void Combat_IsInCombat_NonCombatant()
        {
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            IMobileObject nonCombatant = new NonPlayerCharacter();
            combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsFalse(combat.IsInCombat(nonCombatant));
        }

        [TestMethod]
        public void Combat_AreFighting_Mob1()
        {
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            combat.AddCombatPair(attacker.Object, defender.Object);

            Assert.IsTrue(combat.AreFighting(attacker.Object, defender.Object));
        }

        [TestMethod]
        public void Combat_AreFighting_Mob2()
        {
            //Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            //GlobalValues.TagWrapper = tagWrapper.Object;
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            combat.AddCombatPair(attacker.Object, defender.Object);
            IMobileObject attacker2 = new NonPlayerCharacter();
            combat.AddCombatPair(attacker2, defender.Object);

            Assert.IsTrue(combat.AreFighting(defender.Object, attacker2));
        }

        [TestMethod]
        public void Combat_AreFighting_NotFighting()
        {
            Mock<IMobileObject> defender = new Mock<IMobileObject>();
            Mock<IMobileObject> attacker = new Mock<IMobileObject>();
            List<string> keywords = new List<string>();
            defender.Setup(e => e.KeyWords).Returns(keywords);

            combat.AddCombatPair(attacker.Object, defender.Object);
            IMobileObject nonCombatant = new NonPlayerCharacter();

            Assert.IsFalse(combat.AreFighting(attacker.Object, nonCombatant));
        }

        [TestMethod]
        public void Combat_DetermineIfHit_Hit()
        {
            Mock<IMobileObject> mMob = new Mock<IMobileObject>();
            mMob.Setup(e => e.CalculateToHitRoll(Stat.Dexterity)).Returns(1);
            mMob.Setup(e => e.CalculateToDodgeRoll(Stat.Dexterity)).Returns(1);

            Assert.IsTrue(combat.DetermineIfHit(mMob.Object, mMob.Object, Stat.Dexterity, Stat.Dexterity));
        }

        [TestMethod]
        public void Combat_DetermineIfHit_Miss()
        {
            Mock<IMobileObject> mMob = new Mock<IMobileObject>();
            mMob.Setup(e => e.CalculateToHitRoll(Stat.Dexterity)).Returns(0);
            mMob.Setup(e => e.CalculateToDodgeRoll(Stat.Dexterity)).Returns(1);

            Assert.IsFalse(combat.DetermineIfHit(mMob.Object, mMob.Object, Stat.Dexterity, Stat.Dexterity));
        }

        [TestMethod]
        public void Combat_DealDamage()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IMobileObject> mMob = new Mock<IMobileObject>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mMob.Setup(e => e.CalculateDamage(damage.Object)).Returns(2);
            mMob.Setup(e => e.TakeDamage(2, damage.Object, mMob.Object)).Returns(1);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;

            Assert.AreEqual(1, combat.DealDamage(mMob.Object, mMob.Object, damage.Object));
        }

        [TestMethod]
        public void Combat_ProcessCombatRound_RountIncrement()
        {
            PropertyInfo info = combat.GetType().GetProperty("_combatRound", BindingFlags.Instance | BindingFlags.NonPublic);

            combat.ProcessCombatRound();
            Assert.AreEqual(1u, info.GetValue(combat));
        }

        [TestMethod]
        public void Combat_ProcessAttack_AddDefendingMobToCombat()
        {
            Mock<IMobileObject> mob1 = new Mock<IMobileObject>();
            Mock<IMobileObject> mob2 = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            PropertyInfo info = combat.GetType().GetProperty("combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            ConcurrentDictionary<IMobileObject, CombatPair> combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)info.GetValue(combat);
            CombatPair pair = new CombatPair();

            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            mob1.Setup(e => e.Health).Returns(1);
            mob2.Setup(e => e.Health).Returns(1);
            mob1.Setup(e => e.Room).Returns(room.Object);
            mob2.Setup(e => e.Room).Returns(room.Object);
            pair.Attacker = mob1.Object;
            pair.Defender = mob2.Object;
            combatants.TryAdd(mob1.Object, pair);

            combat.ProcessCombatRound();

            Assert.AreEqual(2, combatants.Count);
        }

        [TestMethod]
        public void Combat_ProcessAttack_RemoveFightingInPeacefulRoom()
        {
            Mock<IMobileObject> mob1 = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            PropertyInfo info = combat.GetType().GetProperty("combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            ConcurrentDictionary<IMobileObject, CombatPair> combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)info.GetValue(combat);
            CombatPair pair = new CombatPair();
            pair.Attacker = mob1.Object;
            pair.Defender = mob1.Object;
            combatants.TryAdd(mob1.Object, pair);

            mob1.Setup(e => e.Health).Returns(1);
            mob1.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>() { RoomAttribute.Peaceful });

            combat.ProcessCombatRound();

            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_RemoveDeadCombatant()
        {
            Mock<IMobileObject> mob1 = new Mock<IMobileObject>();
            mob1.Setup(e => e.Health).Returns(1);
            Mock<IMobileObject> mob2 = new Mock<IMobileObject>();
            mob2.Setup(e => e.Health).Returns(0);
            PropertyInfo info = combat.GetType().GetProperty("combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            ConcurrentDictionary<IMobileObject, CombatPair> combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)info.GetValue(combat);
            CombatPair pair = new CombatPair();
            pair.Attacker = mob1.Object;
            pair.Defender = mob2.Object;
            combatants.TryAdd(mob1.Object, pair);
            pair = new CombatPair();
            pair.Attacker = mob2.Object;
            pair.Defender = mob1.Object;
            combatants.TryAdd(mob2.Object, pair);

            combat.ProcessCombatRound();
            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_RemoveCombatantInDifferentRoom()
        {
            Mock<IMobileObject> mob1 = new Mock<IMobileObject>();
            Mock<IMobileObject> mob2 = new Mock<IMobileObject>();
            PropertyInfo info = combat.GetType().GetProperty("combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            ConcurrentDictionary<IMobileObject, CombatPair> combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)info.GetValue(combat);
            CombatPair pair = new CombatPair();
            Mock<IRoom> room1 = new Mock<IRoom>();
            Mock<IRoom> room2 = new Mock<IRoom>();

            mob1.Setup(e => e.Health).Returns(1);
            mob1.Setup(e => e.Room).Returns(room1.Object);
            mob2.Setup(e => e.Health).Returns(1);
            mob2.Setup(e => e.Room).Returns(room2.Object);
            pair.Attacker = mob1.Object;
            pair.Defender = mob2.Object;
            combatants.TryAdd(mob1.Object, pair);
            pair = new CombatPair();
            pair.Attacker = mob2.Object;
            pair.Defender = mob1.Object;
            combatants.TryAdd(mob2.Object, pair);

            combat.ProcessCombatRound();
            Assert.AreEqual(0, combatants.Count);
        }

        [TestMethod]
        public void Combat_ProcessWeapons_()
        {
            Mock<IMobileObject> mob1 = new Mock<IMobileObject>();
            Mock<IMobileObject> mob2 = new Mock<IMobileObject>();
            Mock<IRoom> room = new Mock<IRoom>();
            PropertyInfo info = combat.GetType().GetProperty("combatants", BindingFlags.Instance | BindingFlags.NonPublic);
            ConcurrentDictionary<IMobileObject, CombatPair> combatants = (ConcurrentDictionary<IMobileObject, CombatPair>)info.GetValue(combat);
            CombatPair pair = new CombatPair();
            Mock<IWeapon> weapon = new Mock<IWeapon>();
            List<IWeapon> weapons = new List<IWeapon>() { weapon.Object, weapon.Object };
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            List<IDamage> damages = new List<IDamage>() { damage.Object };
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            mob1.Setup(e => e.Health).Returns(1);
            mob2.Setup(e => e.Health).Returns(1);
            mob1.Setup(e => e.Room).Returns(room.Object);
            mob2.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            pair.Attacker = mob1.Object;
            pair.Defender = mob2.Object;
            combatants.TryAdd(mob1.Object, pair);
            weapon.Setup(e => e.Speed).Returns(1);
            mob1.Setup(e => e.EquipedWeapon).Returns(weapons);
            damage.Setup(e => e.Dice).Returns(dice.Object);
            weapon.Setup(e => e.DamageList).Returns(damages);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;

            combat.ProcessCombatRound();

            weapon.Verify(e => e.DamageList, Times.Exactly(2));
            mob1.Verify(e => e.CalculateDamage(damage.Object), Times.Exactly(2));
            evnt.Verify(e => e.DamageDealtBeforeDefense(mob1.Object, mob2.Object, 0));
            evnt.Verify(e => e.DamageDealtAfterDefense(mob1.Object, mob2.Object, 0));
        }
    }
}
