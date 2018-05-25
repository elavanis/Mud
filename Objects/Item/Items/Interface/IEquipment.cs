using Objects.Item.Interface;

namespace Objects.Item.Items.Interface
{
    public interface IEquipment : IItem
    {
        int Charisma { get; set; }
        int Constitution { get; set; }
        int Dexterity { get; set; }
        int Intelligence { get; set; }
        Equipment.AvalableItemPosition ItemPosition { get; set; }
        int MaxHealth { get; set; }
        int MaxMana { get; set; }
        int MaxStamina { get; set; }
        int Strength { get; set; }
        int Wisdom { get; set; }
    }
}