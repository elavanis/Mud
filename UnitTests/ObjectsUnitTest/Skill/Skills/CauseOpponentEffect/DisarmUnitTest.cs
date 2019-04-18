using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.CauseOpponentEffect;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.CauseOpponentEffect
{
    [TestClass]

    public class DisarmUnitTest
    {
        Disarm disarm;
        Mock<IMobileObject> mob;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IWeapon> weapon;
        Mock<IRandom> random;
        List<IItem> heldItems;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mob = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            weapon = new Mock<IWeapon>();
            target = new Mock<IMobileObject>();
            performer = new Mock<IMobileObject>();
            random = new Mock<IRandom>();
            heldItems = new List<IItem>();

            defaultValue.Setup(e => e.DiceForSkillLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            target.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            target.Setup(e => e.Items).Returns(heldItems);
            weapon.Setup(e => e.KeyWords).Returns(new List<string>() { "Sword" });
            random.SetupSequence(e => e.Next(0)).Returns(1).Returns(2);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Random = random.Object;

            disarm = new Disarm();
        }

        [TestMethod]
        public void Disarm_TeachMessage()
        {
            string expected = "If your opponent has no weapon then they can't hurt you.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_MeetRequirments_True()
        {
            Assert.IsTrue(disarm.MeetRequirments(performer.Object, target.Object));
        }

        [TestMethod]
        public void Disarm_MeetRequirments_False()
        {
            weapon.Setup(e => e.KeyWords).Returns(new List<string>() { "BareHands" });
            Assert.IsFalse(disarm.MeetRequirments(performer.Object, target.Object));
        }

        [TestMethod]
        public void Disarm_RequirementsFailureMessage()
        {
            IResult result = disarm.RequirementsFailureMessage;
            string expected = "You can not disarm an unarmed opponent.";
            Assert.AreEqual(expected, result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Disarm_IsSuccessful_True()
        {
            random.SetupSequence(e => e.Next(0)).Returns(2).Returns(1);
            Assert.IsTrue(disarm.IsSuccessful(performer.Object, target.Object));
        }

        [TestMethod]
        public void Disarm_IsSuccessful_False()
        {
            Assert.IsFalse(disarm.IsSuccessful(performer.Object, target.Object));
        }

        [TestMethod]
        public void Disarm_AdditionalEffect()
        {
            disarm.AdditionalEffect(performer.Object, target.Object);

            target.Verify(e => e.RemoveEquipment(weapon.Object), Times.Once);
        }
    }
}