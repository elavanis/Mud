using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Global;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Settings.Interface;
using Objects.Mob.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class BugUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IFileIO> fileIO;
        Mock<ISettings> settings;
        Mock<ITime> time;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            fileIO = new Mock<IFileIO>();
            settings = new Mock<ISettings>();
            time = new Mock<ITime>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            mob.Setup(e => e.KeyWords).Returns(new List<string>() { "mob" });
            settings.Setup(e => e.BugDirectory).Returns("bugDirectory");
            time.Setup(e => e.CurrentDateTime).Returns(new DateTime(2000, 1, 1));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.Time = time.Object;

            command = new Bug();
        }

        [TestMethod]
        public void Bug_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Bug [Bug Description]", result.ResultMessage);
        }

        [TestMethod]
        public void Bug_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Bug"));
        }

        [TestMethod]
        public void Bug_PeformCommand_NoParameters()
        {
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Please describe the bug.", result.ResultMessage);
        }

        [TestMethod]
        public void Bug_PeformCommand_Parameters()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            parameter.Setup(e => e.ParameterValue).Returns("stuff");

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("New bug filed.", result.ResultMessage);
            fileIO.Verify(e => e.WriteFile("bugDirectory\\mob - 20000101120000.bug", "stuff"), Times.Once);
        }
    }
}
