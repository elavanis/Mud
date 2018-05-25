using Objects.Item.Items;
using Objects.Item.Items.Interface;

namespace Objects.Room.Interface
{
    public interface IExit
    {
        IDoor Door { get; set; }
        int Room { get; set; }
        int Zone { get; set; }
    }
}