using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die;
using Objects.Effect;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Language;
using Objects.Global.Language.Interface;
using Objects.Magic.Spell.Heal.Health;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Heal.Health
{
    [TestClass]
    public class BaseCureSpellUnitTest
    {
        BaseCureSpell baseCureSpell;
        Mock<IDefaultValues> defaultValues;
        Mock<ITranslator> translator;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            translator = new Mock<ITranslator>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.ReduceValues(1, 100)).Returns(new Dice(10, 5));
            translator.Setup(e => e.Translate(Translator.Languages.Magic, "SpellName")).Returns("magic");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Translator = translator.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            baseCureSpell = new BaseCureSpell("SpellName", 1, 100);
        }

        [TestMethod]
        public void BaseCureSpell_Constructor_DefaultParameters()
        {
            Assert.IsTrue(baseCureSpell.Effect is RecoverHealth);
            Assert.AreEqual(10, baseCureSpell.Parameter.Dice.Die);
            Assert.AreEqual(5, baseCureSpell.Parameter.Dice.Sides);
            Assert.AreEqual("SpellName", baseCureSpell.SpellName);
            Assert.AreEqual(5, baseCureSpell.ManaCost);
        }

        [TestMethod]
        public void BaseCureSpell_Constructor_SpecifiedManaCost()
        {
            baseCureSpell = new BaseCureSpell("SpellName", 1, 100, -2);

            Assert.IsTrue(baseCureSpell.Effect is RecoverHealth);
            Assert.AreEqual(10, baseCureSpell.Parameter.Dice.Die);
            Assert.AreEqual(5, baseCureSpell.Parameter.Dice.Sides);
            Assert.AreEqual("SpellName", baseCureSpell.SpellName);
            Assert.AreEqual(-2, baseCureSpell.ManaCost);
        }
    }
}
