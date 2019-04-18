using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Material;

namespace ObjectsUnitTest.Material
{
    [TestClass]
    public class BaseMaterialUnitTest
    {
        UnitTestMaterial material;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            material = new UnitTestMaterial();
        }

        [TestMethod]
        public void BaseMatherial_Acid()
        {
            Assert.AreEqual(decimal.MaxValue, material.Acid);
        }

        [TestMethod]
        public void BaseMatherial_Bludgeon()
        {
            Assert.AreEqual(decimal.MaxValue, material.Bludgeon);
        }

        [TestMethod]
        public void BaseMatherial_Cold()
        {
            Assert.AreEqual(decimal.MaxValue, material.Cold);
        }

        [TestMethod]
        public void BaseMatherial_Fire()
        {
            Assert.AreEqual(decimal.MaxValue, material.Fire);
        }

        [TestMethod]
        public void BaseMatherial_Force()
        {
            Assert.AreEqual(decimal.MaxValue, material.Force);
        }

        [TestMethod]
        public void BaseMatherial_Lightning()
        {
            Assert.AreEqual(decimal.MaxValue, material.Lightning);
        }

        [TestMethod]
        public void BaseMatherial_Necrotic()
        {
            Assert.AreEqual(decimal.MaxValue, material.Necrotic);
        }

        [TestMethod]
        public void BaseMatherial_Pierce()
        {
            Assert.AreEqual(decimal.MaxValue, material.Pierce);
        }

        [TestMethod]
        public void BaseMatherial_Poison()
        {
            Assert.AreEqual(decimal.MaxValue, material.Poison);
        }

        [TestMethod]
        public void BaseMatherial_Pyschic()
        {
            Assert.AreEqual(decimal.MaxValue, material.Psychic);
        }

        [TestMethod]
        public void BaseMatherial_Radiant()
        {
            Assert.AreEqual(decimal.MaxValue, material.Radiant);
        }

        [TestMethod]
        public void BaseMatherial_Slash()
        {
            Assert.AreEqual(decimal.MaxValue, material.Slash);
        }

        [TestMethod]
        public void BaseMatherial_Thunder()
        {
            Assert.AreEqual(decimal.MaxValue, material.Thunder);
        }


        [TestMethod]
        public void BaseMaterial_Strong()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(12, 15)).Returns(10);
            GlobalReference.GlobalValues.Random = random.Object;

            decimal value = material.Strong();
            Assert.AreEqual(1.0M, value);
        }


        [TestMethod]
        public void BaseMaterial_Moderate()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(9, 12)).Returns(20);
            GlobalReference.GlobalValues.Random = random.Object;

            decimal value = material.Moderate();
            Assert.AreEqual(2.0M, value);
        }


        [TestMethod]
        public void BaseMaterial_Weak()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6, 9)).Returns(30);
            GlobalReference.GlobalValues.Random = random.Object;

            decimal value = material.Weak();
            Assert.AreEqual(3.0M, value);
        }


        private class UnitTestMaterial : BaseMaterial
        {
        }
    }
}
