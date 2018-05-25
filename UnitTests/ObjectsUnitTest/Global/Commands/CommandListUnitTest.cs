using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Commands;
using Objects.Global;
using Moq;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using Objects.Command.God;
using Objects.Command.PC;

namespace ObjectsUnitTest.Global.Commands
{
    [TestClass]
    public class CommandListUnitTest
    {
        CommandList commandList;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues.TagWrapper = new Mock<ITagWrapper>().Object;

            commandList = new CommandList();
        }


        //TODO Add proper unit test, if possible

        /// <summary>
        /// this is not a proper unit test per say, just helps verify we don't have two commands with the same trigger
        /// </summary>
        [TestMethod]
        public void CommandList_GetListOfCommands()
        {
            HashSet<string> hs = new HashSet<string>();
            foreach (string key in commandList.GodCommandsLookup.Keys)
            {
                hs.Add(key);
            }

            foreach (string key in commandList.PcCommandsLookup.Keys)
            {
                hs.Add(key);
            }
        }

        [TestMethod]
        public void CommandList_GetCommand_LReturnsLook()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.God).Returns(false);

            IMobileObjectCommand command = commandList.GetCommand(pc.Object, "L");
            Assert.AreEqual(typeof(Look), command.GetType());
        }

        [TestMethod]
        public void CommandList_GetCommand_NotGodCannotRunGodCommand()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.God).Returns(false);

            IMobileObjectCommand command = commandList.GetCommand(pc.Object, "ITEMSTATS");
            Assert.IsNull(command);
        }

        [TestMethod]
        public void CommandList_GetCommand_GodCanRunGodCommand()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.God).Returns(true);

            IMobileObjectCommand command = commandList.GetCommand(pc.Object, "ITEMSTATS");
            Assert.AreEqual(typeof(ItemStats), command.GetType());
        }

        [TestMethod]
        public void CommandList_GetCommand_FindPcCommand()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();

            IMobileObjectCommand command = commandList.GetCommand(pc.Object, "LOOK");
            Assert.AreEqual(typeof(Look), command.GetType());
        }
    }
}
