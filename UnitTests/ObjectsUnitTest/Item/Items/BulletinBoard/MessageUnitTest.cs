using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Item.Items.BulletinBoard
{
    [TestClass]
    public class MessageUnitTest
    {
        Objects.Item.Items.BulletinBoard.Message message;

        [TestInitialize]
        public void Setup()
        {
            message = new Objects.Item.Items.BulletinBoard.Message();
        }

        [TestMethod]
        public void Message_WriteUnitTests()
        {
            Assert.AreEqual(-1, 0);
        }
    }
}
