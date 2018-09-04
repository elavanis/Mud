using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic;
using Moq;
using Objects.Global.Language.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using System.Collections.Generic;
using Shared.TagWrapper.Interface;
using Shared.TagWrapper;
using Objects.Effect.Interface;
using Objects.Global.Notify.Interface;
using Objects.Room.Interface;
using static Objects.Global.Language.Translator;
using Objects.Language.Interface;

namespace ObjectsUnitTest.Magic
{
    [TestClass]
    public class BaseSpellUnitTest
    {
        UnitTestSpell spell;
        Mock<INonPlayerCharacter> npc;
        Mock<ICommand> command;
        Mock<IParameter> parameter0;
        Mock<ITagWrapper> tagWrapper;
        Mock<IEffect> effect;
        Mock<INotify> notify;
        Mock<IRoom> room;
        Mock<IEffectParameter> effectParameter;
        Mock<ITranslationMessage> translationMessageRoom;
        Mock<ITranslationMessage> translationMessageTarget;
        Mock<ITranslationMessage> translationMessagePerformer;

        [TestInitialize]
        public void Setup()
        {
            spell = new UnitTestSpell();
            npc = new Mock<INonPlayerCharacter>();
            command = new Mock<ICommand>();
            parameter0 = new Mock<IParameter>();
            tagWrapper = new Mock<ITagWrapper>();
            effect = new Mock<IEffect>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();
            effectParameter = new Mock<IEffectParameter>();
            translationMessageRoom = new Mock<ITranslationMessage>();
            translationMessageTarget = new Mock<ITranslationMessage>();
            translationMessagePerformer = new Mock<ITranslationMessage>();

            translationMessageRoom.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("roomNotify");
            translationMessageTarget.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("targetNotify");
            translationMessagePerformer.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("performNotify");
            spell.Effect = effect.Object;
            spell.ManaCost = 1;
            spell.RoomNotification = translationMessageRoom.Object;
            spell.TargetNotification = translationMessageTarget.Object;
            spell.PerformerNotification = translationMessagePerformer.Object;
            spell.Parameter = effectParameter.Object;
            npc.Setup(e => e.Mana).Returns(2);
            npc.Setup(e => e.Room).Returns(room.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            tagWrapper.Setup(e => e.WrapInTag("You need 1 mana to cast the spell param0.", TagWrapper.TagType.Info)).Returns("NotEnoughMana");
            tagWrapper.Setup(e => e.WrapInTag("roomNotify", TagWrapper.TagType.Info)).Returns("room");
            tagWrapper.Setup(e => e.WrapInTag("targetNotify", TagWrapper.TagType.Info)).Returns("target");
            tagWrapper.Setup(e => e.WrapInTag("performNotify", TagWrapper.TagType.Info)).Returns("perform");
            effectParameter.Setup(e => e.Target).Returns(npc.Object);




            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void BaseSpell_TeachMessage()
        {
            Mock<ITranslator> translator = new Mock<ITranslator>();
            translator.Setup(e => e.Translate(Objects.Global.Language.Translator.Languages.Magic, "UnitTestSpell")).Returns("UnitTestSpell");
            GlobalReference.GlobalValues.Translator = translator.Object;

            spell.SpellName = "UnitTestSpell";

            string result = spell.TeachMessage;
            Assert.AreEqual("Repeat after me.  UnitTestSpell", result);
        }

        [TestMethod]
        public void BaseSpell_ProcessSkill_NotEnoughMana()
        {
            npc.Setup(e => e.Mana).Returns(0);
            IResult result = spell.ProcessSpell(npc.Object, command.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("NotEnoughMana", result.ResultMessage);
        }

        [TestMethod]
        public void BaseSpell_ProcessSkill_Processed()
        {
            IResult result = spell.ProcessSpell(npc.Object, command.Object);

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("performNotify", result.ResultMessage);
            notify.Verify(e => e.Room(npc.Object, npc.Object, room.Object, translationMessageRoom.Object, new List<IMobileObject>() { npc.Object }, false, false), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, npc.Object, npc.Object, translationMessageTarget.Object, false, false), Times.Once);
        }

        private class UnitTestSpell : BaseSpell
        {

        }
    }
}
