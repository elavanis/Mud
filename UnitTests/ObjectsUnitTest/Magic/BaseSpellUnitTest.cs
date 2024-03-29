﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Magic;
using Moq;
using Objects.Global.Language.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using System.Collections.Generic;
using Shared.TagWrapper.Interface;
using Objects.Effect.Interface;
using Objects.Global.Notify.Interface;
using Objects.Room.Interface;
using Objects.Language.Interface;
using Objects.Global.StringManuplation.Interface;
using static Shared.TagWrapper.TagWrapper;

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
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

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
            stringManipulator = new Mock<IStringManipulator>();
            translationMessageRoom.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("roomNotify");
            translationMessageTarget.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("targetNotify");
            translationMessagePerformer.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("performNotify");
           
            npc.Setup(e => e.Mana).Returns(2);
            npc.Setup(e => e.Room).Returns(room.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            effectParameter.Setup(e => e.Target).Returns(npc.Object);
            stringManipulator.Setup(e => e.UpdateTargetPerformer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((string x, string y, string z) => z);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            spell = new UnitTestSpell();
            spell.Effect = effect.Object;
            spell.ManaCost = 1;
            spell.RoomNotificationSuccess = translationMessageRoom.Object;
            spell.TargetNotificationSuccess = translationMessageTarget.Object;
            spell.PerformerNotificationSuccess = translationMessagePerformer.Object;
            spell.Parameter = effectParameter.Object;
        }

        [TestMethod]
        public void BaseSpell_Constructor()
        {
            Assert.AreEqual("unitTestSpell", spell.SpellName);
            Assert.AreEqual(1, spell.ManaCost);
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
            Assert.AreEqual("You need 1 mana to cast the spell param0.", result.ResultMessage);
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
            public UnitTestSpell() : base("unitTestSpell", 1)
            {
            }
        }
    }
}
