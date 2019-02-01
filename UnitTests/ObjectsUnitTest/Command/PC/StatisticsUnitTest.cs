using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Command.PC;
using System.Collections.Generic;
using System.Linq;
using Objects.Race.Interface;
using Objects.Global.Exp.Interface;
using Objects.Global.MoneyToCoins.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class StatisticsUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Statistics();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
        }

        [TestMethod]
        public void Statistics_Instructions()
        {
            IResult result = command.Instructions;
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("(Stats) Statistics", result.ResultMessage);
        }

        [TestMethod]
        public void Statistics_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("Stats"));
            Assert.IsTrue(result.Contains("Statistics"));
        }

        [TestMethod]
        public void Statistics_PerformCommand_PC()
        {
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IRace> race = new Mock<IRace>();
            Mock<IExperience> exp = new Mock<IExperience>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            pc.Setup(e => e.Level).Returns(1);
            pc.Setup(e => e.Experience).Returns(2);

            race.Setup(e => e.ToString()).Returns("Human");
            pc.Setup(e => e.Race).Returns(race.Object);
            pc.Setup(e => e.Name).Returns("MyName");
            pc.Setup(e => e.StrengthStat).Returns(1);
            pc.Setup(e => e.DexterityStat).Returns(20);
            pc.Setup(e => e.ConstitutionStat).Returns(300);
            pc.Setup(e => e.IntelligenceStat).Returns(4000);
            pc.Setup(e => e.WisdomStat).Returns(50000);
            pc.Setup(e => e.CharismaStat).Returns(600000);

            pc.Setup(e => e.StrengthMultiClassBonus).Returns(11);
            pc.Setup(e => e.DexterityMultiClassBonus).Returns(22);
            pc.Setup(e => e.ConstitutionMultiClassBonus).Returns(33);
            pc.Setup(e => e.IntelligenceMultiClassBonus).Returns(44);
            pc.Setup(e => e.WisdomMultiClassBonus).Returns(55);
            pc.Setup(e => e.CharismaMultiClassBonus).Returns(66);


            GlobalReference.GlobalValues.Experience = exp.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(pc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("MyName   Human\r\nLevel 1\r\nEXP 2   To Next Level -2\r\n\r\nStat  Value    Bonus\r\nSTR:  1       11\r\nDEX:  20      22\r\nCON:  300     33\r\nINT:  4000    44\r\nWIS:  50000   55\r\nCHA:  600000  66", result.ResultMessage);
        }

        [TestMethod]
        public void Statistics_PerformCommand_NPC()
        {
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IRace> race = new Mock<IRace>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();

            npc.Setup(e => e.Level).Returns(1);

            race.Setup(e => e.ToString()).Returns("Human");
            npc.Setup(e => e.Race).Returns(race.Object);
            npc.Setup(e => e.StrengthStat).Returns(1);
            npc.Setup(e => e.DexterityStat).Returns(20);
            npc.Setup(e => e.ConstitutionStat).Returns(300);
            npc.Setup(e => e.IntelligenceStat).Returns(4000);
            npc.Setup(e => e.WisdomStat).Returns(50000);
            npc.Setup(e => e.CharismaStat).Returns(600000);

            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            IResult result = command.PerformCommand(npc.Object, mockCommand.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("Human\r\nLevel 1\r\n\r\nStat  Value\r\nSTR:  1\r\nDEX:  20\r\nCON:  300\r\nINT:  4000\r\nWIS:  50000\r\nCHA:  600000", result.ResultMessage);
        }
    }
}
