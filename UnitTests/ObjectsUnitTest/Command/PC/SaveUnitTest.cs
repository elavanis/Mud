﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Shared.TagWrapper.Interface;
using Moq;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Linq;
using Objects.World.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SaveUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();

            room = new Mock<IRoom>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);

            command = new Save();
        }

        [TestMethod]
        public void Save_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Save", result.ResultMessage);
        }

        [TestMethod]
        public void Save_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Save"));
        }

        [TestMethod]
        public void Save_PerformCommand_NotPc()
        {
            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Only PlayerCharacters can save.", result.ResultMessage);
        }

        [TestMethod]
        public void Save_PerformCommand_Saved()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.Room).Returns(room.Object);
            Mock<IWorld> world = new Mock<IWorld>();
            GlobalReference.GlobalValues.World = world.Object;

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Character Saved", result.ResultMessage);
            world.Verify(e => e.SaveCharcter(pc.Object), Times.Once);
            pc.Verify(e => e.RemoveOldCorpses(It.IsAny<DateTime>()));
        }
    }
}
