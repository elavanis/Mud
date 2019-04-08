using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Room
{
    public class Exit : IExit
    {
        [ExcludeFromCodeCoverage]
        public int Zone { get; set; }
        [ExcludeFromCodeCoverage]
        public int Room { get; set; }
        [ExcludeFromCodeCoverage]
        public IDoor Door { get; set; }
    }
}
