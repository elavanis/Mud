using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Damage;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class KneeBreakerUnitTest
    {
        KneeBreaker kneeBreaker;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            kneeBreaker = new KneeBreaker();
        }

        [TestMethod]
        public void KneeBreaker_TeachMessage()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, kneeBreaker.TeachMessage);
        }

        [TestMethod]
        public void KneeBreaker_AdditioanlEffect()
        {
            string expected = "Kicking the opponents knee will cause them to loose mobility.";
            Assert.AreEqual(expected, kneeBreaker.TeachMessage);
        }
    }
}
