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
        Mock<INonPlayerCharacter> otherNpc1;
        Mock<INonPlayerCharacter> otherNpc2;
        Mock<INonPlayerCharacter> otherNpc3;
        Mock<INonPlayerCharacter> otherNpc4;
        Mock<INonPlayerCharacter> otherNpc5;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            otherNpc1 = new Mock<INonPlayerCharacter>();
            otherNpc2 = new Mock<INonPlayerCharacter>();
            otherNpc3 = new Mock<INonPlayerCharacter>();
            otherNpc4 = new Mock<INonPlayerCharacter>();
            otherNpc5 = new Mock<INonPlayerCharacter>();

            npc.Setup(e => e.Room).Returns(room.Object);
            otherNpc1.Setup(e => e.Room).Returns(room.Object);
            otherNpc2.Setup(e => e.Room).Returns(room.Object);
            otherNpc3.Setup(e => e.Room).Returns(room.Object);
            otherNpc4.Setup(e => e.Room).Returns(room.Object);
            otherNpc5.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { otherNpc1.Object, otherNpc2.Object, otherNpc3.Object, otherNpc4.Object, otherNpc5.Object, npc.Object });

            cook = new Cook();


            //PropertyInfo stateMachine = cook.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
            //object state = cook.GetType().GetNestedType("State", BindingFlags.NonPublic).GetField("AskedForHasenpfeffer").GetValue(cook);
            //stateMachine.SetValue(cook, state);

        }

        [TestMethod]
        public void Cook_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, cook.Process(npc.Object, command));
        }

        [TestMethod]
        public void Cook_Process_CheckForTrigger()
        {
            otherNpc1.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");
            otherNpc2.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");
            otherNpc3.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");
            otherNpc4.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");
            otherNpc5.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>");

            cook.Process(otherNpc1.Object, null);
            otherNpc1.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(7));
            otherNpc1.Verify(e => e.EnqueueCommand("Say Right away."), Times.Once);
            otherNpc1.Verify(e => e.EnqueueCommand("Emote begins to cut up a rabbit."), Times.Once);
            otherNpc1.Verify(e => e.EnqueueCommand("Emote puts the rabbit in the pot."), Times.Once);

            cook.Process(otherNpc2.Object, null);
            otherNpc2.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(5));
            otherNpc2.Verify(e => e.EnqueueCommand("Say As the king wishes."), Times.Once);
            otherNpc2.Verify(e => e.EnqueueCommand("Emote begins to cut up a potatoes."), Times.Once);
            otherNpc2.Verify(e => e.EnqueueCommand("Emote puts the potatoes in the pot."), Times.Once);

            cook.Process(otherNpc3.Object, null);
            otherNpc3.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(6));
            otherNpc3.Verify(e => e.EnqueueCommand("Say Sure."), Times.Once);
            otherNpc3.Verify(e => e.EnqueueCommand("Emote begins to cut up a carrots."), Times.Once);
            otherNpc3.Verify(e => e.EnqueueCommand("Emote puts the carrots in the pot."), Times.Once);

            cook.Process(otherNpc4.Object, null);
            otherNpc4.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(20));
            otherNpc4.Verify(e => e.EnqueueCommand("Say As the king wishes."), Times.Once);
            otherNpc4.Verify(e => e.EnqueueCommand("Emote stokes the cooking fire."), Times.Once);
            otherNpc4.Verify(e => e.EnqueueCommand("Emote stirs the pot."), Times.Exactly(4));
            otherNpc4.Verify(e => e.EnqueueCommand("Say here you go.  Hasenpfeffer for the king."), Times.Once);

            otherNpc5.Verify(e => e.EnqueueCommand(It.IsAny<string>()), Times.Never);

        }
    }
}
