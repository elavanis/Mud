using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Custom.GrandviewCastle;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.Custom.GrandviewCastle
{
    [TestClass]
    public class CookUnitTest
    {
        Cook cook;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();

            npc.Setup(e => e.Room).Returns(room.Object);
            npc.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
        }

        [TestMethod]
        public void Cook_Process_CommandNotNull()
        {
            cook = new Cook(0);
            string command = "original";

            Assert.AreSame(command, cook.Process(npc.Object, command));
        }

        [TestMethod]
        public void Cook_Proces_0()
        {
            cook = new Cook(0);

            cook.Process(npc.Object, null);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(7));
            npc.Verify(e => e.EnqueueCommand("Say Right away."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote begins to cut up a rabbit."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote puts the rabbit in the pot."), Times.Once);
        }

        [TestMethod]
        public void Cook_Proces_1()
        {
            cook = new Cook(1);

            cook.Process(npc.Object, null);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(5));
            npc.Verify(e => e.EnqueueCommand("Say As the king wishes."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote begins to cut up a potatoes."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote puts the potatoes in the pot."), Times.Once);
        }

        [TestMethod]
        public void Cook_Proces_2()
        {
            cook = new Cook(2);

            cook.Process(npc.Object, null);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(6));
            npc.Verify(e => e.EnqueueCommand("Say Sure."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote begins to cut up a carrots."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote puts the carrots in the pot."), Times.Once);
        }

        [TestMethod]
        public void Cook_Proces_3()
        {
            cook = new Cook(3);

            cook.Process(npc.Object, null);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(20));
            npc.Verify(e => e.EnqueueCommand("Say As the king wishes."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote stokes the cooking fire."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Emote stirs the pot."), Times.Exactly(4));
            npc.Verify(e => e.EnqueueCommand("Say here you go.  Hasenpfeffer for the king."), Times.Once);
        }
    }
}
