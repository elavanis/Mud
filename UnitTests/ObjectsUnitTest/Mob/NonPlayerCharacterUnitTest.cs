using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Mob;
using Moq;
using Objects.Personality.Interface;
using System.Collections.Generic;
using Objects.Item.Items.Interface;
using System.Linq;
using Objects.Room.Interface;
using Objects.LevelRange;
using System.Reflection;
using Objects.Global.DefaultValues.Interface;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Global.Exp.Interface;
using Objects.Die.Interface;
using Objects.Mob.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Settings.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Item.Interface;
using System.Collections.Concurrent;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global.Notify.Interface;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class NonPlayerCharacterUnitTest
    {
        NonPlayerCharacter npc;
        Mock<INonPlayerCharacter> oldFollowTarget;
        Mock<INonPlayerCharacter> newFollowTarget;
        Mock<IRandomDropGenerator> randomDropGenerator;
        Mock<IItem> item;
        Mock<IDice> dice;
        Mock<IDefaultValues> defaultValues;
        Mock<ISettings> settings;
        Mock<IRandom> random;
        Mock<IExperience> experience;
        Mock<IPersonality> personality;
        Mock<IEquipment> equipment;
        Mock<IArmor> armor;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<IRoom> room;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        List<INonPlayerCharacter> npcList;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IMobileObject> attacker;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            randomDropGenerator = new Mock<IRandomDropGenerator>();
            item = new Mock<IItem>();
            dice = new Mock<IDice>();
            defaultValues = new Mock<IDefaultValues>();
            settings = new Mock<ISettings>();
            random = new Mock<IRandom>();
            experience = new Mock<IExperience>();
            personality = new Mock<IPersonality>();
            equipment = new Mock<IEquipment>();
            armor = new Mock<IArmor>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            room = new Mock<IRoom>();
            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            oldFollowTarget = new Mock<INonPlayerCharacter>();
            newFollowTarget = new Mock<INonPlayerCharacter>();
            npcList = new List<INonPlayerCharacter>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            attacker = new Mock<IMobileObject>();

            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(2);
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(1);
            defaultValues.Setup(e => e.DiceForArmorLevel(1)).Returns(dice.Object);
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            settings.Setup(e => e.Multiplier).Returns(1.1);
            randomDropGenerator.Setup(e => e.GenerateRandomDrop(npc)).Returns(item.Object);
            random.Setup(e => e.Next(1, 3)).Returns(1);
            experience.Setup(e => e.GetDefaultNpcExpForLevel(1)).Returns(1);
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0");
            room.Setup(e => e.NonPlayerCharacters).Returns(npcList);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            npcList.Add(newFollowTarget.Object);
            oldFollowTarget.Setup(e => e.Id).Returns(2);
            oldFollowTarget.Setup(e => e.IsAlive).Returns(true);
            newFollowTarget.Setup(e => e.Id).Returns(2);
            newFollowTarget.Setup(e => e.IsAlive).Returns(true);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => x);
            evnt.Setup(e => e.EnqueueMessage(It.IsAny<IMobileObject>(), It.IsAny<string>())).Returns((IMobileObject x, string y) => y);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.RandomDropGenerator = randomDropGenerator.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Experience = experience.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            npc = new NonPlayerCharacter();
            npc.Room = room.Object;
            npcList.Add(npc);

        }

        [TestMethod]
        public void NonPlayerCharacter_EXP()
        {
            npc.Level = 1;

            Assert.AreEqual(1, npc.EXP);

            npc.EXP = 10;

            Assert.AreEqual(10, npc.EXP);
        }

        [TestMethod]
        public void NonPlayerCharacter_Personalities_None()
        {
            Assert.AreEqual(0, npc.Personalities.Count);
            Assert.IsInstanceOfType(npc.Personalities, typeof(List<IPersonality>));
        }

        [TestMethod]
        public void NonPlayerCharacter_Personalities_Populated()
        {
            npc.Personalities.Add(personality.Object);

            Assert.AreEqual(1, npc.Personalities.Count);
        }

        [TestMethod]
        public void NonPlayerCharacter_NpcEquipedEquipment_None()
        {
            Assert.AreEqual(0, npc.NpcEquipedEquipment.Count);
            Assert.IsInstanceOfType(npc.NpcEquipedEquipment, typeof(List<IEquipment>));
        }

        [TestMethod]
        public void NonPlayerCharacter_NpcEquipedEquipment_Populated()
        {
            npc.NpcEquipedEquipment.Add(equipment.Object);

            Assert.AreEqual(1, npc.NpcEquipedEquipment.Count);
        }

        [TestMethod]
        public void NonPlayerCharacter_EquipedArmor_None()
        {
            Assert.AreEqual(0, npc.EquipedArmor.Count());
            Assert.IsInstanceOfType(npc.EquipedArmor, typeof(IEnumerable<IArmor>));
        }

        [TestMethod]
        public void NonPlayerCharacter_NpcEquipedEquipment_PopulatedNpcEquipedEquipment()
        {
            npc.NpcEquipedEquipment.Add(armor.Object);

            Assert.AreEqual(1, npc.EquipedArmor.Count());
        }

        [TestMethod]
        public void NonPlayerCharacter_NpcEquipedEquipment_PopulatedEquipedEquipment()
        {
            npc.AddEquipment(armor.Object);

            Assert.AreEqual(1, npc.EquipedArmor.Count());
        }

        [TestMethod]
        public void NonPlayerCharacter_FinsihLoad()
        {
            npc.Items.Add(item.Object);

            npc.Level = 1;

            npc.FinishLoad();

            Assert.AreEqual(1ul, npc.Money);
            item.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void NonPlayerCharacter_SetDefaultStats_StatRange()
        {
            npc.Level = 1;
            npc.StrengthMin = 1;
            npc.StrengthMax = 2;
            npc.DexterityMin = 1;
            npc.DexterityMax = 2;
            npc.ConstitutionMin = 1;
            npc.ConstitutionMax = 2;
            npc.IntelligenceMin = 1;
            npc.IntelligenceMax = 2;
            npc.WisdomMin = 1;
            npc.WisdomMax = 2;
            npc.CharismaMin = 1;
            npc.CharismaMax = 2;

            npc.FinishLoad();

            Assert.AreEqual(1, npc.StrengthStat);
            Assert.AreEqual(1, npc.DexterityStat);
            Assert.AreEqual(1, npc.ConstitutionStat);
            Assert.AreEqual(1, npc.IntelligenceStat);
            Assert.AreEqual(1, npc.WisdomStat);
            Assert.AreEqual(1, npc.CharismaStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_SetDefaultStats_LevelRange()
        {
            npc.LevelRange = new LevelRange() { LowerLevel = 1, UpperLevel = 2 };
            npc.Level = 1;

            npc.FinishLoad();

            Assert.AreEqual(43, npc.StrengthStat
                + npc.DexterityStat
                + npc.ConstitutionStat
                + npc.IntelligenceStat
                + npc.WisdomStat
                + npc.CharismaStat
                );
        }

        [TestMethod]
        public void NonPlayerCharacter_SetDefaultStats_Level2()
        {
            npc.Level = 2;

            npc.FinishLoad();

            Assert.AreEqual(50, npc.StrengthStat
                + npc.DexterityStat
                + npc.ConstitutionStat
                + npc.IntelligenceStat
                + npc.WisdomStat
                + npc.CharismaStat
                );
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Str()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(0);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.StrengthStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Dex()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(1);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.DexterityStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Con()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(2);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.ConstitutionStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Int()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(3);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.IntelligenceStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Wis()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(4);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.WisdomStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Cha()
        {
            npc.Level = 1;

            random.Setup(e => e.Next(6)).Returns(5);

            npc.FinishLoad();
            Assert.AreEqual(8, npc.CharismaStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_PointsMoreThan100()
        {
            //we use reflection to find the method and test it in isolation so we can verify the level points are done properly
            MethodInfo mi = npc.GetType().GetMethod("LevelRandomStat", BindingFlags.Instance | BindingFlags.NonPublic);
            npc.LevelPoints = 20;

            random.Setup(e => e.Next(6)).Returns(0);

            mi.Invoke(npc, null);

            Assert.AreEqual(2, npc.StrengthStat);
            Assert.AreEqual(18, npc.LevelPoints);
        }

        [TestMethod]
        public void NonPlayerCharacter_SetValuesToMax()
        {
            npc.ConstitutionStat = 1;
            npc.IntelligenceStat = 1;

            npc.FinishLoad();

            Assert.AreEqual(npc.MaxHealth, npc.Health);
            Assert.AreEqual(npc.MaxMana, npc.Mana);
            Assert.AreEqual(npc.MaxStamina, npc.Stamina);
        }

        [TestMethod]
        public void NonPlayerCharacter_LoadNpcEquipment_1()
        {
            npc.Level = 1;

            npc.FinishLoad();

            Assert.AreEqual(1, npc.NpcEquipedEquipment.Count);
        }

        [TestMethod]
        public void NonPlayerCharacter_LoadNpcEquipment_0()
        {
            npc.Level = 1;
            npc.AddEquipment(armor.Object);

            npc.FinishLoad();

            Assert.AreEqual(0, npc.NpcEquipedEquipment.Count);
        }

        [TestMethod]
        public void NonPlayerCharacter_Die()
        {
            ICorpse corpse = npc.Die(attacker.Object);

            room.Verify(e => e.RemoveMobileObjectFromRoom(npc));
            room.Verify(e => e.AddItemToRoom(It.Is<IItem>(f => f == corpse), 0));
            evnt.Verify(e => e.OnDeath(npc));
            Assert.AreSame(attacker.Object, corpse.Killer);
        }

        [TestMethod]
        public void NonPlayerCharacter_UpdateFollowerToCurrentReference()
        {
            npc.FollowTarget = oldFollowTarget.Object;

            npc.FinishLoad();

            Assert.AreSame(newFollowTarget.Object, npc.FollowTarget);
        }

        //[TestMethod]
        //public void NonPlayerCharacter_EnqueueMessage_NoPossessor()
        //{
        //    npc.EnqueueMessage("1");

        //    PropertyInfo propertyInfo = npc.GetType().GetProperty("_messageQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        //    ConcurrentQueue<string> concurrentQueue = (ConcurrentQueue<string>)propertyInfo.GetValue(npc);

        //    Assert.AreEqual(0, concurrentQueue.Count);
        //}

        [TestMethod]
        public void NonPlayerCharacter_EnqueueMessage_Possessed()
        {
            npc.PossingMob = oldFollowTarget.Object;

            npc.EnqueueMessage("1");

            PropertyInfo propertyInfo = npc.GetType().GetProperty("_messageQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            ConcurrentQueue<string> concurrentQueue = (ConcurrentQueue<string>)propertyInfo.GetValue(npc);

            string message;
            concurrentQueue.TryDequeue(out message);
            Assert.AreEqual("1", message);
        }
    }
}
