using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using Objects.Race.Interface;

namespace ObjectsUnitTest.Material.Materials
{
    [TestClass]
    public class NpcInnateArmorUnitTest
    {
        private NpcInnateArmor material;
        private Mock<INonPlayerCharacter> npc;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            npc = new Mock<INonPlayerCharacter>();
            Mock<IRace> race = new Mock<IRace>();
            race.Setup(e => e.Bludgeon).Returns(1);
            race.Setup(e => e.Pierce).Returns(2);
            race.Setup(e => e.Slash).Returns(3);

            race.Setup(e => e.Force).Returns(4);
            race.Setup(e => e.Necrotic).Returns(5);
            race.Setup(e => e.Psychic).Returns(6);
            race.Setup(e => e.Radiant).Returns(7);
            race.Setup(e => e.Thunder).Returns(8);

            race.Setup(e => e.Acid).Returns(9);
            race.Setup(e => e.Cold).Returns(10);
            race.Setup(e => e.Fire).Returns(11);
            race.Setup(e => e.Lightning).Returns(12);
            race.Setup(e => e.Poison).Returns(13);

            npc.Setup(e => e.Race).Returns(race.Object);

            material = new NpcInnateArmor(npc.Object);
        }

        [TestMethod]
        public void NpcInnateArmor_Constructor()
        {
            Assert.AreEqual(1, material.Bludgeon);
            Assert.AreEqual(2, material.Pierce);
            Assert.AreEqual(3, material.Slash);

            Assert.AreEqual(4, material.Force);
            Assert.AreEqual(5, material.Necrotic);
            Assert.AreEqual(6, material.Psychic);
            Assert.AreEqual(7, material.Radiant);
            Assert.AreEqual(8, material.Thunder);

            Assert.AreEqual(9, material.Acid);
            Assert.AreEqual(10, material.Cold);
            Assert.AreEqual(11, material.Fire);
            Assert.AreEqual(12, material.Lightning);
            Assert.AreEqual(13, material.Poison);
        }
    }
}
