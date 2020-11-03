using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Objects.Item.Items.Custom.UnderGrandViewCastle;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Item.Items.Custom.UnderGrandViewCastle
{
    class RunicStatueUnitTest
    {
        [TestClass]
        public class RunisStatueUnitTest
        {
            RunicStatue statue;

            [TestInitialize]
            public void Setup()
            {
                GlobalReference.GlobalValues = new GlobalValues();

                statue = new RunicStatue();
            }

            [TestMethod]
            public void RunisStatue_WriteTests()
            {
                Assert.IsTrue(false);
            }
        }
    }
}
