using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public RoomId(IRoom room) : base(room.Zone, room.Id)
        {

        }


        public override string ToString()
        {
            return string.Format("{0}-{1}", Zone, Id);
        }
    }
}