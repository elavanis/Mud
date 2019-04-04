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
    public class PyschicScreamUnitTest
    {
        PyschicScream pyschicScream;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(50)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            pyschicScream = new PyschicScream();
        }

        [TestMethod]
        public void PyschicScreamBreath()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(50), Times.Exactly(2));
            Assert.AreEqual("Closing your eyes you and using your minds voice you scream at {target}.", pyschicScream.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} closes their eyes and {target} begins to scream in terror covering their ears.", pyschicScream.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} closes their eyes.  Suddenly you hear the sound screaming as if from a banshee.", pyschicScream.TargetNotificationSuccess.Message);
        }
    }
}
