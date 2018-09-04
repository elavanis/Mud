using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Command.PC;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;
using System.Linq;
using Objects.Skill.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class PerformUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("(P)erform [Skill Name] {Parameter(s)}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Perform();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
        }

        [TestMethod]
        public void Perform_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Perform_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("P"));
            Assert.IsTrue(result.Contains("Perform"));
        }

        [TestMethod]
        public void Perform_PerformCommand_NoParameter()
        {
            tagWrapper.Setup(e => e.WrapInTag("What skill would you like to use?", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Perform_PerformCommand_UnknownSkill()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Dictionary<string, ISkill> skills = new Dictionary<string, ISkill>();

            tagWrapper.Setup(e => e.WrapInTag("You do not know that skill.", TagType.Info)).Returns("message");
            parameter.Setup(e => e.ParameterValue).Returns("skill");
            mob.Setup(e => e.KnownSkills).Returns(skills);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Perform_PerformCommand_KnownSkill()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Dictionary<string, ISkill> skills = new Dictionary<string, ISkill>();
            Mock<ISkill> skill = new Mock<ISkill>();
            Mock<IEvent> evnt = new Mock<IEvent>();
            Mock<IEngine> engine = new Mock<IEngine>();
            Mock<IResult> mockResult = new Mock<IResult>();

            tagWrapper.Setup(e => e.WrapInTag("You do not know that skill.", TagType.Info)).Returns("message");
            parameter.Setup(e => e.ParameterValue).Returns("skill");
            skills.Add("SKILL", skill.Object);
            skill.Setup(e => e.ProcessSkill(mob.Object, mockCommand.Object)).Returns(mockResult.Object);
            skill.Setup(e => e.ToString()).Returns("skill");
            mob.Setup(e => e.KnownSkills).Returns(skills);
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.AreSame(mockResult.Object, result);
            evnt.Verify(e => e.Perform(mob.Object, "skill"), Times.Once);
        }
    }
}
