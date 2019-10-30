using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Custom.MountainGoblinCamp;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities.Custom.MountainGoblinCamp
{
    [TestClass]
    public class ChiefDaughterPresentUnitTest
    {
        ChiefDaughterPresent chiefDaughterPresent;
        Mock<INonPlayerCharacter> chief;
        Mock<INonPlayerCharacter> daughter;
        Mock<IRoom> room;
        Mock<IRandom> random;
        Mock<IRandomDropGenerator> randomDropGenerator;
        Mock<IEquipment> equipment;
        List<IItem> items;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            chief = new Mock<INonPlayerCharacter>();
            daughter = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            random = new Mock<IRandom>();
            randomDropGenerator = new Mock<IRandomDropGenerator>();
            equipment = new Mock<IEquipment>();
            items = new List<IItem>();

            chief.Setup(e => e.Items).Returns(items);
            chief.Setup(e => e.KeyWords).Returns(new List<string>() { "chief" });
            chief.Setup(e => e.Room).Returns(room.Object);
            daughter.Setup(e => e.FollowTarget).Returns(chief.Object);
            daughter.Setup(e => e.Zone).Returns(23);
            daughter.Setup(e => e.Id).Returns(10);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { chief.Object, daughter.Object });
            randomDropGenerator.Setup(e => e.GenerateRandomEquipment(0, 0)).Returns(equipment.Object);
            equipment.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });

            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.RandomDropGenerator = randomDropGenerator.Object;

            chiefDaughterPresent = new ChiefDaughterPresent();
        }

        [TestMethod]
        public void ChiefDaughterPresenUnitTest_DaughterPresetAndFollowing()
        {
            chiefDaughterPresent.Process(chief.Object, null);

            chief.Verify(e => e.EnqueueCommand("say You have brought my daughter back to me.  I am most grateful."), Times.Once);
            chief.Verify(e => e.EnqueueCommand("say Please accept this gift as a reward."), Times.Once);
            chief.Verify(e => e.EnqueueCommand("say Servant, fetch me my best keyword for our hero."), Times.Once);
            chief.Verify(e => e.EnqueueCommand("Wait"), Times.Exactly(2));
            Assert.IsTrue(items.Contains(equipment.Object));
            chief.Verify(e => e.EnqueueCommand("say Please accept this keyword as a reward for rescuing my daughter."), Times.Once);
            chief.Verify(e => e.EnqueueCommand("Give keyword chief"), Times.Once);
            daughter.VerifySet(e => e.FollowTarget = null);

        }
    }
}