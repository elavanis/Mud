using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Skill;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill
{
    [TestClass]
    public class BasePassiveSkillUnitTest
    {
        UnitTestPassiveSkill passiveSkill;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
            
            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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
