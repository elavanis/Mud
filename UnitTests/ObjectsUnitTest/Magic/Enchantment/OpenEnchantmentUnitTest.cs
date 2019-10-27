using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Magic.Enchantment
{
    [TestClass]
    public class OpenEnchantmentUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

        }

        [TestMethod]
        public void OpenEnchantmentUnitTest_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }
    }
}