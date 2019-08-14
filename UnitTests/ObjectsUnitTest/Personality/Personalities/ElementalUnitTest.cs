using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class ElementalUnitTest
    {
        Elemental elemental;
        Mock<IElemental> elementalMob;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            elementalMob = new Mock<IElemental>();
            elemental = new Elemental();
        }

        [TestMethod]
        public void Elemental_Process()
        {
            elemental.Process(elementalMob.Object, null);

            elementalMob.Verify(e => e.ProcessElementalTick(), Times.Once);
        }
    }
}
