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

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class NonPlayerCharacterUnitTest
    {
        NonPlayerCharacter npc;
        Mock<IRandomDropGenerator> randomDropGenerator;
        Mock<IItem> item;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            Mock<IDice> dice = new Mock<IDice>();
            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            Mock<ISettings> settings = new Mock<ISettings>();
            randomDropGenerator = new Mock<IRandomDropGenerator>();
            item = new Mock<IItem>();

            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(2);
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(1);
            defaultValues.Setup(e => e.DiceForArmorLevel(1)).Returns(dice.Object);
            settings.Setup(e => e.BaseStatValue).Returns(7);
            randomDropGenerator.Setup(e => e.GenerateRandomDrop(npc)).Returns(item.Object);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.RandomDropGenerator = randomDropGenerator.Object;

            npc = new NonPlayerCharacter();
        }

        [TestMethod]
        public void NonPlayerCharacter_EXP()
        {
            Mock<IExperience> experience = new Mock<IExperience>();
            experience.Setup(e => e.GetDefaultNpcExpForLevel(1)).Returns(1);
            GlobalReference.GlobalValues.Experience = experience.Object;

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
            Mock<IPersonality> personality = new Mock<IPersonality>();
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
            Mock<IEquipment> equipment = new Mock<IEquipment>();
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
            Mock<IArmor> equipment = new Mock<IArmor>();
            npc.NpcEquipedEquipment.Add(equipment.Object);

            Assert.AreEqual(1, npc.EquipedArmor.Count());
        }

        [TestMethod]
        public void NonPlayerCharacter_NpcEquipedEquipment_PopulatedEquipedEquipment()
        {
            Mock<IArmor> equipment = new Mock<IArmor>();
            npc.AddEquipment(equipment.Object);

            Assert.AreEqual(1, npc.EquipedArmor.Count());
        }

        [TestMethod]
        public void NonPlayerCharacter_FinsihLoad()
        {
            Mock<IItem> item = new Mock<IItem>();
            npc.Items.Add(item.Object);

            npc.Level = 1;

            npc.FinishLoad();

            Assert.AreEqual(1ul, npc.Money);
            item.Verify(e => e.FinishLoad(-1), Times.Once);
        }

        [TestMethod]
        public void NonPlayerCharacter_SetDefaultStats_StatRange()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1, 3)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

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
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1, 3)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

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
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1, 3)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(2);

            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(1);
            defaultValues.Setup(e => e.DiceForArmorLevel(2)).Returns(dice.Object);
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            settings.Setup(e => e.Multiplier).Returns(1.1);
            GlobalReference.GlobalValues.Settings = settings.Object;

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

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(0);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.StrengthStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Dex()
        {
            npc.Level = 1;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.DexterityStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Con()
        {
            npc.Level = 1;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(2);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.ConstitutionStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Int()
        {
            npc.Level = 1;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(3);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.IntelligenceStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Wis()
        {
            npc.Level = 1;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(4);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.WisdomStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_Cha()
        {
            npc.Level = 1;

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.BaseStatValue).Returns(7);
            settings.Setup(e => e.AssignableStatPoints).Returns(1);
            GlobalReference.GlobalValues.Settings = settings.Object;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(5);
            GlobalReference.GlobalValues.Random = random.Object;


            npc.FinishLoad();
            Assert.AreEqual(8, npc.CharismaStat);
        }

        [TestMethod]
        public void NonPlayerCharacter_LevelRandomStat_PointsMoreThan100()
        {
            //we use reflection to find the method and test it in isolation so we can verify the level points are done properly
            MethodInfo mi = npc.GetType().GetMethod("LevelRandomStat", BindingFlags.Instance | BindingFlags.NonPublic);
            npc.LevelPoints = 20;

            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6)).Returns(0);
            GlobalReference.GlobalValues.Random = random.Object;


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
            Mock<IArmor> armor = new Mock<IArmor>();
            npc.AddEquipment(armor.Object);

            npc.FinishLoad();

            Assert.AreEqual(0, npc.NpcEquipedEquipment.Count);
        }

        [TestMethod]
        public void NonPlayerCharacter_Die()
        {
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            Mock<IRoom> room = new Mock<IRoom>();
            List<INonPlayerCharacter> npcList = new List<INonPlayerCharacter>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IEvent> evnt = new Mock<IEvent>();

            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0");
            npcList.Add(npc);
            room.Setup(e => e.NonPlayerCharacters).Returns(npcList);
            npc.Room = room.Object;
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;

            npc.Die();

            room.Verify(e => e.RemoveMobileObjectFromRoom(npc));
            room.Verify(e => e.AddItemToRoom(It.IsAny<IItem>(), 0));
            evnt.Verify(e => e.OnDeath(npc));

        }

        [TestMethod]
        public void NonPlayerCharacter_UpdateFollowerToCurrentReference()
        {
            Mock<IRoom> room = new Mock<IRoom>();
            List<INonPlayerCharacter> npcList = new List<INonPlayerCharacter>();
            room.Setup(e => e.NonPlayerCharacters).Returns(npcList);
            npcList.Add(npc);
            npc.Room = room.Object;

            NonPlayerCharacter target = new NonPlayerCharacter();
            target.Id = 2;
            npcList.Add(target);

            NonPlayerCharacter follower = new NonPlayerCharacter();
            follower.Id = 2;
            npc.FollowTarget = follower;

            npc.FinishLoad();

            Assert.AreSame(target, npc.FollowTarget);
        }
    }
}
