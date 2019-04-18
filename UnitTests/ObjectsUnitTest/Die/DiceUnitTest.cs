using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die;
using Objects.Global;
using Objects.Global.Random.Interface;

namespace ObjectsUnitTest.Die
{
    [TestClass]
    public class DiceUnitTest
    {
        Dice dice;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            dice = new Dice(2, 1);
        }

        [TestMethod]
        public void Dice_Constructor()
        {
            Assert.AreEqual(2, dice.Die);
            Assert.AreEqual(1, dice.Sides);
        }

        [TestMethod]
        public void Dice_RollDice()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(1)).Returns(0);
            GlobalReference.GlobalValues.Random = random.Object;

            Assert.AreEqual(2, dice.RollDice());
        }
    }
}
