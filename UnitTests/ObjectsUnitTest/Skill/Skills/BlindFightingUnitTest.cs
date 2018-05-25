using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Skill.Skills;

namespace ObjectsUnitTest.Skill.Skills
{
    [TestClass]
    public class BlindFightingUnitTest
    {
        BlindFighting blindFighting;

        [TestInitialize]
        public void Setup()
        {
            blindFighting = new BlindFighting();
        }

        [TestMethod]
        public void BlindFighting_TeachMessage()
        {
            string expected = "The best way to learn to fight blind is to wear a blindfold while training.";
            Assert.AreEqual(expected, blindFighting.TeachMessage);
        }
    }
}
