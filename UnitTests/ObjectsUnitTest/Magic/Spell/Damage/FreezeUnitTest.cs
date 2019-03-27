using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Language;
using Objects.Global.Language.Interface;
using Objects.Magic.Spell.Damage;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Damage
{
    [TestClass]
    public class FreezeUnitTest
    {
        Freeze freeze;
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

            defaultValues.Setup(e => e.DiceForSpellLevel(40)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            translator.Setup(e => e.Translate(Translator.Languages.AncientMagic, It.IsAny<string>())).Returns((Translator.Languages x, string y) => (y));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Translator = translator.Object;

            freeze = new Freeze();
        }

        [TestMethod]
        public void Freeze()
        {
            string incantation = GlobalReference.GlobalValues.Translator.Translate(Translator.Languages.AncientMagic, nameof(Freeze));
            defaultValues.Verify(e => e.DiceForSpellLevel(40), Times.Exactly(2));
            Assert.AreEqual("Speaking " + incantation + " your hands become cold and numb.  You blow across your hand causing {target} to be engulfed in a blizzard.", freeze.PerformerNotificationSuccess.Message);
            Assert.AreEqual("{performer} speaks the words " + incantation + " before a blizzard at {target}.", freeze.RoomNotificationSuccess.Message);
            Assert.AreEqual("{performer} speaks " + incantation + " and blows a blizzard at you.", freeze.TargetNotificationSuccess.Message);
        }
    }
}