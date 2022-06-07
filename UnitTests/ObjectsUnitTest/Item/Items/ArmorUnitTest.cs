using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Item.Items;
using Objects.Material.Interface;
using static Objects.Damage.Damage;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class ArmorUnitTest
    {
        Armor armor;
        Mock<IDice> dice;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
            dice = new Mock<IDice>();
            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
         
            defaultValues.Setup(e=>e.DiceForArmorLevel(1)).Returns(dice.Object);

            armor = new Armor(dice.Object, Equipment.AvalableItemPosition.Wield, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
        }

        [TestMethod]
        public void Constructor_Dice()
        {
            Assert.AreSame(dice, armor.Dice);
            Assert.AreSame("examineDescription", armor.ExamineDescription);
            Assert.AreSame("lookDescription", armor.LookDescription);
            Assert.AreSame("sentenceDescription", armor.SentenceDescription);
            Assert.AreSame("shortDescription", armor.ShortDescription);
        }

        [TestMethod]
        public void Constructor_Level()
        {
            armor = new Armor(1, Equipment.AvalableItemPosition.Wield, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");

            Assert.AreEqual(1, armor.Level);
            Assert.AreSame(dice, armor.Dice);
            Assert.AreSame("examineDescription", armor.ExamineDescription);
            Assert.AreSame("lookDescription", armor.LookDescription);
            Assert.AreSame("sentenceDescription", armor.SentenceDescription);
            Assert.AreSame("shortDescription", armor.ShortDescription);
        }

        [TestMethod]
        public void Armor_Acid()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Acid);
        }

        [TestMethod]
        public void Armor_Bludgeon()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Bludgeon);
        }

        [TestMethod]
        public void Armor_Cold()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Cold);
        }

        [TestMethod]
        public void Armor_Fire()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Fire);
        }

        [TestMethod]
        public void Armor_Force()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Force);
        }

        [TestMethod]
        public void Armor_Lightning()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Lightning);
        }

        [TestMethod]
        public void Armor_Necrotic()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Necrotic);
        }

        [TestMethod]
        public void Armor_Pierce()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Pierce);
        }

        [TestMethod]
        public void Armor_Poison()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Poison);
        }

        [TestMethod]
        public void Armor_Psychic()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Psychic);
        }

        [TestMethod]
        public void Armor_Radiant()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Radiant);
        }

        [TestMethod]
        public void Armor_Slash()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Slash);
        }

        [TestMethod]
        public void Armor_Thunder()
        {
            Assert.AreEqual(decimal.MaxValue, armor.Thunder);
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Acid()
        {
            armor.Acid = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Acid));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Bludgeon()
        {
            armor.Bludgeon = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Bludgeon));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Cold()
        {
            armor.Cold = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Cold));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Fire()
        {
            armor.Fire = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Fire));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Force()
        {
            armor.Force = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Force));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Lightning()
        {
            armor.Lightning = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Lightning));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Necrotic()
        {
            armor.Necrotic = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Necrotic));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Pierce()
        {
            armor.Pierce = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Pierce));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Poison()
        {
            armor.Poison = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Poison));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Psychic()
        {
            armor.Psychic = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Psychic));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Radiant()
        {
            armor.Radiant = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Radiant));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Slash()
        {
            armor.Slash = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Slash));
        }

        [TestMethod]
        public void Armor_GetTypeModifier_Thunder()
        {
            armor.Thunder = 1;
            Assert.AreEqual(1, armor.GetTypeModifier(DamageType.Thunder));
        }

        [TestMethod]
        public void Armor_FinishLoad_MatrialSet()
        {
            Mock<IMaterial> material = new Mock<IMaterial>();
            material.Setup(e => e.Acid).Returns(1);
            material.Setup(e => e.Bludgeon).Returns(2);
            material.Setup(e => e.Cold).Returns(3);
            material.Setup(e => e.Fire).Returns(4);
            material.Setup(e => e.Force).Returns(5);
            material.Setup(e => e.Lightning).Returns(6);
            material.Setup(e => e.Necrotic).Returns(7);
            material.Setup(e => e.Pierce).Returns(8);
            material.Setup(e => e.Poison).Returns(9);
            material.Setup(e => e.Psychic).Returns(10);
            material.Setup(e => e.Radiant).Returns(11);
            material.Setup(e => e.Slash).Returns(12);
            material.Setup(e => e.Thunder).Returns(13);
            armor.Material = material.Object;

            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(2);
            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            defaultValues.Setup(e => e.MoneyForNpcLevel(1)).Returns(1);
            defaultValues.Setup(e => e.DiceForArmorLevel(0)).Returns(dice.Object);
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            armor.FinishLoad();

            Assert.AreEqual(1, armor.Acid);
            Assert.AreEqual(2, armor.Bludgeon);
            Assert.AreEqual(3, armor.Cold);
            Assert.AreEqual(4, armor.Fire);
            Assert.AreEqual(5, armor.Force);
            Assert.AreEqual(6, armor.Lightning);
            Assert.AreEqual(7, armor.Necrotic);
            Assert.AreEqual(8, armor.Pierce);
            Assert.AreEqual(9, armor.Poison);
            Assert.AreEqual(10, armor.Psychic);
            Assert.AreEqual(11, armor.Radiant);
            Assert.AreEqual(12, armor.Slash);
            Assert.AreEqual(13, armor.Thunder);
        }

        [TestMethod]
        public void Armor_FinishLoad_DiceNull()
        {
            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(2);

            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            defaultValues.Setup(e => e.DiceForArmorLevel(0)).Returns(dice.Object);
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            armor.FinishLoad();

            Assert.AreEqual(dice.Object.Die, armor.Dice.Die);
            Assert.AreEqual(dice.Object.Sides, armor.Dice.Sides);
        }
    }
}
