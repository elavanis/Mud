using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Skill;
using Shared.TagWrapper;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ObjectsUnitTest.Skill
{
    [TestClass]
    public class BaseSkillUnitTest
    {
        UnitTestSkill baseSkill;
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
            baseSkill = new UnitTestSkill();
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
            baseSkill.Effect = effect.Object;
            baseSkill.StaminaCost = 1;
            baseSkill.RoomNotification = translationMessageRoom.Object;
            baseSkill.TargetNotification = translationMessageTarget.Object;
            baseSkill.PerformerNotification = translationMessagePerformer.Object;
            baseSkill.Parameter = effectParameter.Object;
            npc.Setup(e => e.Stamina).Returns(2);
            npc.Setup(e => e.Room).Returns(room.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            tagWrapper.Setup(e => e.WrapInTag("You need 1 stamina to use the skill param0.", TagWrapper.TagType.Info)).Returns("NotEnoughStamina");

            effectParameter.Setup(e => e.Target).Returns(npc.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void BaseSkill_TeachMethod()
        {
            Assert.AreEqual("The best way to learn is with lots practice.", baseSkill.TeachMessage);
        }

        [TestMethod]
        public void BaseSkill_ProcessSkill_NotEnoughStamina()
        {
            npc.Setup(e => e.Stamina).Returns(0);
            IResult result = baseSkill.ProcessSkill(npc.Object, command.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("NotEnoughStamina", result.ResultMessage);
        }

        [TestMethod]
        public void BaseSkill_ProcessSkill_Processed()
        {
            IResult result = baseSkill.ProcessSkill(npc.Object, command.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("performNotify", result.ResultMessage);
            notify.Verify(e => e.Room(npc.Object, npc.Object, room.Object, translationMessageRoom.Object, new List<IMobileObject>() { npc.Object }, false, false), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()));
        }

        private class UnitTestSkill : BaseSkill
        {
            public UnitTestSkill() : base("unitTestSkill")
            {

            }
        }
    }
}
