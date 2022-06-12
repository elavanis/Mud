using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Item.Items;
using static Objects.Item.Items.Equipment;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class ShieldUnitTest
    {
        Shield shield;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();

            defaultValues.Setup(e=>e.DiceForArmorLevel(1)).Returns(dice.Object);    
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
        }

        [TestMethod]
        public void Shield_Constructor_Level()
        {
            shield = new Shield(1, AvalableItemPosition.Wield, "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
            Assert.AreEqual(1, shield.Level);
            Assert.AreSame(dice, shield.Dice);
            Assert.AreEqual(Equipment.AvalableItemPosition.Wield, shield.ItemPosition);
            Assert.AreEqual("examineDescription", shield.ExamineDescription);
            Assert.AreEqual("lookDescription", shield.LookDescription);
            Assert.AreEqual("sentenceDescription", shield.SentenceDescription);
            Assert.AreEqual("shortDescription", shield.ShortDescription);
        }
    }
}
