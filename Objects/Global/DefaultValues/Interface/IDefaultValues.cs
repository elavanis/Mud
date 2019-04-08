
using Objects.Die.Interface;

namespace Objects.Global.DefaultValues.Interface
{
    public interface IDefaultValues
    {
        IDice DiceForArmorLevel(int level);
        IDice DiceForWeaponLevel(int level);
        IDice DiceForTrapLevel(int level, int percentOfAveragePcHealth = 100);
        IDice DiceForSpellLevel(int level);
        IDice DiceForSkillLevel(int level);

        IDice ReduceValues(int die, int sides);
        uint MoneyForNpcLevel(int level);
    }
}