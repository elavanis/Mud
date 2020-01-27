using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Weapon;
using Objects.Global.Settings.Interface;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectsUnitTest.Global.Damage
{
    [TestClass]
    public class WeaponIdUnitTest
    {
        WeaponId weaponId;
        Mock<ISettings> settings;
        Mock<IFileIO> fileIO;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            settings = new Mock<ISettings>();
            fileIO = new Mock<IFileIO>();

            settings.Setup(e => e.WeaponIdDirectory).Returns("path");
            fileIO.Setup(e => e.ReadAllText("path\\WeaponId.txt")).Returns("10");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;

            weaponId = new WeaponId();
        }

        [TestMethod]
        public void WeaponId_Initialize()
        {
            weaponId.Initialize();

            Assert.AreEqual(11, weaponId.Id);
        }

        [TestMethod]
        public void WeaponId_Id_Increment()
        {
            Assert.AreEqual(1, weaponId.Id);
        }


    }
}
