using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.LevelDifference;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.LevelDifference
{
    [TestClass]
    public class EvaluateLevelDifferenceUnitTest
    {
        Mock<IMobileObject> mob1;
        Mock<IMobileObject> mob2;
        EvaluateLevelDifference evaluateLevelDifference;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mob1 = new Mock<IMobileObject>();
            mob2 = new Mock<IMobileObject>();

            evaluateLevelDifference = new EvaluateLevelDifference();
        }


        [TestMethod]
        public void EvaluateLevelDifference_SameLevel()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(1);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("This should be a close match.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_1Higher()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(2);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("They should be victorious but badly wounded.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_2Higher()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(3);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("They should win.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_3Higher()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(4);
            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("The house money is on them winning.", result);

            mob2.Setup(e => e.Level).Returns(5);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("The house money is on them winning.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_5Higher()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(6);
            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("They don't have a chance.", result);

            mob2.Setup(e => e.Level).Returns(7);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("They don't have a chance.", result);

            mob2.Setup(e => e.Level).Returns(8);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("They don't have a chance.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_8Higher()
        {
            mob1.Setup(e => e.Level).Returns(1);
            mob2.Setup(e => e.Level).Returns(9);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("MEDIC!!!", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_1Lower()
        {
            mob1.Setup(e => e.Level).Returns(2);
            mob2.Setup(e => e.Level).Returns(1);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("You should be victorious but badly wounded.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_2Lower()
        {
            mob1.Setup(e => e.Level).Returns(3);
            mob2.Setup(e => e.Level).Returns(1);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("You should win.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_3Lower()
        {
            mob1.Setup(e => e.Level).Returns(4);
            mob2.Setup(e => e.Level).Returns(1);
            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("The house money is on you winning.", result);

            mob1.Setup(e => e.Level).Returns(5);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("The house money is on you winning.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_5Lower()
        {
            mob1.Setup(e => e.Level).Returns(6);
            mob2.Setup(e => e.Level).Returns(1);
            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("You don't have a chance.", result);

            mob1.Setup(e => e.Level).Returns(7);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("You don't have a chance.", result);

            mob1.Setup(e => e.Level).Returns(8);
            result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("You don't have a chance.", result);
        }

        [TestMethod]
        public void EvaluateLevelDifference_8Lower()
        {
            mob1.Setup(e => e.Level).Returns(9);
            mob2.Setup(e => e.Level).Returns(1);

            string result = evaluateLevelDifference.Evalute(mob1.Object, mob2.Object);
            Assert.AreEqual("There should be laws against attacking the helpless.", result);
        }
    }
}
