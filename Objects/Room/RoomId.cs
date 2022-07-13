using Objects.Room.Interface;

namespace Objects.Room
{
    public class RoomId : BaseObjectId
    {
        public RoomId() : base()
        {

        }

        public RoomId(int zone, int roomId) : base(zone, roomId)
        {

        }

        public RoomId(IRoom room) : base(room.ZoneId, room.Id)
        {

        }


        public override string ToString()
        {
            return string.Format("{0}-{1}", Zone, Id);
        }
    }
}