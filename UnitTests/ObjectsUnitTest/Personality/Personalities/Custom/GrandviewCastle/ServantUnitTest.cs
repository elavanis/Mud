using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.Settings.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Custom.GrandviewCastle;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Personality.Personalities.Custom.GrandviewCastle
{
    [TestClass]
    public class ServantUnitTest
    {
        Servant servant;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        Mock<IFindObjects> findObjects;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<IRandom> random;
        Mock<ISettings> settings;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            findObjects = new Mock<IFindObjects>();
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            random = new Mock<IRandom>();
            settings = new Mock<ISettings>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();

            npc.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.ZoneId).Returns(24);
            room.Setup(e => e.Id).Returns(22);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "queens guard")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "King")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            defaultValues.Setup(e => e.DiceForArmorLevel(45)).Returns(dice.Object);
            random.Setup(e => e.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
            settings.Setup(e => e.BaseStatValue).Returns(5);
            moneyToCoins.Setup(e => e.FormatedAsCoins(It.IsAny<ulong>())).Returns("some");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;

            servant = new Servant();
        }

        [TestMethod]
        public void Servant_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, servant.Process(npc.Object, command));
        }

        [TestMethod]
        public void Servant_Process_InCombat()
        {
            npc.Setup(e => e.IsInCombat).Returns(true);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Servant_Process_Wait()
        {
            State = "Wait";
            npc.SetupSequence(e => e.DequeueMessage())
              .Returns("<Communication>King says Servant, bring me my meal.</Communication>")
              .Returns(null);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("AskedWhatWanted", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(4));
            npc.Verify(e => e.EnqueueCommand("Emote bows."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Say Your Honorable Majestic Majesty Graciousness, what would you like to eat?"), Times.Once);
        }

        [TestMethod]
        public void Servant_Process_AskedWhatWanted()
        {
            State = "AskedWhatWanted";
            npc.SetupSequence(e => e.DequeueMessage())
              .Returns("<Communication>King says Bring me hasenpfeffer.</Communication>")
              .Returns(null);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("KingToldHasenpfeffer", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(3));
            npc.Verify(e => e.EnqueueCommand("Say Right away Your Honorable Royal Majestic Graciousness."), Times.Once);
        }

        [TestMethod]
        public void Servant_Process_KingToldHasenpfeffer()
        {
            State = "KingToldHasenpfeffer";

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("OnWayToKitchen", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(3));
            npc.Verify(e => e.EnqueueCommand("East"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Down"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("North"), Times.Once);
        }

        [TestMethod]
        public void Servant_Process_OnWayToKitchen_Full()
        {
            State = "OnWayToKitchen";
            room.Setup(e => e.Id).Returns(19);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "cook")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual("Say The King wants hasenpfeffer to eat.", result);
            Assert.AreEqual("AskCookForHasenpfeffer", State);
        }

        [TestMethod]
        public void Servant_Process_OnWayToKitchen_Empty()
        {
            State = "OnWayToKitchen";
            Step = 0;
            room.Setup(e => e.Id).Returns(19);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "cook")).Returns(new List<INonPlayerCharacter>());

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(0, Step);
            Assert.AreEqual(null, result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt1()
        {
            State = "EmptyKitchen";
            Step = 0;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(1, Step);
            Assert.AreEqual("Say Hello?", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt2()
        {
            State = "EmptyKitchen";
            Step = 4;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(5, Step);
            Assert.AreEqual("Say Is there anyone here?", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt3()
        {
            State = "EmptyKitchen";
            Step = 9;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(10, Step);
            Assert.AreEqual("Say Great how am I going to make hasenpfeffer?", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt4()
        {
            State = "EmptyKitchen";
            Step = 14;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(15, Step);
            Assert.AreEqual("Emote scurries around the kitchen looking for something to give the King.", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt5()
        {
            State = "EmptyKitchen";
            Step = 19;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(20, Step);
            Assert.AreEqual("Emote scurries around the kitchen looking for something to give the King.", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt6()
        {
            State = "EmptyKitchen";
            Step = 24;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(25, Step);
            Assert.AreEqual("Say Ah Ha!", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt7()
        {
            State = "EmptyKitchen";
            Step = 29;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(30, Step);
            Assert.AreEqual("Say This carrot will work.", result);
            Assert.AreEqual("EmptyKitchen", State);
        }

        [TestMethod]
        public void Servant_Process_EmptyKitchenPt8()
        {
            State = "EmptyKitchen";
            Step = 34;

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(35, Step);
            Assert.AreEqual(null, result);
            Assert.AreEqual("GiveToKingCarrot", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(3));
            npc.Verify(e => e.EnqueueCommand("South"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Up"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("West"), Times.Once);
        }


        [TestMethod]
        public void Servant_Process_AskCookForHasenpfeffer()
        {
            State = "AskCookForHasenpfeffer";
            npc.SetupSequence(e => e.DequeueMessage())
                .Returns("<Communication>Cook says here you go. Hasenpfeffer for the King.</Communication>")
                .Returns(null);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("GiveToKingHasenpfeffer", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(3));
            npc.Verify(e => e.EnqueueCommand("South"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Up"), Times.Once);
            npc.Verify(e => e.EnqueueCommand("West"), Times.Once);
        }

        [TestMethod]
        public void Servant_Process_GiveToKingHasenpfeffer()
        {
            State = "GiveToKingHasenpfeffer";
            room.Setup(e => e.Id).Returns(21);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "King")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual("Bon appetit Most Gracious Majesty.", result);
            Assert.AreEqual("Wait", State);
        }

        [TestMethod]
        public void Servant_Process_GiveToKingCarrot()
        {
            State = "GiveToKingCarrot";
            room.Setup(e => e.Id).Returns(21);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "King")).Returns(new List<INonPlayerCharacter>() { npc.Object });

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual("Your hasenpfeffer Your Magisty.", result);
            Assert.AreEqual("Wait", State);
        }

        private string State
        {
            get
            {
                PropertyInfo stateMachine = servant.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                return stateMachine.GetValue(servant).ToString();
            }
            set
            {
                PropertyInfo stateMachine = servant.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                object state = servant.GetType().GetNestedType("State", BindingFlags.NonPublic).GetField(value).GetValue(servant);
                stateMachine.SetValue(servant, state);
            }
        }

        private int Step
        {
            get
            {
                FieldInfo field = servant.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                return (int)field.GetValue(servant);
            }
            set
            {
                FieldInfo field = servant.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(servant, value);
            }
        }


    }
}