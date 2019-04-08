using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Magic.Enchantment.DefeatbleInfo;
using Objects.Mob.Interface;
using static Objects.Global.Stats.Stats;

namespace ObjectsUnitTest.Magic.Enchantment.DefeatbleInfo
{
    [TestClass]
    public class DefeatInfoUnitTest
    {
        DefeatInfo defeatInfo;
        Mock<IMobileObject> mob;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            defeatInfo = new DefeatInfo();
            mob = new Mock<IMobileObject>();
            random = new Mock<IRandom>();

            defeatInfo.CurrentEnchantmentPoints = 1000;
            defeatInfo.MobStat = Stat.Constitution;
            mob.Setup(e => e.GetStatEffective(Stat.Constitution)).Returns(800);
            random.Setup(e => e.Next(1000)).Returns(500);
            random.Setup(e => e.Next(800)).Returns(400);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void DefeatbleInfo_DoesPayerDefeatEnchantment_No()
        {
            bool result = defeatInfo.DoesPayerDefeatEnchantment(mob.Object);

            Assert.IsFalse(result);
            Assert.AreEqual(800, defeatInfo.CurrentEnchantmentPoints);
        }

        [TestMethod]
        public void DefeatbleInfo_DoesPayerDefeatEnchantment_Yes()
        {
            mob.Setup(e => e.GetStatEffective(Stat.Constitution)).Returns(1000);

            bool result = defeatInfo.DoesPayerDefeatEnchantment(mob.Object);

            Assert.IsTrue(result);
        }
    }
}
