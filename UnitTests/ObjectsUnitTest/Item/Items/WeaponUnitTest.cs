using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Damage.Interface;
using Objects.Item.Items;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class WeaponUnitTest
    {
        Weapon weapon;

        [TestInitialize]
        public void Setup()
        {
            weapon = new Weapon();
        }

        [TestMethod]
        public void Weapon_Constructor()
        {
            Assert.AreEqual(Equipment.AvalableItemPosition.Wield, weapon.ItemPosition);
        }

        [TestMethod]
        public void Weapon_DamageList_Blank()
        {
            Assert.IsNotNull(weapon.DamageList);
            Assert.AreEqual(0, weapon.DamageList.Count);
        }

        [TestMethod]
        public void Weapon_DamageList_Populated()
        {
            Mock<IDamage> damage = new Mock<IDamage>();
            weapon.DamageList.Add(damage.Object);

            Assert.AreEqual(1, weapon.DamageList.Count);
            Assert.AreSame(damage.Object, weapon.DamageList[0]);
        }
    }
}
