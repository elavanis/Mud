using Objects.Die.Interface;
using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Shield : Armor, IShield
    {
        public Shield(IDice dice, AvalableItemPosition position, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(dice, position, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            ItemPosition = AvalableItemPosition.Wield;
        }

        public Shield(int level, AvalableItemPosition position, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(level, position, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            ItemPosition = AvalableItemPosition.Wield;
        }

        [ExcludeFromCodeCoverage]
        public int NegateDamagePercent { get; set; }
    }
}
