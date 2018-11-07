using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Item.Items;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Item.Item;

namespace ObjectsUnitTest.Item.Items
{
    public class FountainUnitTest
    {

        [TestMethod]
        public void Fountain_Constructor()
        {
            Fountain fountain = new Fountain();

            Assert.IsTrue(fountain.Attributes.Contains(ItemAttribute.NoGet));
            Assert.AreEqual("Fountain", fountain.KeyWords[0]);
        }
    }
}
