using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Damage;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace ObjectsUnitTest.Damage
{
    [TestClass]
    public class DamageDealtUnitTest
    {
        [TestMethod]
        public void DamageDealt_Constructor()
        {
            DamageDealt damageDealt = new DamageDealt(DamageType.Force, 10);

            Assert.AreEqual(DamageType.Force, damageDealt.DamageType);
            Assert.AreEqual(10, damageDealt.Amount);
        }
    }
}
