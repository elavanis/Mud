using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Damage;
using Objects.Global.Settings.Interface;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectsUnitTest.Global.Damage
{
    [TestClass]
    public class DamageIdUnitTest
    {
        DamageId damageId;
        Mock<ISettings> settings;
        Mock<IFileIO> fileIO;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            settings = new Mock<ISettings>();
            fileIO = new Mock<IFileIO>();

            settings.Setup(e => e.DamageIdDirectory).Returns("path");
            fileIO.Setup(e => e.ReadAllText("path\\DamageId.txt")).Returns("10");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;

            damageId = new DamageId();
        }

        [TestMethod]
        public void DamageId_Initialize()
        {
            damageId.Initialize();

            Assert.AreEqual(11, damageId.Id);
        }

        [TestMethod]
        public void DamageId_Id_Increment()
        {
            Assert.AreEqual(1, damageId.Id);
        }


    }
}
