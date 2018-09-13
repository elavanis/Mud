using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Random;
using Objects.Global.Random.Interface;
using Objects.Global.Settings.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;

namespace ObjectsUnitTest.Global.Random
{
    [TestClass]
    public class RandomDropGeneratorUnitTest
    {
        RandomDropGenerator randomDropGenerator;
        Mock<IDefaultValues> defaultValues;
        Mock<IRandom> random;
        Mock<INonPlayerCharacter> npc;
        Mock<ISettings> settings;

        [TestInitialize]
        public void Setup()
        {
            randomDropGenerator = new RandomDropGenerator();
            defaultValues = new Mock<IDefaultValues>();
            random = new Mock<IRandom>();
            npc = new Mock<INonPlayerCharacter>();
            settings = new Mock<ISettings>();

            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(new Dice(1, 1));
            defaultValues.Setup(e => e.DiceForWeaponLevel(2)).Returns(new Dice(2, 2));
            random.Setup(e => e.Next(It.IsAny<int>())).Returns(0);
            npc.Setup(e => e.TypeOfMob).Returns(MobType.Humanoid);
            npc.Setup(e => e.Level).Returns(1);
            settings.Setup(e => e.OddsOfGeneratingRandomDrop).Returns(1);
            settings.Setup(e => e.OddsOfDropBeingPlusOne).Returns(2);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_NoRandomDrops()
        {
            settings.Setup(e => e.OddsOfGeneratingRandomDrop).Returns(0);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_NoRandomDropCreated()
        {
            settings.Setup(e => e.OddsOfGeneratingRandomDrop).Returns(1);
            random.Setup(e => e.Next(1)).Returns(1);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_Other()
        {
            npc.Setup(e => e.TypeOfMob).Returns(MobType.Other);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_Humanoid()
        {
            settings.Setup(e => e.OddsOfDropBeingPlusOne).Returns(0);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_HumanoidPlusOneItem()
        {
            random.SetupSequence(e => e.Next(2))
                .Returns(0)
                .Returns(1);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            IWeapon weapon = result as IWeapon;
            Assert.IsNotNull(weapon);
            Assert.AreEqual(1, weapon.Level);
            Assert.AreEqual(2, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(2, weapon.DamageList[0].Dice.Sides);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Club()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Club);
            Assert.AreEqual("The club has been worn smooth with several large indentions.  There surly a story for each one but hopefully you were the story teller and not the receiving actor.", weapon.ExamineDescription);
            Assert.AreEqual("The club looks to well balanced with a frayed leather grip.", weapon.LookDescription);
            Assert.AreEqual("The stout wooden club looks to be well balanced.", weapon.ShortDescription);
            Assert.AreEqual("club", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Club"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Bludgeon, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Mace()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Mace);
            Assert.AreEqual("The head of the mace is a round ball.", weapon.ExamineDescription);
            Assert.AreEqual("The shaft of the mace is smooth and the head of the polished.", weapon.LookDescription);
            Assert.AreEqual("The metal mace has an ornate head used for bashing things.", weapon.ShortDescription);
            Assert.AreEqual("mace", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Mace"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Bludgeon, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_WizardStaff()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.WizardStaff);
            Assert.AreEqual("The wooden staff is constantly in flux as small leaves grow out from the staff, blossom crimson flowers and then wilt and are reabsorbed into the staff.", weapon.ExamineDescription);
            Assert.AreEqual("The wooden staff has gnarled fingers for a head.", weapon.LookDescription);
            Assert.AreEqual("The wizards staff hums with a deep sound that resonates deep in your body.", weapon.ShortDescription);
            Assert.AreEqual("wizard staff", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Wizard"));
            Assert.IsTrue(weapon.KeyWords.Contains("Staff"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Bludgeon, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Axe()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Axe);
            Assert.AreEqual("The blade is carved runes and made of iron.", weapon.ExamineDescription);
            Assert.AreEqual("The axe could have been used by a great warrior of days or the local peasant down the road.  It is hard tell the history just from its looks.", weapon.LookDescription);
            Assert.AreEqual("The axe has a large head and strong wooden handle.", weapon.ShortDescription);
            Assert.AreEqual("axe", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Axe"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Slash, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Sword()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Sword);
            Assert.AreEqual("The blade is made from steal.  The guard is shaped like a pair of wings and the handle is wrapped in white silk.  There is a dragon claw holding a amber stone for a pommel.", weapon.ExamineDescription);
            Assert.AreEqual("The blade is pitted and has one side sharpened.", weapon.LookDescription);
            Assert.AreEqual("A short sword used to cut down ones foes.", weapon.ShortDescription);
            Assert.AreEqual("sword", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Sword"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Slash, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Dagger()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Dagger);
            Assert.AreEqual("The blade is made from steal.  The handle is wrapped in white silk and there is a small knights helmet for a pommel.", weapon.ExamineDescription);
            Assert.AreEqual("The blade is pitted and has a small fuller running the length of the blade.", weapon.LookDescription);
            Assert.AreEqual("The dagger is short sharp and pointy.  Perfect for concealing on your person.", weapon.ShortDescription);
            Assert.AreEqual("dagger", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Dagger"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Pierce, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Pick()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Pick);
            Assert.AreEqual("The head of the war pick is polished smooth and shines slightly.", weapon.ExamineDescription);
            Assert.AreEqual("This pick has a large grooved hammer head and a sharp pick on the back.", weapon.LookDescription);
            Assert.AreEqual("This war pick is a versatile weapon used to fight against armored opponents.", weapon.ShortDescription);
            Assert.AreEqual("war pick", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("War"));
            Assert.IsTrue(weapon.KeyWords.Contains("Pick"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Pierce, weapon.DamageList[0].Type);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomWeapon_Spear()
        {
            IWeapon weapon = randomDropGenerator.GenerateRandomWeapon(1, 1, WeaponType.Spear);
            Assert.AreEqual("The spear head is made of flint.", weapon.ExamineDescription);
            Assert.AreEqual("The spear head is pointed and about nine inches long.", weapon.LookDescription);
            Assert.AreEqual("A large pointed spear that can be used to poke holes in ones foes or pick up trash.", weapon.ShortDescription);
            Assert.AreEqual("spear", weapon.SentenceDescription);
            Assert.IsTrue(weapon.KeyWords.Contains("Spear"));
            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Die);
            Assert.AreEqual(1, weapon.DamageList[0].Dice.Sides);
            Assert.AreEqual(DamageType.Pierce, weapon.DamageList[0].Type);
        }


        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor()
        {
            random.SetupSequence(e => e.Next(12)).Returns(5)
                                               .Returns(5);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);



        }

    }
}
