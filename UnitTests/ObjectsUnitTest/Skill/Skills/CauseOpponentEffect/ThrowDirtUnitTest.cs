using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Effect;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Language.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.CauseOpponentEffect;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Objects.Global.Stats.Stats;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.CauseOpponentEffect
{
    [TestClass]

    public class ThrowDirtUnitTest
    {
        LocalThrowDirt throwDirt;
        Mock<IMobileObject> mob;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IStringManipulator> stringManipulator;
        Mock<INotify> notify;
        List<IEnchantment> enchantments;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mob = new Mock<IMobileObject>();
            tagWrapper = new Mock<ITagWrapper>();
            target = new Mock<IMobileObject>();
            performer = new Mock<IMobileObject>();
            stringManipulator = new Mock<IStringManipulator>();
            notify = new Mock<INotify>();
            enchantments = new List<IEnchantment>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            stringManipulator.Setup(e => e.UpdateTargetPerformer(performer.Object.SentenceDescription, target.Object.SentenceDescription, "{performer} throws dirt into your eyes blinding you.")).Returns("message");
            target.Setup(e => e.Enchantments).Returns(enchantments);
            performer.Setup(e => e.StrengthEffective).Returns(100);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;

            throwDirt = new LocalThrowDirt();
        }

        [TestMethod]
        public void ThrowDirt_TeachMessage()
        {
            string expected = "Throw some dirt from the ground in your opponent eyes and they will be blinded.";
            Assert.AreEqual(expected, throwDirt.TeachMessage);
        }

        [TestMethod]
        public void ThrowDirt_AdditionalEffect()
        {
            throwDirt.Testable_AdditionalEffect(performer.Object, target.Object);

            notify.Verify(e => e.Mob(target.Object, It.Is<ITranslationMessage>(f => f.Message == "message")), Times.Once);
            Assert.AreEqual(1, enchantments.Count);
            IEnchantment enchantment = enchantments[0];
            Assert.IsTrue(enchantment is HeartbeatBigTickEnchantment);
            Assert.IsTrue(enchantment.Effect is Blindness);
            Assert.AreEqual(100, enchantment.ActivationPercent);
            Assert.AreEqual(9999, enchantment.EnchantmentEndingDateTime.Year);
            Assert.AreEqual(performer.Object.StrengthEffective, enchantment.DefeatInfo.CurrentEnchantmentPoints);
            Assert.AreEqual(Stat.Constitution, enchantment.DefeatInfo.MobStat);
        }


        public class LocalThrowDirt : ThrowDirt
        {
            public void Testable_AdditionalEffect(IMobileObject performer, IMobileObject target)
            {
                base.AdditionalEffect(performer, target);
            }
        }
    }
}