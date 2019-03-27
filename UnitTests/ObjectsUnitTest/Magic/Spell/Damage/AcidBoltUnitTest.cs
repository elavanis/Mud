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
    public class AcidBoltUnitTest
    {
        AcidBolt acidBolt;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(20)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            acidBolt = new AcidBolt();
        }

        [TestMethod]
        public void AcidBolt()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(20), Times.Exactly(2));
            Assert.AreEqual("Acid leaps from you hands and lands on {target} burning their skin.", acidBolt.PerformerNotificationSuccess.Message);
            Assert.AreEqual("Acid leaps from {performer} arms and lands {target} burning their skin.", acidBolt.RoomNotificationSuccess.Message);
            Assert.AreEqual("Acid leaps from {performer} arms and lands on you burning your skin.", acidBolt.TargetNotificationSuccess.Message);
        }
    }
}
