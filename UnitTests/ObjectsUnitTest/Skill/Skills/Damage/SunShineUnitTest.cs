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
    public class SunShineUnitTest
    {
        SunShine sunShine;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(60)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            sunShine = new SunShine();
        }

        [TestMethod]
        public void SunShine_TeachMethod()
        {
            string result = sunShine.TeachMessage;

            Assert.AreEqual("Trick your opponent to lower their guard and make them pay for it.", result);
        }
    }
}
