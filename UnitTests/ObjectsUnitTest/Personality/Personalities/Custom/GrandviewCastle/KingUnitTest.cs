using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.GameDateTime;
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
    public class KingUnitTest
    {
        King king;
        Mock<INonPlayerCharacter> npc;
        Mock<IFindObjects> findObjects;
        Mock<IRoom> room;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<IRandom> random;
        Mock<ISettings> settings;
        Mock<IMoneyToCoins> moneyToCoins;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;
        Mock<IPlayerCharacter> player;
        Mock<ICanMobDoSomething> canMobDoSomething;
        List<IPlayerCharacter> players;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            findObjects = new Mock<IFindObjects>();
            room = new Mock<IRoom>();
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            random = new Mock<IRandom>();
            settings = new Mock<ISettings>();
            moneyToCoins = new Mock<IMoneyToCoins>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            inGameDateTime = new Mock<IInGameDateTime>();
            gameDateTime = new Mock<IGameDateTime>();
            player = new Mock<IPlayerCharacter>();
            canMobDoSomething = new Mock<ICanMobDoSomething>();
            players = new List<IPlayerCharacter>();

            npc.Setup(e => e.Room).Returns(room.Object);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "kings guard")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "queen")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "servant")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.Id).Returns(21);
            room.Setup(e => e.Zone).Returns(24);
            room.Setup(e => e.PlayerCharacters).Returns(players);
            defaultValues.Setup(e => e.DiceForArmorLevel(47)).Returns(dice.Object);
            random.Setup(e => e.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
            settings.Setup(e => e.BaseStatValue).Returns(5);
            moneyToCoins.Setup(e => e.FormatedAsCoins(It.IsAny<ulong>())).Returns("some");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            canMobDoSomething.Setup(e => e.SeeObject(npc.Object, player.Object)).Returns(true);

            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.CanMobDoSomething = canMobDoSomething.Object;

            king = new King();
            SetGreetedQueen(true);
        }

        [TestMethod]
        public void King_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, king.Process(npc.Object, command));
        }

        [TestMethod]
        public void King_Process_InCombat()
        {
            npc.Setup(e => e.IsInCombat).Returns(true);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Flee", result);
            npc.Verify(e => e.EnqueueCommand("Say GUARDS!"), Times.Once);
            room.Verify(e => e.Enter(It.IsAny<INonPlayerCharacter>()), Times.Exactly(3));
        }

        [TestMethod]
        public void King_Process_GreetQueen()
        {
            SetGreetedQueen(false);

            string result = king.Process(npc.Object, null);

            PropertyInfo propertyInfo = king.GetType().GetProperty("GreetedQueen", BindingFlags.Instance | BindingFlags.NonPublic);

            bool greetedQueen = (bool)propertyInfo.GetValue(king);
            Assert.AreEqual("Good morning honey.", result);
        }

        #region Day Tests
        [TestMethod]
        public void King_Process_Day_Sleep1()
        {
            Step = 0;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Emote stretches and yawns loudly.", result);
            Assert.AreEqual(1, Step);
        }

        [TestMethod]
        public void King_Process_Day_Sleep3()
        {
            Step = 2;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Good morning world.", result);
            Assert.AreEqual(3, Step);
        }

        [TestMethod]
        public void King_Process_Day_Sleep4()
        {
            Step = 3;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Stand", result);
            Assert.AreEqual(0, Step);
            Assert.AreEqual("MoveToBathRoom", State);
        }

        [TestMethod]
        public void King_Process_Day_MoveToBathRoom22()
        {
            State = "MoveToBathRoom";
            room.Setup(e => e.Id).Returns(22);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("South", result);
        }

        [TestMethod]
        public void King_Process_Day_MoveToBathRoom24()
        {
            State = "MoveToBathRoom";
            room.Setup(e => e.Id).Returns(24);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("InBathRoom", State);
            Assert.AreEqual(0, Step);
        }

        [TestMethod]
        public void King_Process_Day_InBathRoom1()
        {
            State = "InBathRoom";
            Step = 0;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Emote brushes his teeth.", result);
            Assert.AreEqual(1, Step);
        }

        [TestMethod]
        public void King_Process_Day_InBathRoom3()
        {
            State = "InBathRoom";
            Step = 2;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Emote uses the bathroom.", result);
            Assert.AreEqual(3, Step);
        }

        [TestMethod]
        public void King_Process_Day_InBathRoom5()
        {
            State = "InBathRoom";
            Step = 4;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("North", result);
            Assert.AreEqual(5, Step);
        }

        [TestMethod]
        public void King_Process_Day_InBathRoom6()
        {
            State = "InBathRoom";
            Step = 5;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("East", result);
            Assert.AreEqual("ThroneRoom", State);
        }

        [TestMethod]
        public void King_Process_Day_ThroneRoomFailStep()
        {
            State = "ThroneRoom";
            Step = 1;
            random.Setup(e => e.PercentDiceRoll(50)).Returns(true);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_ThroneRoomFailDice()
        {
            State = "ThroneRoom";
            Step = 4;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_ThroneRoomFailServant()
        {
            State = "ThroneRoom";
            Step = 4;
            random.Setup(e => e.PercentDiceRoll(50)).Returns(true);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "servant")).Returns(new List<INonPlayerCharacter>());

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_ThroneRoomSuccess()
        {
            State = "ThroneRoom";
            Step = 4;
            random.Setup(e => e.PercentDiceRoll(50)).Returns(true);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Servant, bring me my meal.", result);
            Assert.AreEqual(0, Step);
            Assert.AreEqual("AskedForMeal", State);
        }

        [TestMethod]
        public void King_Process_Day_AskedForMealFailStep()
        {
            State = "AskedForMeal";
            Step = 1;
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Kings servant says Your Honorable Majestic Majesty Graciousness, what would you like to eat?</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_AskedForMealFailServant()
        {
            State = "AskedForMeal";
            Step = 4;
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "servant")).Returns(new List<INonPlayerCharacter>());
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Kings servant says Your Honorable Majestic Majesty Graciousness, what would you like to eat?</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_AskedForMealFailMessage()
        {
            State = "AskedForMeal";
            Step = 4;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void King_Process_Day_AskedForMealSuccess()
        {
            State = "AskedForMeal";
            Step = 4;
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Kings servant says Your Honorable Majestic Majesty Graciousness, what would you like to eat?</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual(0, Step);
            Assert.AreEqual("AskedForHasenpfeffer", State);
        }

        [TestMethod]
        public void King_Process_Day_AskedForHasenpfefferMessage1()
        {
            State = "AskedForHasenpfeffer";
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Kings servant says Bon appetit Most Gracious Majesty.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("ReceivedHasenpfeffer", State);
        }

        [TestMethod]
        public void King_Process_Day_AskedForHasenpfefferMessage2()
        {
            State = "AskedForHasenpfeffer";
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Kings servant says Your hasenpfeffer Your Magisty.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual("ReceivedCarrot", State);
        }

        [TestMethod]
        public void King_Process_Day_AskedForHasenpfefferStep1()
        {
            State = "AskedForHasenpfeffer";
            Step = 0;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Bring me hasenpfeffer.", result);
        }

        [TestMethod]
        public void King_Process_Day_AskedForHasenpfefferStep2()
        {
            State = "AskedForHasenpfeffer";
            Step = 19;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Where is my hasenpfeffer?", result);
        }

        [TestMethod]
        public void King_Process_Day_ReceivedHasenpfeffer()
        {
            State = "ReceivedHasenpfeffer";

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Emote eats hasenpfeffer.", result);
            Assert.AreEqual("ThroneRoom", State);
        }

        [TestMethod]
        public void King_Process_Day_ReceivedCarrot_Step1()
        {
            State = "ReceivedCarrot";
            Step = 0;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Emote eats hasenpfeffer.", result);
        }

        [TestMethod]
        public void King_Process_Day_ReceivedCarrot_Step3()
        {
            State = "ReceivedCarrot";
            Step = 2;

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say If I didn't know this was hasenpfeffer I'd swear it was carrots.", result);
            Assert.AreEqual("ThroneRoom", State);
        }
        #endregion Day Tests

        #region Night Tests
        [TestMethod]
        public void King_Process_Night_CloseCourt()
        {
            gameDateTime.Setup(e => e.Hour).Returns(13);
            room.Setup(e => e.Id).Returns(20);

            string result = king.Process(npc.Object, null);

            npc.Verify(e => e.EnqueueCommand("Say Court is closed for the day. Please come back tomorrow."), Times.Once);
            Assert.AreEqual("West", result);
            Assert.AreEqual("SpendTimeWithQueen", State);
        }

        [TestMethod]
        public void King_Process_Night_SummonGuards()
        {
            gameDateTime.Setup(e => e.Hour).Returns(13);
            room.Setup(e => e.Id).Returns(21);
            players.Add(player.Object);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say GUARDS!", result);
            room.Verify(e => e.Enter(It.IsAny<INonPlayerCharacter>()), Times.Exactly(3));
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen1()
        {
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            room.Setup(e => e.Id).Returns(22);

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("North", result);
            Assert.AreEqual(0, Step);
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen2()
        {
            Step = 4;
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            //room.Setup(e => e.Id).Returns(20);
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Queen says Hello dear.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Hello my beautify queen.", result);
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen3()
        {
            Step = 4;
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            //room.Setup(e => e.Id).Returns(20);
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Queen says I wish we could just leave this all behind.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say That sounds nice.  We should take a trip to the country to get away for a while.", result);
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen4()
        {
            Step = 4;
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            //room.Setup(e => e.Id).Returns(20);
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Queen says A trip to the country sounds great  We can goto the villa.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Lets plan to do this when the weather gets a little nicer.", result);
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen5()
        {
            Step = 4;
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            //room.Setup(e => e.Id).Returns(20);
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Queen says Agreed.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Say Good night my love.", result);
        }

        [TestMethod]
        public void King_Process_Night_SpendTimeWithQueeen6()
        {
            Step = 4;
            State = "SpendTimeWithQueen";
            gameDateTime.Setup(e => e.Hour).Returns(13);
            //room.Setup(e => e.Id).Returns(20);
            npc.Setup(e => e.DequeueMessage()).Returns("<Communication>Queen says Goodnight my dear.</Communication>");

            string result = king.Process(npc.Object, null);

            Assert.AreEqual("Sleep", State);
            Assert.AreEqual("South", result);
        }
        #endregion Night Tests

        [TestMethod]
        public void KingUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }


        private string State
        {
            get
            {
                PropertyInfo stateMachine = king.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                return stateMachine.GetValue(king).ToString();
            }
            set
            {
                PropertyInfo stateMachine = king.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
                object state = king.GetType().GetNestedType("State", BindingFlags.NonPublic).GetField(value).GetValue(king);
                stateMachine.SetValue(king, state);
            }
        }

        private int Step
        {
            get
            {
                FieldInfo field = king.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                return (int)field.GetValue(king);
            }
            set
            {
                FieldInfo field = king.GetType().GetField("Step", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(king, value);
            }
        }


        private void SetGreetedQueen(bool value)
        {
            PropertyInfo propertyInfo = king.GetType().GetProperty("GreetedQueen", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyInfo.SetValue(king, value);
        }
    }
}