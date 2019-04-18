using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Damage;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Objects.Damage.Damage;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class ThicketOfBladesUnitTests
    {
        ThicketOfBlades thicketOfBlades;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IWeapon> weapon;
        Mock<IDamage> damage;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            weapon = new Mock<IWeapon>();
            damage = new Mock<IDamage>();

            defaultValue.Setup(e => e.DiceForSkillLevel(90)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            performer.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            weapon.Setup(e => e.DamageList).Returns(new List<IDamage>() { damage.Object });
            damage.Setup(e => e.Type).Returns(DamageType.Slash);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            thicketOfBlades = new ThicketOfBlades();
        }

        [TestMethod]
        public void ThicketOfBlades_TeachMessage()
        {
            string expected = "Move your blade in a circular path like this.  Good now faster, faster.  Perfect.";
            Assert.AreEqual(expected, thicketOfBlades.TeachMessage);
        }

        //[TestMethod]
        //public void ThicketOfBlades_MeetRequirements_True()
        //{
        //    Assert.IsTrue(thicketOfBlades.MeetRequirments(performer.Object, target.Object));
        //}

        //[TestMethod]
        //public void ThicketOfBlades_MeetRequirements_False()
        //{
        //    damage.Setup(e => e.Type).Returns(DamageType.Pierce);

        //    Assert.IsFalse(thicketOfBlades.MeetRequirments(performer.Object, target.Object));
        //}

        [TestMethod]
        public void ThicketOfBlades_WriteTests()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
