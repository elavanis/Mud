using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using static Objects.Item.Items.Weapon;

namespace Objects.Global.Random.Interface
{
    public interface IRandomDropGenerator
    {
        IItem GenerateRandomDrop(INonPlayerCharacter nonPlayerCharacter);
        IEquipment GenerateRandomEquipment(int level, int effectiveLevel);
        IWeapon GenerateRandomWeapon(int level, int effectiveLevel, WeaponType weaponType);
    }
}