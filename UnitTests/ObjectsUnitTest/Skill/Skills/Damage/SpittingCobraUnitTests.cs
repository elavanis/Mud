using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Skill.Skills.Damage;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class SpittingCobraUnitTests
    {
        SpittingCobra spittingCobra;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(30)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            spittingCobra = new SpittingCobra();
        }

        [TestMethod]
        public void SpittingCobra_TeachMessage()
        {
            string expected = "Sometimes when fighting we look to animals for inspiration.  Put this pill in your mouth.  Crush it and spit its contents in your opponent's face like a spitting cobra.";
            Assert.AreEqual(expected, spittingCobra.TeachMessage);
        }

        [TestMethod]
        public void SpittingCobra_AdditionalEffect()
        {
            throw new NotImplementedException();
        }
    }
}
