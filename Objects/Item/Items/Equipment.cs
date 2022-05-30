using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Equipment : Item, IEquipment
    {
        public Equipment(AvalableItemPosition position, int id, int zone, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(id, zone, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            ItemPosition = position;
        }

        [ExcludeFromCodeCoverage]
        public AvalableItemPosition ItemPosition { get; set; } = AvalableItemPosition.Held;
        public enum AvalableItemPosition
        {
            Wield,
            Head,
            Neck,
            Arms,
            Hand,
            Finger,
            Body,
            Waist,
            Legs,
            Feet,
            Held,
            NotWorn
        }

        [ExcludeFromCodeCoverage]
        public int MaxHealth { get; set; }
        [ExcludeFromCodeCoverage]
        public int MaxMana { get; set; }
        [ExcludeFromCodeCoverage]
        public int MaxStamina { get; set; }

        [ExcludeFromCodeCoverage]
        public int Strength { get; set; }
        [ExcludeFromCodeCoverage]
        public int Dexterity { get; set; }
        [ExcludeFromCodeCoverage]
        public int Constitution { get; set; }
        [ExcludeFromCodeCoverage]
        public int Intelligence { get; set; }
        [ExcludeFromCodeCoverage]
        public int Wisdom { get; set; }
        [ExcludeFromCodeCoverage]
        public int Charisma { get; set; }
    }
}
