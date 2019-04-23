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
        LocalKneeBreaker kneeBreaker;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            target.Setup(e => e.Stamina).Returns(10);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            kneeBreaker = new LocalKneeBreaker();
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
            kneeBreaker.Testable_AdditionalEffect(performer.Object, target.Object);

            target.VerifySet(e => e.Stamina = 5);
        }



        public class LocalKneeBreaker : KneeBreaker
        {
            public void Testable_AdditionalEffect(IMobileObject performer, IMobileObject target)
            {
                base.AdditionalEffect(performer, target);
            }
        }
    }
}
