using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Language;
using Objects.Global.Language.Interface;
using Objects.Magic.Spell.Damage;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Damage
{
    [TestClass]
    public class LightningBoltUnitTest
    {
        LightningBolt lightningBolt;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<ITranslator> translator;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            translator = new Mock<ITranslator>();

            defaultValues.Setup(e => e.DiceForSpellLevel(30)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            translator.Setup(e => e.Translate(Translator.Languages.AncientMagic, It.IsAny<string>())).Returns((Translator.Languages x, string y) => (y));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Translator = translator.Object;

            lightningBolt = new LightningBolt();
        }

        [TestMethod]
        public void LightningBolt()
        {
            string incantation = GlobalReference.GlobalValues.Translator.Translate(Translator.Languages.AncientMagic, nameof(LightningBolt));

            defaultValues.Verify(e => e.DiceForSpellLevel(30), Times.Exactly(2));
            Assert.AreEqual("Speaking " + incantation + " a small storm cloud appears before you sending a bolt of lighting toward {target}.", lightningBolt.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} says " + incantation + " and a small storm cloud appears sending a bolt of lighting toward {target}.", lightningBolt.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} says " + incantation + " and a small storm cloud appears sending a bolt of lighting toward you.", lightningBolt.TargetNotificationSuccess.Message);
        }
    }
}
