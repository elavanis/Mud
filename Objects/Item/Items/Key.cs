using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Key : Item, IKey
    {
        [ExcludeFromCodeCoverage]
        public int ZoneNumber { get; set; }

        [ExcludeFromCodeCoverage]
        public int KeyNumber { get; set; }
    }
}
