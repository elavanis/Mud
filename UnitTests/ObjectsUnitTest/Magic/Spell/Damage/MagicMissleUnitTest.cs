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
    public class MagicMissleUnitTest
    {
        MagicMissle magicMissle;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(1)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            magicMissle = new MagicMissle();
        }

        [TestMethod]
        public void MagicMissle()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(1), Times.Exactly(2));
            Assert.AreEqual("With a flick of your wrist three magical darts fly from your hand striking {target}.", magicMissle.PerformerNotificationSuccess.Message);
            Assert.AreEqual("Three magical darts fly from {performer} and strike {target}.", magicMissle.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} makes a gesture with their hand causing three magical darts to strike you in the chest.", magicMissle.TargetNotificationSuccess.Message);
        }
    }
}
