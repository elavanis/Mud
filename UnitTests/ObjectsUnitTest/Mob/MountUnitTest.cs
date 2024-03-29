﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Global.Settings.Interface;
using Objects.Mob;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Mob.Mount;

namespace ObjectsUnitTest.Mob
{
    [TestClass]
    public class MountUnitTest
    {
        Mount mount;
        Mock<IRandom> random;
        Mock<ISettings> settings;
        Mock<IRoom> room;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            random = new Mock<IRandom>();
            settings = new Mock<ISettings>();
            room = new Mock<IRoom>();

            random.Setup(e => e.Next(It.IsAny<int>())).Returns(0);
            settings.Setup(e => e.BaseStatValue).Returns(5);
            settings.Setup(e => e.AssignableStatPoints).Returns(2);

            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            mount = new Mount(DefaultValues.Horse, room.Object);
            mount.Level = 1;
        }

        [TestMethod]
        public void Mount_Constructor_Horse()
        {
            mount = new Mount(DefaultValues.Horse, room.Object);

            Assert.AreEqual(2, mount.Movement);
            Assert.AreEqual(10, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Kisses", mount.KeyWords[0]);
            Assert.AreEqual("Horse", mount.KeyWords[1]);
            Assert.AreEqual("A large black horse.", mount.ShortDescription);
            Assert.AreEqual("horse", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Unicorn()
        {
            mount = new Mount(DefaultValues.Unicorn, room.Object);

            Assert.AreEqual(2, mount.Movement);
            Assert.AreEqual(12, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Uni", mount.KeyWords[0]);
            Assert.AreEqual("Unicorn", mount.KeyWords[1]);
            Assert.AreEqual("The white unicorn looks at you.", mount.ShortDescription);
            Assert.AreEqual("unicorn", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Nightmare()
        {
            mount = new Mount(DefaultValues.Nightmare, room.Object);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(15, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Orkiz", mount.KeyWords[0]);
            Assert.AreEqual("Nightmare", mount.KeyWords[1]);
            Assert.AreEqual("Flames burn brightly from the mane and hooves of this black as ink nightmare.", mount.ShortDescription);
            Assert.AreEqual("nightmare", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Elephant()
        {
            mount = new Mount(DefaultValues.Elephant, room.Object);

            Assert.AreEqual(1, mount.Movement);
            Assert.AreEqual(20, mount.StaminaMultiplier);
            Assert.AreEqual(5, mount.MaxRiders);
            Assert.AreEqual("Skitters", mount.KeyWords[0]);
            Assert.AreEqual("Elephant", mount.KeyWords[1]);
            Assert.AreEqual("The elephants trunk reaches down toward the ground looking for food.", mount.ShortDescription);
            Assert.AreEqual("elephant", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Elk()
        {
            mount = new Mount(DefaultValues.Elk, room.Object);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(7, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Addax", mount.KeyWords[0]);
            Assert.AreEqual("Elk", mount.KeyWords[1]);
            Assert.AreEqual("The elk has a large rack with two reigns tied off on a saddle.", mount.ShortDescription);
            Assert.AreEqual("elk", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Panther()
        {
            mount = new Mount(DefaultValues.Panther, room.Object);

            Assert.AreEqual(5, mount.Movement);
            Assert.AreEqual(5, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Storm", mount.KeyWords[0]);
            Assert.AreEqual("Panther", mount.KeyWords[1]);
            Assert.AreEqual("Yellow eyes almost glow against the black panthers fur.", mount.ShortDescription);
            Assert.AreEqual("panther", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_Constructor_Griffin()
        {
            mount = new Mount(DefaultValues.Griffin, room.Object);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(7, mount.StaminaMultiplier);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Apollo", mount.KeyWords[0]);
            Assert.AreEqual("Griffin", mount.KeyWords[1]);
            Assert.AreEqual("A majestic griffin stands at the ready.", mount.ShortDescription);
            Assert.AreEqual("griffin", mount.SentenceDescription);
            Assert.AreEqual(room.Object, mount.Room);
        }

        [TestMethod]
        public void Mount_FinishLoad()
        {
            mount.FinishLoad();

            Assert.AreEqual(7, mount.StrengthStat);
            Assert.AreEqual(5, mount.DexterityStat);
            Assert.AreEqual(5, mount.ConstitutionStat);
            Assert.AreEqual(5, mount.IntelligenceStat);
            Assert.AreEqual(5, mount.WisdomStat);
            Assert.AreEqual(5, mount.CharismaStat);
        }

        [TestMethod]
        public void Mount_MaxStamina()
        {
            mount.ConstitutionStat = 10;
            mount.StaminaMultiplier = 5;

            Assert.AreEqual(500, mount.MaxStamina);
        }
    }
}
