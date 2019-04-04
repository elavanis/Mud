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

            defaultValues.Setup(e => e.DiceForSpellLevel(90)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            rot = new Rot();
        }

        [TestMethod]
        public void Rot()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(90), Times.Exactly(2));

            Assert.AreEqual("Waving your hand across your arm spots of rotting flesh begin to form.  Going back and your arm is healthy.  Suddenly {target} cries out in pain.  A cursory glance shows them a pale color with spots of decay.", rot.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} waves his hand back and forth across his arm causing boils and rotting flesh to appear and disappear.  As the afflictions leave {performer} they appear on {target}.", rot.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} waves his hand back and forth across his arm causing decay to first appear on his arm then disappear and reappear on you.", rot.TargetNotificationSuccess.Message);
        }
    }
}
