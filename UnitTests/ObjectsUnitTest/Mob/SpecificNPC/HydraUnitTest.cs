using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Mob.SpecificNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectsUnitTest.Mob.SpecificNPC
{
    [TestClass]
    public class HydraUnitTest
    {
        Hydra hydra;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> level1Dice;
        Mock<IDice> level5Dice;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            level1Dice = new Mock<IDice>();
            level5Dice = new Mock<IDice>();

            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(level1Dice.Object);
            defaultValues.Setup(e => e.DiceForWeaponLevel(5)).Returns(level5Dice.Object);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            hydra = new Hydra();
        }

        [TestMethod]
        public void Hydra_WriteTests()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Hydra_HasHydraPersonality()
        {
            Assert.AreEqual(1, hydra.Personalities.Count);
            Assert.IsNotNull(hydra.Personalities[0] as Objects.Personality.Personalities.Hydra);
        }

        [TestMethod]
        public void Hydra_NotDisarmable()
        {
            Assert.IsTrue(hydra.AttributesCurrent.Contains(Objects.Mob.MobileObject.MobileAttribute.NoDisarm));
        }

        [TestMethod]
        public void Hydra_SetLevel1()
        {
            hydra.Level = 1;

            //verify 5 heads
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            //verify min level 1 dice
            Assert.AreEqual(level1Dice.Object, hydra.EquipedWeapon.FirstOrDefault().DamageList[0].Dice);
        }

        [TestMethod]
        public void Hydra_SetLevel10()
        {
            hydra.Level = 10;

            //verify 5 heads
            Assert.AreEqual(5, hydra.EquipedWeapon.Count());
            //verify level 5 dice
            Assert.AreEqual(level5Dice.Object, hydra.EquipedWeapon.FirstOrDefault().DamageList[0].Dice);
        }
    }
}
