using Objects.Interface;
using Objects.Item.Interface;
using static Objects.Global.Direction.Directions;

namespace Objects.Item.Items.Interface
{
    public interface IDoor : IItem, IOpenable
    {
        int KeyNumber { get; set; }
        bool Locked { get; set; }
        bool Pickable { get; set; }
        int PickDificulty { get; set; }
        bool Linked { get; set; }
        IBaseObjectId LinkedRoomId { get; set; }
        Direction LinkedRoomDirection { get; set; }

    }
}