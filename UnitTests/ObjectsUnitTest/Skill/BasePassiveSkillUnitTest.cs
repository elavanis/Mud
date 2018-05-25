using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Skill;

namespace ObjectsUnitTest.Skill
{
    [TestClass]
    public class BasePassiveSkillUnitTest
    {
        UnitTestPassiveSkill passiveSkill;

        [TestInitialize]
        public void Setup()
        {
            passiveSkill = new UnitTestPassiveSkill();
        }

        [TestMethod]
        public void BasePassiveSkill_Constructor()
        {
            Assert.IsTrue(passiveSkill.Passive);
        }

        private class UnitTestPassiveSkill : BasePassiveSkill
        {

        }
    }
}
