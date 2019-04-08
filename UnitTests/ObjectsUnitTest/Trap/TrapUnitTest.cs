using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Stats;
using Objects.Trap;
using Objects.Trap.Interface;

namespace ObjectsUnitTest.Trap
{
    [TestClass]
    public class TrapUnitTest
    {
        [TestMethod]
        public void Trap_Initialize()
        {
            ITrap trap = new Objects.Trap.Trap();

            Assert.IsNotNull(trap.Enchantments);
            Assert.IsNotNull(trap.DisarmWord);
            Assert.AreEqual(trap.Trigger, Target.TrapTrigger.PC);
            Assert.AreEqual(trap.DisarmStat, Stats.Stat.Dexterity);
        }
    }
}
