using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Magic.Spell.Damage;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Damage
{
    [TestClass]
    public class RotUnitTest
    {
        Rot rot;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(70)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            rot = new Rot();
        }

        [TestMethod]
        public void Rot()
        {
            Assert.AreEqual("You thrust both hands toward {target} and say {SpellName}.  A fireball leaps through the air and grows until it lands on {target}.", rot.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} thrust both hands toward {target} and says {SpellName}.  A giant ball of fire flies through the air towards {target} engulfing them in fire.", rot.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} thrust both hands toward you and says {SpellName}.  A giant ball of fire flies through the air towards you.", rot.TargetNotificationSuccess.Message);
        }
    }
}
