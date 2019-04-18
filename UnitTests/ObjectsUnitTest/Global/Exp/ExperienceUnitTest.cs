using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global.Exp;
using Objects.Global;
using Objects.Global.Settings.Interface;

namespace ObjectsUnitTest.Global.Exp
{
    [TestClass]
    public class ExperienceUnitTest
    {
        Experience exp;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            Mock<ISettings> settings = new Mock<ISettings>();
            settings.Setup(e => e.MaxLevel).Returns(100);
            settings.Setup(e => e.Multiplier).Returns(1.1d);
            settings.Setup(e => e.MaxCalculationLevel).Returns(130);
            GlobalReference.GlobalValues.Settings = settings.Object;

            exp = new Experience();
        }

        [TestMethod]
        public void Experience_GetDefaultNpcExpForLevel_0()
        {
            int result = exp.GetDefaultNpcExpForLevel(0);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Experience_GetDefaultNpcExpForLevel_1()
        {
            int result = exp.GetDefaultNpcExpForLevel(1);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Experience_GetDefaultNpcExpForLevel_100()
        {
            int result = exp.GetDefaultNpcExpForLevel(100);

            Assert.AreEqual(1262643, result);
        }



        [TestMethod]
        public void Experience_GetExpForLevel_1()
        {
            int result = exp.GetExpForLevel(1);

            Assert.AreEqual(1000, result);
        }

        [TestMethod]
        public void Experience_GetExpForLevel_100()
        {
            int result = exp.GetExpForLevel(100);

            Assert.AreEqual(896223637, result);
        }

        [TestMethod]
        public void Experience_GetExpForLevel_101()
        {
            int result = exp.GetExpForLevel(101);

            Assert.AreEqual(int.MaxValue, result);
        }


    }
}
