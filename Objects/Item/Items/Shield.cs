using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Shield : Armor, IShield
    {
        public Shield()
        {
            ItemPosition = AvalableItemPosition.Wield;
        }

        [ExcludeFromCodeCoverage]
        public int NegateDamagePercent { get; set; }
    }
}
