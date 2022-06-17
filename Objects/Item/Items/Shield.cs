using Objects.Die.Interface;
using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Shield : Armor, IShield
    {
        public Shield(IDice dice, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(dice, AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        public Shield(int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(level, AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        [ExcludeFromCodeCoverage]
        public int NegateDamagePercent { get; set; }
    }
}
