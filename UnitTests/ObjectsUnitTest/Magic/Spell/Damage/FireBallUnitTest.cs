using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Magic.Spell.Damage;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Damage
{
    [TestClass]
    public class FireballUnitTest
    {
        FireBall fireball;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(70)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            fireball = new FireBall();
        }

        [TestMethod]
        public void FireBall()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(70), Times.Exactly(2));
            Assert.AreEqual("You thrust both hands toward {target} and say {SpellName}.  A fireball leaps through the air and grows until it lands on {target}.", fireball.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} thrust both hands toward {target} and says {SpellName}.  A giant ball of fire flies through the air towards {target} engulfing them in fire.", fireball.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} thrust both hands toward you and says {SpellName}.  A giant ball of fire flies through the air towards you.", fireball.TargetNotificationSuccess.Message);
        }
    }
}

