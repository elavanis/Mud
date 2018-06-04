using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Settings.Interface;
using Shared.FileIO.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectsUnitTest.Global.ValidateAsset
{
    [TestClass]
    public class ValidateAssetUnitTest
    {
        Objects.Global.ValidateAsset.ValidateAsset validateAsset;
        ConcurrentDictionary<string, string> assetHashes;
        Mock<ISettings> settings;
        Mock<IFileIO> fileIO;

        [TestInitialize]
        public void Setup()
        {
            validateAsset = new Objects.Global.ValidateAsset.ValidateAsset();
            FieldInfo fieldInfo = validateAsset.GetType().GetField("assetHashes", BindingFlags.NonPublic | BindingFlags.Instance);
            assetHashes = (ConcurrentDictionary<string, string>)fieldInfo.GetValue(validateAsset);
            settings = new Mock<ISettings>();
            fileIO = new Mock<IFileIO>();

            settings.Setup(e => e.AssetsDirectory).Returns("c:\\assets");
            fileIO.Setup(e => e.ReadBytes("c:\\assets\file")).Returns(new byte[] { 1 });

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;

        }

        [TestMethod]
        public void ValidateAsset_Clear()
        {
            assetHashes.TryAdd("a", "b");

            Assert.AreEqual(1, assetHashes.Keys.Count);

            validateAsset.ClearValidations();

            Assert.AreEqual(0, assetHashes.Keys.Count);
        }

        [TestMethod]
        public void ValidateAsset_GetCheckSum()
        {
            validateAsset.GetCheckSum("prevalue|file");

            Assert.AreEqual(1, assetHashes.Keys.Count);
            Assert.IsTrue(assetHashes.ContainsKey("file"));
            Assert.AreEqual("D41D8CD98F00B204E9800998ECF8427E", assetHashes["file"]);
        }

        [TestMethod]
        public void ValidateAsset_Error()
        {
            string result = validateAsset.GetCheckSum("file");

            Assert.IsNull(result);
        }
    }
}
