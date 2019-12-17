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

            fileIO.Setup(e => e.GetFilesFromDirectory(@"c:\test")).Returns(new string[] { @"c:\test\file1", @"c:\test\file2" });
            fileIO.Setup(e => e.Exists(@"c:\test\file1")).Returns(true);
            fileIO.Setup(e => e.Exists(@"c:\test\file2")).Returns(false);
            fileIO.Setup(e => e.ReadBytes(@"c:\test\file1")).Returns(new byte[] { 0 });

            cachedFileIO = new CachedFileIO(null, fileIO.Object);
        }

        [TestMethod]
        public void CachedFileIO_Constructor()
        {
            cachedFileIO = new CachedFileIO(new List<string>() { @"c:\test" }, fileIO.Object);

            fileIO.Verify(e => e.CreateDirectory(@"c:\test"), Times.Once);
            fileIO.Verify(e => e.GetFilesFromDirectory(@"c:\test"), Times.Once);
            fileIO.Verify(e => e.Exists(@"c:\test\file1"), Times.Once);
            fileIO.Verify(e => e.Exists(@"c:\test\file2"), Times.Once);
            fileIO.Verify(e => e.ReadBytes(@"c:\test\file1"), Times.Once);
            fileIO.Verify(e => e.ReadBytes(@"c:\test\file2"), Times.Never);
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
        public void CachedFileIO_GetFilesFromDirectory()
        {
            cachedFileIO.AppendFile(@"c:\dir1\f1", "input");
            cachedFileIO.AppendFile(@"c:\dir2\f2", "input");

            string[] files = cachedFileIO.GetFilesFromDirectory(@"c:\dir1");

            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(@"c:\dir1\f1", files[0]);
        }

        [TestMethod]
        public void CachedFileIO_Exists()
        {
            cachedFileIO.AppendFile(@"c:\dir1\f1", "input");

            Assert.IsTrue(cachedFileIO.Exists(@"c:\dir1\f1"));
            Assert.IsFalse(cachedFileIO.Exists(@"c:\dir1\f2"));
        }

        [TestMethod]
        public void CachedFileIO_Delete()
        {
            cachedFileIO.Delete(@"c:\dir1\f1");

            fileIO.Verify(e => e.Delete(@"c:\dir1\f1"), Times.Once);
        }

        [TestMethod]
        public void CachedFileIO_CreateDirectory()
        {
            cachedFileIO.CreateDirectory(@"c:\dir1\f1");

            fileIO.Verify(e => e.CreateDirectory(@"c:\dir1\f1"), Times.Once);
        }


        [TestMethod]
        public void CachedFileIO_WriteSome()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
