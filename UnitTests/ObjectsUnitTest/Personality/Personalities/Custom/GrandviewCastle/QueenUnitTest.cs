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
    public class QueenUnitTest
    {
        Queen queen;
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
        //Mock<IPlayerCharacter> player;
        //Mock<ICanMobDoSomething> canMobDoSomething;
        //List<IPlayerCharacter> players;

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
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "queens guard")).Returns(new List<INonPlayerCharacter>() { npc.Object });
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

            queen = new Queen();
            GreetedKing = true;
        }

        [TestMethod]
        public void Queen_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, queen.Process(npc.Object, command));
        }

        [TestMethod]
        public void Queen_Process_InCombat()
        {
            npc.Setup(e => e.IsInCombat).Returns(true);

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Flee", result);
            npc.Verify(e => e.EnqueueCommand("Say GUARDS!"), Times.Once);
            room.Verify(e => e.Enter(It.IsAny<INonPlayerCharacter>()), Times.Exactly(3));
        }

        [TestMethod]
        public void Queen_Process_GreetKing()
        {
            room.Setup(e => e.Id).Returns(21);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "king")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            GreetedKing = false;

            string result = queen.Process(npc.Object, null);

            Assert.IsTrue(GreetedKing);
            Assert.AreEqual("Good morning honey.", result);
        }

        [TestMethod]
        public void Queen_Process_Sleep1()
        {
            State = "Sleep";
            Step = 0;

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Say Make the sun go away.  I officially decree it.  Go away.", result);
            Assert.AreEqual(1, Step);
        }

        [TestMethod]
        public void Queen_Process_Sleep5()
        {
            State = "Sleep";
            Step = 4;

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Emote rolls out of bed.", result);
            Assert.AreEqual(5, Step);
        }

        [TestMethod]
        public void Queen_Process_Sleep10()
        {
            State = "Sleep";
            Step = 9;

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Stand", result);
            Assert.AreEqual(10, Step);
        }

        [TestMethod]
        public void Queen_Process_Sleep12()
        {
            State = "Sleep";
            Step = 11;

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Emote drinks coffee.", result);
            Assert.AreEqual(12, Step);
        }

        [TestMethod]
        public void Queen_Process_Sleep14()
        {
            State = "Sleep";
            Step = 13;

            string result = queen.Process(npc.Object, null);

            Assert.AreEqual("Say Much better.", result);
            Assert.AreEqual(14, Step);
            Assert.AreEqual("Up", State);
        }

        [TestMethod]
        public void QueenUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }


        private string State
        {
            get
            {
                PropertyInfo stateMachine = queen.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                return stateMachine.GetValue(queen).ToString();
            }
            set
            {
                PropertyInfo stateMachine = queen.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                object state = queen.GetType().GetNestedType("State", BindingFlags.NonPublic).GetField(value).GetValue(queen);
                stateMachine.SetValue(queen, state);
            }
        }

        private int Step
        {
            get
            {
                FieldInfo field = queen.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                return (int)field.GetValue(queen);
            }
            set
            {
                FieldInfo field = queen.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(queen, value);
            }
        }


        private bool GreetedKing
        {
            get
            {
                PropertyInfo propertyInfo = queen.GetType().GetProperty("GreetedKing", BindingFlags.Instance | BindingFlags.NonPublic);
                return (bool)propertyInfo.GetValue(queen);
            }
            set
            {
                PropertyInfo propertyInfo = queen.GetType().GetProperty("GreetedKing", BindingFlags.Instance | BindingFlags.NonPublic);
                propertyInfo.SetValue(queen, value);
            }

        }
    }
}