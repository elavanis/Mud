using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class HydraUnitTest
    {
        Hydra hydra;
        Mock<IHydra> hydraMob;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            hydraMob = new Mock<IHydra>();
            hydra = new Hydra();
        }

        [TestMethod]
        public void Hydra_Process()
        {
            hydra.Process(hydraMob.Object, null);

            hydraMob.Verify(e => e.RegrowHeads(), Times.Once);
        }
    }
}