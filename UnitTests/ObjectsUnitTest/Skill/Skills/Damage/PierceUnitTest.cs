using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.StringManuplation.Interface;
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
    public class PierceUnitTest
    {
        Pierce pierce;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<ICommand> command;
        Mock<IParameter> parameterCommandName;
        Mock<IParameter> parameterTarget;
        Mock<IFindObjects> findObjects;
        Mock<IWeapon> weapon;
        Mock<IDamage> damage;
        Mock<INotify> notify;
        Mock<IEffect> effect;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            command = new Mock<ICommand>();
            parameterCommandName = new Mock<IParameter>();
            parameterTarget = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            weapon = new Mock<IWeapon>();
            damage = new Mock<IDamage>();
            notify = new Mock<INotify>();
            effect = new Mock<IEffect>();
            stringManipulator = new Mock<IStringManipulator>();

            defaultValue.Setup(e => e.DiceForSkillLevel(20)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterCommandName.Object, parameterTarget.Object });
            parameterCommandName.Setup(e => e.ParameterValue).Returns("commandName");
            parameterTarget.Setup(e => e.ParameterValue).Returns("target");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(performer.Object, parameterTarget.Object.ParameterValue, 0, true, true, true, true, true)).Returns(target.Object);
            performer.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            weapon.Setup(e => e.DamageList).Returns(new List<IDamage>() { damage.Object });
            damage.Setup(e => e.Type).Returns(DamageType.Pierce);

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            pierce = new Pierce();
            pierce.Effect = effect.Object;

        }

        [TestMethod]
        public void Pierce_TeachMessage()
        {
            string expected = "Keep the pointy end of the weapon toward your opponent and thrust.";
            Assert.AreEqual(expected, pierce.TeachMessage);
        }

        [TestMethod]
        public void Pierce_PerformAction_MeetRequirment_Unsucessful()
        {
            damage.Setup(e => e.Type).Returns(DamageType.Bludgeon);

            IResult result = pierce.ProcessSkill(performer.Object, command.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You must have a piercing weapon equipped.", result.ResultMessage);
        }

        [TestMethod]
        public void Pierce_PerformAction_MeetRequirment_Sucessful()
        {
            IResult result = pierce.ProcessSkill(performer.Object, command.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
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
