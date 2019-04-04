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
    public class PoisonBreathUnitTest
    {
        PoisonBreath poisonBreath;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(60)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            poisonBreath = new PoisonBreath();
        }

        [TestMethod]
        public void PoisonBreath()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(60), Times.Exactly(2));
            Assert.AreEqual("Like a blur you rush {target} and pop a pill of poison gas in your mouth.  With a quick bite you blow the gas in {target} face leaving them choking on the poisonous gas.", poisonBreath.PerformerNotificationSuccess.Message);
            Assert.AreEqual("Like a blur {performer} rushes toward {target} as they blow a cloud of green gas in their face.", poisonBreath.RoomNotificationSuccess.Message);
            Assert.AreEqual("One second you are fighting {performer} at arms length and the next they are in your face.  with a slightly evil grin they blow a cloud of noxious gas in your face leaving you to choke on the fumes.", poisonBreath.TargetNotificationSuccess.Message);
        }
    }
}
