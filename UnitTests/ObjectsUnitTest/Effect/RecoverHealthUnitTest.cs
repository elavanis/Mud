﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global.Logging.Interface;
using Objects.Global;
using Objects.Effect;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Objects.Die.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class RecoverHealthUnitTest
    {
        RecoverHealth effect;
        Mock<IPlayerCharacter> pc;
        Mock<IEffectParameter> parameter;
        Mock<IDice> dice;

        [TestInitialize]
        public void Setup()
        {
            effect = new RecoverHealth();
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            dice = new Mock<IDice>();

            parameter.Setup(e => e.Dice).Returns(dice.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
            dice.Setup(e => e.RollDice()).Returns(10);
        }

        [TestMethod]
        public void RecoverHealth_ProcessEffect()
        {

            Mock<ILogger> logger = new Mock<ILogger>();

            GlobalReference.GlobalValues.Logger = logger.Object;

            effect.ProcessEffect(parameter.Object);

            pc.VerifySet(e => e.Health = 10);
        }

        [TestMethod]
        public void RecoverHealth_ProcessEffect_MaxStamina()
        {

            Mock<ILogger> logger = new Mock<ILogger>();

            pc.Setup(e => e.MaxHealth).Returns(5);
            pc.Setup(e => e.Health).Returns(10);

            GlobalReference.GlobalValues.Logger = logger.Object;

            effect.ProcessEffect(parameter.Object);

            pc.VerifySet(e => e.Health = 5);
        }
    }
}
