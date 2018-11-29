using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Item.Items.BulletinBoard
{
    [TestClass]
    public class BulletinBoardUnitTest
    {
        Objects.Item.Items.BulletinBoard.BulletinBoard bulletinBoard;

        [TestInitialize]
        public void Setup()
        {
            bulletinBoard = new Objects.Item.Items.BulletinBoard.BulletinBoard();
        }

        [TestMethod]
        public void BulletinBoard_WriteUnitTests()
        {
            Assert.AreEqual(-1, 0);
        }
    }
}
