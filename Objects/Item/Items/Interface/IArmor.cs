using Objects.Die.Interface;
using Objects.Material.Interface;

namespace Objects.Item.Items.Interface
{
    public interface IArmor : IEquipment
    {
        decimal Acid { get; set; }
        decimal Bludgeon { get; set; }
        decimal Cold { get; set; }
        decimal Fire { get; set; }
        decimal Force { get; set; }
        decimal Lightning { get; set; }
        decimal Necrotic { get; set; }
        decimal Pierce { get; set; }
        decimal Poison { get; set; }
        decimal Psychic { get; set; }
        decimal Radiant { get; set; }
        decimal Slash { get; set; }
        decimal Thunder { get; set; }

        IDice Dice { get; set; }
        IMaterial Material { get; set; }

        decimal GetTypeModifier(Damage.Damage.DamageType damageType);
    }
}
