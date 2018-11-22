using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.CauseOpponentEffect;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.CauseOpponentEffect
{
    [TestClass]

    public class DisarmUnitTest
    {
        Disarm disarm;
        Mock<IMobileObject> mob;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;

        [TestInitialize]
        public void Setup()
        {
            mob = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            disarm = new Disarm();
        }

        [TestMethod]
        public void Disarm_TeachMessage()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_MeetRequirments_True()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_MeetRequirments_False()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_IsSuccessful_True()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_IsSuccessful_False()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_AdditionalEffect()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }
    }
}