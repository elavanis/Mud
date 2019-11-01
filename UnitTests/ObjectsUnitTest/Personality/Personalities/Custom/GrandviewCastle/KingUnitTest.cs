using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.MoneyToCoins.Interface;
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

            npc.Setup(e => e.Room).Returns(room.Object);
            findObjects.Setup(e => e.FindNpcInRoom(room.Object, "kings guard")).Returns(new List<INonPlayerCharacter>() { npc.Object });
            defaultValues.Setup(e => e.DiceForArmorLevel(47)).Returns(dice.Object);
            random.Setup(e => e.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
            settings.Setup(e => e.BaseStatValue).Returns(5);
            moneyToCoins.Setup(e => e.FormatedAsCoins(It.IsAny<ulong>())).Returns("some");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));


            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            king = new King();
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

            king.Process(npc.Object, null);

            npc.Verify(e => e.EnqueueCommand("Shout GUARDS!"), Times.Once);
            room.Verify(e => e.Enter(It.IsAny<INonPlayerCharacter>()), Times.Exactly(3));

        }


        [TestMethod]
        public void KingUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }

        private void SetState(string stateName)
        {
            PropertyInfo stateMachine = king.GetType().GetProperty("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);
            object state = king.GetType().GetNestedType("State", BindingFlags.NonPublic).GetField(stateName).GetValue(king);
            stateMachine.SetValue(king, state);
        }
    }
}