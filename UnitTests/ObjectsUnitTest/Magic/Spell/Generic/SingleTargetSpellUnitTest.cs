using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.FindObjects.Interface;
using Objects.Interface;
using Objects.Language.Interface;
using Objects.Magic.Spell.Generic;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Generic
{
    [TestClass]
    public class SingleTargetSpellUnitTest
    {
        SingleTargetSpell singleTargetSpell;
        Mock<INonPlayerCharacter> npc;
        Mock<INonPlayerCharacter> npc2;
        Mock<ICommand> command;
        Mock<IFindObjects> findObjects;
        Mock<IParameter> parameter0;
        Mock<IParameter> parameter1;
        Mock<ITagWrapper> tagWrapper;
        Mock<IEffect> effect;
        Mock<ITranslationMessage> translationMessage;

        [TestInitialize]
        public void Setup()
        {
            singleTargetSpell = new SingleTargetSpell();
            npc = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            command = new Mock<ICommand>();
            findObjects = new Mock<IFindObjects>();
            parameter0 = new Mock<IParameter>();
            parameter1 = new Mock<IParameter>();
            tagWrapper = new Mock<ITagWrapper>();
            effect = new Mock<IEffect>();
            translationMessage = new Mock<ITranslationMessage>();

            singleTargetSpell.Effect = effect.Object;
            singleTargetSpell.PerformerNotificationSuccess = translationMessage.Object;
            parameter0.Setup(e => e.ParameterValue).Returns("param0");
            parameter1.Setup(e => e.ParameterValue).Returns("param1");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(npc.Object, parameter1.Object.ParameterValue, 0, true, true, true, true, true)).Returns(npc2.Object);
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object, parameter1.Object });
            tagWrapper.Setup(e => e.WrapInTag("The spell param0 requires a target.", TagType.Info)).Returns("notEnoughParams");
            tagWrapper.Setup(e => e.WrapInTag("Unable to find param1.", TagType.Info)).Returns("notFound");
            tagWrapper.Setup(e => e.WrapInTag("Unable to find an opponent to cast the spell on.", TagType.Info)).Returns("failure");
            translationMessage.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("success");

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

        }

        [TestMethod]
        public void SingleTargetSpell_ProcessSpell_NotEnoughParameters()
        {
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });

            IResult result = singleTargetSpell.ProcessSpell(npc.Object, command.Object);
            Assert.AreEqual("notEnoughParams", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSpell_ProcessSpell_NotFound()
        {
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(npc.Object, parameter1.Object.ParameterValue, 0, true, true, true, true, true)).Returns((IBaseObject)null);

            IResult result = singleTargetSpell.ProcessSpell(npc.Object, command.Object);
            Assert.AreEqual("notFound", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSpell_ProcessSpell_Found()
        {
            IResult result = singleTargetSpell.ProcessSpell(npc.Object, command.Object);
            Assert.AreEqual("success", result.ResultMessage);
             Assert.IsFalse(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSpell_ProcessSpell_InCombat()
        {
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            npc.Setup(e => e.IsInCombat).Returns(true);
            npc.Setup(e => e.Opponent).Returns(npc2.Object);

            IResult result = singleTargetSpell.ProcessSpell(npc.Object, command.Object);
            Assert.AreEqual("success", result.ResultMessage);
             Assert.IsFalse(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void SingleTargetSpell_ProcessSpell_InCombatNoOpponet()
        {
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter0.Object });
            npc.Setup(e => e.IsInCombat).Returns(true);

            IResult result = singleTargetSpell.ProcessSpell(npc.Object, command.Object);
            Assert.AreEqual("failure", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }
    }
}
