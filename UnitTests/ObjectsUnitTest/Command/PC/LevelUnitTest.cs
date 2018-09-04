using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Personality.Interface;
using static Objects.Room.Room;
using Objects.Global.Engine.Interface;
using System.Linq;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class LevelUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;

        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Level [Stat] {Amount To Level}", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            command = new Level();
        }

        [TestMethod]
        public void Level_Instructions()
        {
            IResult result = command.Instructions;

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Level_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Level"));
        }

        [TestMethod]
        public void Learn_PerformCommand_NoParameter()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            tagWrapper.Setup(e => e.WrapInTag("You have 5 points to spend.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Learn_PerformCommand_NoLevelPoints()
        {
            mob.Setup(e => e.LevelPoints).Returns(0);
            Mock<IParameter> parameter = new Mock<IParameter>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            tagWrapper.Setup(e => e.WrapInTag("You have no points to spend at this time.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Learn_PerformCommand_NotEnoughLevelPoints()
        {
            mob.Setup(e => e.LevelPoints).Returns(1);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("cha");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You do not have that many points to spend.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Learn_PerformCommand_Strength()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("str");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel stronger.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.StrengthStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_Dexterity()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("dex");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel more agile.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.DexterityStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_Constitution()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("con");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel healthier.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.ConstitutionStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_Intelligence()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("int");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel smarter.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.IntelligenceStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_Wisdom()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("wis");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel wiser.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.WisdomStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_Charisma()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("cha");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("You feel better looking.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            mob.VerifySet(e => e.CharismaStat = 5);
            mob.VerifySet(e => e.LevelPoints = 0);
        }

        [TestMethod]
        public void Learn_PerformCommand_NotValidStat()
        {
            mob.Setup(e => e.LevelPoints).Returns(5);
            Mock<IParameter> parameter = new Mock<IParameter>();
            parameter.Setup(e => e.ParameterValue).Returns("bug");
            Mock<IParameter> parameter2 = new Mock<IParameter>();
            parameter2.Setup(e => e.ParameterValue).Returns("5");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object, parameter2.Object });
            tagWrapper.Setup(e => e.WrapInTag("Unsure which stat to increase.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
