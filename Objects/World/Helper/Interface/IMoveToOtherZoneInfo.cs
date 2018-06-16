using Objects.Mob.Interface;
using Objects.Room.Interface;
using static Objects.Global.Direction.Directions;

namespace Objects.World.Helper.Interface
{
    public interface IMoveToOtherZoneInfo
    {
        IMobileObject Performer { get; set; }
        IRoom ProposedRoom { get; set; }
        Direction Direction { get; set; }
    }
}