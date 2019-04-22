using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Generic;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Generic
{

    [TestClass]
    public class SingleTargetSkillUnitTest
    {
        Mock<SingleTargetSkill> singleTargetSkill;
        Mock<INonPlayerCharacter> npc;
        Mock<INonPlayerCharacter> npc2;
        Mock<ICommand> command;
        Mock<IFindObjects> findObjects;
        Mock<IParameter> parameter0;
        Mock<IParameter> parameter1;
        Mock<ITagWrapper> tagWrapper;
        Mock<IEffect> effect;
        Mock<IStringManipulator> stringManipulator;
        Mock<ITranslationMessage> translationMessage;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            singleTargetSkill = new Mock<SingleTargetSkill>("abc");
            npc = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            command = new Mock<ICommand>();
            findObjects = new Mock<IFindObjects>();
            parameter0 = new Mock<IParameter>();
            parameter1 = new Mock<IParameter>();
            tagWrapper = new Mock<ITagWrapper>();
            effect = new Mock<IEffect>();
            stringManipulator = new Mock<IStringManipulator>();
            translationMessage = new Mock<ITranslationMessage>();

            singleTargetSkill.CallBase = true;
            singleTargetSkill.Object.Effect = effect.Object;
            singleTargetSkill.Object.PerformerNotificationSuccess = translationMessage.Object;
            npc.Setup(e => e.SentenceDescription).Returns("npc");
            npc2.Setup(e => e.SentenceDescription).Returns("npc2");
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            parameter1.Setup(e => e.ParameterValue).Returns("param1");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(npc.Object, parameter1.Object.ParameterValue, 0, true, true, true, true, true)).Returns(npc2.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object, parameter1.Object });
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            stringManipulator.Setup(e => e.UpdateTargetPerformer("npc", "npc2", "success")).Returns("updated");
            translationMessage.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("success");


            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;
        }

        [TestMethod]
        public void SingleTargetSkill_ProcessSkill_NotEnoughParameters()
        {
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });

            IResult result = singleTargetSkill.Object.ProcessSkill(npc.Object, command.Object);
            Assert.AreEqual("The skill param0 requires a target.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSkill_ProcessSkill_NotFound()
        {
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(npc.Object, parameter1.Object.ParameterValue, 0, true, true, true, true, true)).Returns((IBaseObject)null);

            IResult result = singleTargetSkill.Object.ProcessSkill(npc.Object, command.Object);
            Assert.AreEqual("Unable to find param1.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSkill_ProcessSkill_Found()
        {
            IResult result = singleTargetSkill.Object.ProcessSkill(npc.Object, command.Object);
            Assert.AreEqual("updated", result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
            effect.Verify(e => e.ProcessEffect(singleTargetSkill.Object.Parameter));
            stringManipulator.Verify(e => e.UpdateTargetPerformer("npc", "npc2", "success"));
        }
    }
}
