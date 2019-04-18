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
    public class LightBurstUnitTest
    {
        LightBurst lightBurst;
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

            defaultValues.Setup(e => e.DiceForSpellLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            lightBurst = new LightBurst();
        }

        [TestMethod]
        public void LightBurst()
        {
            defaultValues.Verify(e => e.DiceForSpellLevel(80), Times.Exactly(2));
            Assert.AreEqual("Circling your hands around an imaginary sphere you push out.  Turning away the imaginary sphere burst into light blinding {target}.", lightBurst.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} swirls their hands around an imaginary sphere.  They push the sphere towards {target} and quickly before light white blinding burst forth filling the room.", lightBurst.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} swirls their hands around an imaginary sphere and pushes it towards you while quickly turning away.  A moment later bright light burst forth.", lightBurst.TargetNotificationSuccess.Message);
        }
    }
}
