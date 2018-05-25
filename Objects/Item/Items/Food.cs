using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Food : IFood
    {
        [ExcludeFromCodeCoverage]
        public int HungerSatisifaction { get; set; }

        [ExcludeFromCodeCoverage]
        public int ThirstSatisifaction { get; set; }
    }
}
