using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Mob;
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
        [TestInitialize]
        public void Setup()
        {
            mount = new Mount();
            random = new Mock<IRandom>();

            random.Setup(e => e.Next(It.IsAny<int>())).Returns(0);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Horse()
        {
            mount.LoadDefaultValues(DefaultValues.Horse);

            Assert.AreEqual(2, mount.Movement);
            Assert.AreEqual(10, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Track, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Kisses", mount.KeyWords[0]);
            Assert.AreEqual("Horse", mount.KeyWords[1]);
            Assert.AreEqual("A large black horse.", mount.ShortDescription);
            Assert.AreEqual("horse", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Unicorn()
        {
            mount.LoadDefaultValues(DefaultValues.Unicorn);

            Assert.AreEqual(2, mount.Movement);
            Assert.AreEqual(12, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Track, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Uni", mount.KeyWords[0]);
            Assert.AreEqual("Unicorn", mount.KeyWords[1]);
            Assert.AreEqual("The white unicorn looks at you.", mount.ShortDescription);
            Assert.AreEqual("unicorn", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Nightmare()
        {
            mount.LoadDefaultValues(DefaultValues.Nightmare);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(15, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Summon, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Orkiz", mount.KeyWords[0]);
            Assert.AreEqual("Nightmare", mount.KeyWords[1]);
            Assert.AreEqual("Flames burn brightly from the mane and hooves of this black as night horse.", mount.ShortDescription);
            Assert.AreEqual("nightmare", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Elephant()
        {
            mount.LoadDefaultValues(DefaultValues.Elephant);

            Assert.AreEqual(1, mount.Movement);
            Assert.AreEqual(20, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Track, mount.TypeOfCall);
            Assert.AreEqual(5, mount.MaxRiders);
            Assert.AreEqual("Skitters", mount.KeyWords[0]);
            Assert.AreEqual("Elephant", mount.KeyWords[1]);
            Assert.AreEqual("The elephants trunk reaches down toward the ground looking for food.", mount.ShortDescription);
            Assert.AreEqual("elephant", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Elk()
        {
            mount.LoadDefaultValues(DefaultValues.Elk);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(7, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Track, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Addax", mount.KeyWords[0]);
            Assert.AreEqual("Elk", mount.KeyWords[1]);
            Assert.AreEqual("The elk has a large rack with two reigns tied off on a saddle.", mount.ShortDescription);
            Assert.AreEqual("elk", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Panther()
        {
            mount.LoadDefaultValues(DefaultValues.Panther);

            Assert.AreEqual(5, mount.Movement);
            Assert.AreEqual(5, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Track, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Storm", mount.KeyWords[0]);
            Assert.AreEqual("Panther", mount.KeyWords[1]);
            Assert.AreEqual("Yellow eyes almost glow against the black panthers fur.", mount.ShortDescription);
            Assert.AreEqual("panther", mount.SentenceDescription);
        }

        [TestMethod]
        public void Mount_LoadDefaultValues_Griffin()
        {
            mount.LoadDefaultValues(DefaultValues.Griffin);

            Assert.AreEqual(3, mount.Movement);
            Assert.AreEqual(7, mount.StaminaMultiplier);
            Assert.AreEqual(CallType.Summon, mount.TypeOfCall);
            Assert.AreEqual(1, mount.MaxRiders);
            Assert.AreEqual("Apollo", mount.KeyWords[0]);
            Assert.AreEqual("Griffin", mount.KeyWords[1]);
            Assert.AreEqual("A majestic griffin stands at the ready.", mount.ShortDescription);
            Assert.AreEqual("griffin", mount.SentenceDescription);
        }
    }
}
