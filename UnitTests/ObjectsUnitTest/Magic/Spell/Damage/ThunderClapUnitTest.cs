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
    public class ThunderClapUnitTest
    {
        ThunderClap thunderClap;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(10)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            thunderClap = new ThunderClap();
        }

        [TestMethod]
        public void ThunderClap()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(10), Times.Exactly(2));
            Assert.AreEqual("You clap your hands together at {target} and a large crack like thunder shakes them to the bone.", thunderClap.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} claps their hands at {target} and a large crack of thunder fills the air.", thunderClap.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} claps their hands in front of them the loud crack of thunder rattles your bones.", thunderClap.TargetNotificationSuccess.Message);
        }
    }
}