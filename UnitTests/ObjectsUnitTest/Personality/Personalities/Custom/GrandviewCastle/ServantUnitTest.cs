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
            room.Setup(e => e.Zone).Returns(24);
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
            Step = 0;
            string command = "original";

            Assert.AreSame(command, servant.Process(npc.Object, command));
            Assert.AreEqual(0, Step);
        }

        [TestMethod]
        public void Servant_Process_InCombat()
        {
            Step = 0;
            npc.Setup(e => e.IsInCombat).Returns(true);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual(0, Step);
        }

        [TestMethod]
        public void Servant_Process_Wait()
        {
            Step = 0;
            npc.SetupSequence(e => e.DequeueMessage())
              .Returns("<Communication>King says Servant, bring me my meal.</Communication>")
              .Returns(null);

            string result = servant.Process(npc.Object, null);

            Assert.AreEqual(null, result);
            Assert.AreEqual(0, Step);
            Assert.AreEqual("AskedWhatWanted", State);
            npc.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(4));
            npc.Verify(e => e.EnqueueCommand("Emote bows."), Times.Once);
            npc.Verify(e => e.EnqueueCommand("Say Your Honorable Majestic Majesty Graciousness, what would you like to eat?"), Times.Once);
        }



        [TestMethod]
        public void ServantUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
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