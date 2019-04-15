using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class PartyUnitTest
    {
        Party party;

        [TestInitialize]
        public void Setup()
        {
            party = new Party();
        }

        [TestMethod]
        public void Party_WritePartyUnitTests()
        {
            Assert.AreEqual(1, 2);
        }
    }
}