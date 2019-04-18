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
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;
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
            GlobalReference.GlobalValues = new GlobalValues();

            randomDropGenerator = new RandomDropGenerator();
            defaultValues = new Mock<IDefaultValues>();
            random = new Mock<IRandom>();
            npc = new Mock<INonPlayerCharacter>();
            settings = new Mock<ISettings>();

            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(new Dice(1, 1));
            defaultValues.Setup(e => e.DiceForWeaponLevel(2)).Returns(new Dice(2, 2));
            random.Setup(e => e.Next(It.IsAny<int>())).Returns(0);
            random.Setup(e => e.PercentDiceRoll(1)).Returns(true);
            random.Setup(e => e.PercentDiceRoll(0)).Returns(true);
            npc.Setup(e => e.TypeOfMob).Returns(MobType.Humanoid);
            npc.Setup(e => e.Level).Returns(1);
            settings.Setup(e => e.RandomDropPercent).Returns(1);
            settings.Setup(e => e.DropBeingPlusOnePercent).Returns(2);
            settings.Setup(e => e.MaxLevel).Returns(107);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_NoRandomDrops()
        {
            settings.Setup(e => e.RandomDropPercent).Returns(0);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_NoRandomDropCreated()
        {
            settings.Setup(e => e.RandomDropPercent).Returns(0);

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
            settings.Setup(e => e.DropBeingPlusOnePercent).Returns(0);

            IItem result = randomDropGenerator.GenerateRandomDrop(npc.Object);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomDrop_HumanoidPlusOneItem()
        {
            settings.SetupSequence(e => e.DropBeingPlusOnePercent).Returns(1).Returns(0);

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
        public void RandomDropGenerator_GenerateRandomArmor_Head()
        {
            random.Setup(e => e.Next(12)).Returns(1);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("The helmet has two small holes cut out for seeing out.", armor.ExamineDescription);
            Assert.AreEqual("The helmet is hard and light but well padded giving the ultimate compromise between protection and usability.", armor.LookDescription);
            Assert.AreEqual("A well made helmet that looks like it might fit.", armor.ShortDescription);
            Assert.AreEqual("helmet", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Helmet"));
            Assert.AreEqual(AvalableItemPosition.Head, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Necklace()
        {
            random.Setup(e => e.Next(12)).Returns(2);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("A black stone rests softly in the middle of the necklace.", armor.ExamineDescription);
            Assert.AreEqual("The necklace has a stone attached to it via a round pendent.", armor.LookDescription);
            Assert.AreEqual("A delicate necklace fit for any royal lady to wear to any party.", armor.ShortDescription);
            Assert.AreEqual("necklace", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Necklace"));
            Assert.AreEqual(AvalableItemPosition.Neck, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Arms()
        {
            random.Setup(e => e.Next(12)).Returns(3);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("The bracer is made of strips of material held together with leather.", armor.ExamineDescription);
            Assert.AreEqual("Just a hair longer than your arm these bracers look to be a perfect fit.", armor.LookDescription);
            Assert.AreEqual("A pair of bracers that look to offer good protection for your arms.", armor.ShortDescription);
            Assert.AreEqual("bracer", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Bracer"));
            Assert.AreEqual(AvalableItemPosition.Arms, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Hand()
        {
            random.Setup(e => e.Next(12)).Returns(4);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("Made of a thin material these gloves have a magical property to them that grants the wearer protection.", armor.ExamineDescription);
            Assert.AreEqual("The gloves have a spider web design on the back and a spider for the design on the inside.", armor.LookDescription);
            Assert.AreEqual("The gloves look to be thin and not offer much protection.", armor.ShortDescription);
            Assert.AreEqual("gloves", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Gloves"));
            Assert.AreEqual(AvalableItemPosition.Hand, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Finger()
        {
            random.Setup(e => e.Next(12)).Returns(5);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("The ring once had a design on the inside but has been worn smooth with time.", armor.ExamineDescription);
            Assert.AreEqual("The ring is smooth on the outside.", armor.LookDescription);
            Assert.AreEqual("The ring is a simple ring with no special markings or anything to suggest it is magical.", armor.ShortDescription);
            Assert.AreEqual("ring", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Ring"));
            Assert.AreEqual(AvalableItemPosition.Finger, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Body()
        {
            random.Setup(e => e.Next(12)).Returns(6);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("There is a large emblem on the front of a tree.", armor.ExamineDescription);
            Assert.AreEqual("The breast plate is hard giving the wearer plenty of protection while being light.", armor.LookDescription);
            Assert.AreEqual("A strong breast plate that has a small dent in the left side but otherwise is in perfect condition.", armor.ShortDescription);
            Assert.AreEqual("breast plate", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("breast"));
            Assert.IsTrue(armor.KeyWords.Contains("plate"));
            Assert.IsTrue(armor.KeyWords.Contains("breastplate"));
            Assert.AreEqual(AvalableItemPosition.Body, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Waist()
        {
            random.Setup(e => e.Next(12)).Returns(7);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("The belt is made of an unknown material that shifts colors through all the colors of the rainbow.", armor.ExamineDescription);
            Assert.AreEqual("The belt is prismatic.  The color shifts through the rainbow as you move relative to it.", armor.LookDescription);
            Assert.AreEqual("The belt is a prismatic color that shifts wildly.", armor.ShortDescription);
            Assert.AreEqual("belt", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Belt"));
            Assert.AreEqual(AvalableItemPosition.Waist, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Legs()
        {
            random.Setup(e => e.Next(12)).Returns(8);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("The pattern on the leggings produce a soft blue glow.", armor.ExamineDescription);
            Assert.AreEqual("The leggings have are a dark gray color with delicately carved curving lines on the front forming a intricate pattern.", armor.LookDescription);
            Assert.AreEqual("A pair of leggings.", armor.ShortDescription);
            Assert.AreEqual("legging", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Legging"));
            Assert.AreEqual(AvalableItemPosition.Legs, armor.ItemPosition);
        }

        [TestMethod]
        public void RandomDropGenerator_GenerateRandomArmor_Feet()
        {
            random.Setup(e => e.Next(12)).Returns(9);

            IArmor armor = randomDropGenerator.GenerateRandomArmor(1, 1);

            Assert.AreEqual("Three pouches hang off the outside of each boot allowing you to have quick access to small items.", armor.ExamineDescription);
            Assert.AreEqual("Made of supple leather the boots are soft and easy to wear at the expense of some protection.", armor.LookDescription);
            Assert.AreEqual("A pair of leather boots.", armor.ShortDescription);
            Assert.AreEqual("boot", armor.SentenceDescription);
            Assert.IsTrue(armor.KeyWords.Contains("Boot"));
            Assert.AreEqual(AvalableItemPosition.Feet, armor.ItemPosition);
        }
    }
}
