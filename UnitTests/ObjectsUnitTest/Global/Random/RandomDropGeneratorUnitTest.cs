using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Random;
using Objects.Global.Random.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Item.Items.Weapon;

namespace ObjectsUnitTest.Global.Random
{
    [TestClass]
    public class RandomDropGeneratorUnitTest
    {
        RandomDropGenerator randomDropGenerator;
        Mock<IDefaultValues> defaultValues;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            randomDropGenerator = new RandomDropGenerator();
            defaultValues = new Mock<IDefaultValues>();
            random = new Mock<IRandom>();

            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(new Dice(1, 2));
            random.Setup(e => e.Next(It.IsAny<int>())).Returns(0);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.Random = random.Object;
        }


    }
}
