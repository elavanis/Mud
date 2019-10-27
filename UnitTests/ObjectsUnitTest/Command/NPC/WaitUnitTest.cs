using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Command.NPC
{
    [TestClass]
    public class WaitUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

        }

        [TestMethod]
        public void WaitUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }

    }
}