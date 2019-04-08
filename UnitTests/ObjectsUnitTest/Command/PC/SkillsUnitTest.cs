using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Skill.Interface;
using System.Text;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SkillsUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<ISkill> skill1;
        Mock<ISkill> skill2;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Skills();
            skill1 = new Mock<ISkill>();
            skill2 = new Mock<ISkill>();
            Dictionary<string, ISkill> skills = new Dictionary<string, ISkill>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            skill1.Setup(e => e.AbilityName).Returns("ABC");
            skill1.Setup(e => e.StaminaCost).Returns(1);
            skill2.Setup(e => e.AbilityName).Returns("AHHHHH");
            skill2.Setup(e => e.StaminaCost).Returns(2);
            skills.Add(skill1.Object.AbilityName, skill1.Object);
            skills.Add(skill2.Object.AbilityName, skill2.Object);

            mob.Setup(e => e.KnownSkills).Returns(skills);
        }

        [TestMethod]
        public void Skills_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Skills", result.ResultMessage);
        }

        [TestMethod]
        public void Skills_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Skills"));
        }

        [TestMethod]
        public void Skills_PerformCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Skill   Stamina Cost");
            stringBuilder.AppendLine("ABC     1");
            stringBuilder.AppendLine("AHHHHH  2");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(stringBuilder.ToString().Trim(), result.ResultMessage);
        }
    }
}
