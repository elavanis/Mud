using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Custom.EnchantersTower;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.EnchantersTower
{
    [TestClass]
    public class FollowPlayerAfterRescueUnitTest
    {
        FollowPlayerAfterRescue follow;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> guard;
        Mock<IPlayerCharacter> pc;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            guard = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();

            npc.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { guard.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            guard.Setup(e => e.Id).Returns(11);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "pc" });

            follow = new FollowPlayerAfterRescue();
        }

        [TestMethod]
        public void FollowPlayerAfterRescue_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, follow.Process(npc.Object, command));
        }

        [TestMethod]
        public void FollowPlayerAfterRescue_Process_GuardPresent()
        {
            Assert.IsNull(follow.Process(npc.Object, null));
            npc.Verify(e => e.EnqueueCommand(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void FollowPlayerAfterRescue_Process_NoGuard()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());

            Assert.IsNull(follow.Process(npc.Object, null));
            npc.Verify(e => e.EnqueueCommand("Follow pc"));
        }
    }
}
