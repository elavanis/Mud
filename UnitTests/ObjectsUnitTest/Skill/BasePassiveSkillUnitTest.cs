using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
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
            GlobalReference.GlobalValues = new GlobalValues();

            passiveSkill = new UnitTestPassiveSkill();
        }

        [TestMethod]
        public void BasePassiveSkill_Constructor()
        {
            Assert.IsTrue(passiveSkill.Passive);
        }

        private class UnitTestPassiveSkill : BasePassiveSkill
        {
            public UnitTestPassiveSkill() : base("unitTestPassiveSkill")
            {

            }
        }
    }
}
