using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality.Personalities;

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