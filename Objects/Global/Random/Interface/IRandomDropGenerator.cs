using Objects.Item.Interface;
using Objects.Mob.Interface;

namespace Objects.Global.Random.Interface
{
    public interface IRandomDropGenerator
    {
        IItem GenerateRandomDrop(INonPlayerCharacter nonPlayerCharacter);
        IItem GenerateRandomEquipment(int level, int effectiveLevel);
    }
}