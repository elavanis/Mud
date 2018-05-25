using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects;
using Objects.Interface;

namespace ObjectsUnitTest
{
    [TestClass]
    public class BaseObjectIdUnitTest
    {
        BaseObjectId baseObjectId;
        Mock<IBaseObject> baseObject;

        [TestInitialize]
        public void Setup()
        {
            baseObjectId = new BaseObjectId();
            baseObject = new Mock<IBaseObject>();

            baseObject.Setup(e => e.Zone).Returns(1);
            baseObject.Setup(e => e.Id).Returns(2);
        }

        [TestMethod]
        public void BaseObjectId_Constructor()
        {
            baseObjectId = new BaseObjectId(1, 2);

            Assert.AreEqual(1, baseObjectId.Zone);
            Assert.AreEqual(2, baseObjectId.Id);
        }

        [TestMethod]
        public void BaseObjectId_ConstructorWithObject()
        {
            baseObjectId = new BaseObjectId(baseObject.Object);

            Assert.AreEqual(1, baseObjectId.Zone);
            Assert.AreEqual(2, baseObjectId.Id);
        }

        [TestMethod]
        public void BaseObjectId_ToString()
        {
            baseObjectId.Zone = 1;
            baseObjectId.Id = 2;

            string expected = "1-2";
            string actual = baseObjectId.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
