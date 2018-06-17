using Objects.Command.Interface;
using Objects.Global.Direction;
using Objects.Mob.Interface;
using Objects.Room.Interface;

namespace Objects.Command.PC.Interface
{
    public interface IMove
    {
        IResult MoveToRoom(IMobileObject performer, IRoom room, Directions.Direction direction, IRoom proposedRoom);
    }
}