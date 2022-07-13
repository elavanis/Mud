using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Skill.Skills;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills
{
    [TestClass]
    public class BlindFightingUnitTest
    {
        BlindFighting blindFighting;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
            
            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
