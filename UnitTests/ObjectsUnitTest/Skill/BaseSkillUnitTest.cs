using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.Notify.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Skill;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

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
        Mock<ITranslationMessage> translationMessageRoomSuccess;
        Mock<ITranslationMessage> translationMessageTargetSuccess;
        Mock<ITranslationMessage> translationMessagePerformerSuccess;
        Mock<ITranslationMessage> translationMessageRoomFailure;
        Mock<ITranslationMessage> translationMessageTargetFailure;
        Mock<ITranslationMessage> translationMessagePerformerFailure;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            baseSkill = new UnitTestSkill();
            npc = new Mock<INonPlayerCharacter>();
            command = new Mock<ICommand>();
            parameter0 = new Mock<IParameter>();
            tagWrapper = new Mock<ITagWrapper>();
            effect = new Mock<IEffect>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();
            effectParameter = new Mock<IEffectParameter>();
            translationMessageRoomSuccess = new Mock<ITranslationMessage>();
            translationMessageTargetSuccess = new Mock<ITranslationMessage>();
            translationMessagePerformerSuccess = new Mock<ITranslationMessage>();
            translationMessageRoomFailure = new Mock<ITranslationMessage>();
            translationMessageTargetFailure = new Mock<ITranslationMessage>();
            translationMessagePerformerFailure = new Mock<ITranslationMessage>();
            stringManipulator = new Mock<IStringManipulator>();

            translationMessageRoomSuccess.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("roomNotifySuccess");
            translationMessageTargetSuccess.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("targetNotifySuccess");
            translationMessagePerformerSuccess.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("performNotifySuccess");
            translationMessageRoomFailure.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("roomNotifyFailure");
            translationMessageTargetFailure.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("targetNotifyFailure");
            translationMessagePerformerFailure.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("performNotifyFailure");
            baseSkill.Effect = effect.Object;
            baseSkill.StaminaCost = 1;
            baseSkill.RoomNotificationSuccess = translationMessageRoomSuccess.Object;
            baseSkill.TargetNotificationSuccess = translationMessageTargetSuccess.Object;
            baseSkill.PerformerNotificationSuccess = translationMessagePerformerSuccess.Object;
            baseSkill.RoomNotificationFailure = translationMessageRoomFailure.Object;
            baseSkill.TargetNotificationFailure = translationMessageTargetFailure.Object;
            baseSkill.PerformerNotificationFailure = translationMessagePerformerFailure.Object;

            baseSkill.Parameter = effectParameter.Object;
            npc.Setup(e => e.Stamina).Returns(2);
            npc.Setup(e => e.Room).Returns(room.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            stringManipulator.Setup(e => e.UpdateTargetPerformer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b, string c) => (c));
            effectParameter.Setup(e => e.Target).Returns(npc.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;
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
            Assert.AreEqual("You need 1 stamina to use the skill param0.", result.ResultMessage);
        }

        [TestMethod]
        public void BaseSkill_ProcessSkill_Successful()
        {
            IResult result = baseSkill.ProcessSkill(npc.Object, command.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("performNotifySuccess", result.ResultMessage);
            notify.Verify(e => e.Room(npc.Object, npc.Object, room.Object, translationMessageRoomSuccess.Object, new List<IMobileObject>() { npc.Object }, false, false), Times.Once);
        }

        [TestMethod]
        public void BaseSkill_ProcessSkill_Unsucessful()
        {
            baseSkill.Successful = false;
            IResult result = baseSkill.ProcessSkill(npc.Object, command.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("performNotifyFailure", result.ResultMessage);
            notify.Verify(e => e.Room(npc.Object, npc.Object, room.Object, translationMessageRoomFailure.Object, new List<IMobileObject>() { npc.Object }, false, false), Times.Once);
        }

        private class UnitTestSkill : BaseSkill
        {
            public bool Successful { get; set; } = true;

            public UnitTestSkill() : base("unitTestSkill")
            {

            }

            public override bool IsSuccessful(IMobileObject performer, IMobileObject target)
            {
                return Successful;
            }

        }
    }
}
