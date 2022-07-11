using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Skill.Skills.Damage;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace ObjectsUnitTest.Skill.Skills.Damage
{
    [TestClass]
    public class BaseDamageSkillUnitTest
    {
        LocalBaseDamageSkill baseDamage;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;

        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();

            defaultValues.Setup(e => e.ReduceValues(10, 20)).Returns(dice.Object);

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;

            baseDamage = new LocalBaseDamageSkill("localBaseSkill", 10, 20, DamageType.Bludgeon);
        }

        [TestMethod]
        public void BaseDamageSkill_Constructor_DefaultStamina()
        {
            VerifyBaseDamage(10);
        }

        [TestMethod]
        public void BaseDamageSkill_Constructor_SpecifiedStamina()
        {
            baseDamage = new LocalBaseDamageSkill("localBaseSkill", 10, 20, DamageType.Bludgeon, 1);
            VerifyBaseDamage(1);
        }

        private void VerifyBaseDamage(int staminaCost)
        {
            IEffect effect = baseDamage.Effect;
            IDamage damage = baseDamage.Parameter.Damage;

            Assert.IsNotNull(effect as Objects.Effect.Damage);
            Assert.AreSame(dice.Object, baseDamage.Parameter.Dice);
            Assert.AreEqual(DamageType.Bludgeon, damage.Type);
            Assert.AreEqual(staminaCost, baseDamage.StaminaCost);
        }

        private class LocalBaseDamageSkill : BaseDamageSkill
        {
            public LocalBaseDamageSkill(string skillName, int die, int sides, DamageType damageType, int staminaCost = -1)
                : base(skillName, die, sides, damageType, staminaCost)
            {

            }
        }
    }
}
