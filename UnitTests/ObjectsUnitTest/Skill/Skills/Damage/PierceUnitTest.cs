using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Damage;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class PierceUnitTest
    {
        Pierce pierce;
        Mock<IMobileObject> mob;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mob = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValue.Setup(e => e.DiceForSkillLevel(20)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            pierce = new Pierce();
        }

        [TestMethod]
        public void Pierce_TeachMessage()
        {
            string expected = "Keep the pointy end of the weapon toward your opponent and thrust.";
            Assert.AreEqual(expected, pierce.TeachMessage);
        }

        [TestMethod]
        public void Pierce_WriteTests()
        {
            Assert.AreEqual(1, 2);
        }

        //[TestMethod]
        //public void Pierce_MeetRequirments_Fail()
        //{
        //    Assert.IsFalse(pierce.MeetRequirments(mob.Object, mob.Object));
        //}

        //[TestMethod]
        //public void Pierce_MeetRequirments_Pass()
        //{
        //    List<IWeapon> weapons = new List<IWeapon>();
        //    Mock<IWeapon> weapon = new Mock<IWeapon>();
        //    Mock<IDamage> damage = new Mock<IDamage>();

        //    weapons.Add(weapon.Object);
        //    weapon.Setup(e => e.DamageList).Returns(new List<IDamage>() { damage.Object });
        //    damage.Setup(e => e.Type).Returns(Objects.Damage.Damage.DamageType.Pierce);
        //    mob.Setup(e => e.EquipedWeapon).Returns(weapons);

        //    Assert.IsTrue(pierce.MeetRequirments(mob.Object, mob.Object));
        //}

        [TestMethod]
        public void Pierce_RequirementsFailureMessage()
        {
            IResult result = pierce.RequirementsFailureMessage;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You must have a piercing weapon equipped.", result.ResultMessage);
        }
    }
}
