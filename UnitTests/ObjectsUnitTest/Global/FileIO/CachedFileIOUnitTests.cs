using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.FileIO;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.FileIO
{
    [TestClass]
    public class CachedFileIOUnitTests
    {

        CachedFileIO cachedFileIO;
        Mock<IFileIO> fileIO;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            fileIO = new Mock<IFileIO>();

            cachedFileIO = new CachedFileIO(null, fileIO.Object);
        }

        [TestMethod]
        public void CachedFileIO_Constructor()
        {
            cachedFileIO = new CachedFileIO(new List<string>() { @"c:\test" }, fileIO.Object);

            fileIO.Verify(e => e.CreateDirectory(@"c:\test"), Times.Once);
            fileIO.Verify(e => e.GetFilesFromDirectory(@"c:\test"), Times.Once);
        }

        [TestMethod]
        public void CachedFileIO_AppendFile_ReadAllText_ReadlLines()
        {
            List<string> lines = new List<string>() { "line1", "line2" };

            cachedFileIO.AppendFile("test", lines);

            string result = cachedFileIO.ReadAllText("test");
            Assert.AreEqual("line1\r\nline2", result);

            string[] result2 = cachedFileIO.ReadLines("test");
            string[] expected = new string[] { "line1", "line2" };
            Assert.AreEqual(expected.Length, result2.Length);
            Assert.AreEqual(expected[0], result2[0]);
            Assert.AreEqual(expected[1], result2[1]);
        }

        [TestMethod]
        public void CachedFileIO_WriteFile_ReadAllText()
        {
            cachedFileIO.WriteFile("test", "1");
            cachedFileIO.WriteFile("test", "2");

            string result = cachedFileIO.ReadAllText("test");
            Assert.AreEqual("2", result);
        }

        [TestMethod]
        public void CachedFileIO_WriteFileBase64_ReadAllText()
        {
            string original = Convert.ToBase64String(Encoding.ASCII.GetBytes("1"));
            string expected = Convert.ToBase64String(Encoding.ASCII.GetBytes("2"));

            cachedFileIO.WriteFileBase64("test", original);
            cachedFileIO.WriteFileBase64("test", expected);

            string result = cachedFileIO.ReadFileBase64("test");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CachedFileIO_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
