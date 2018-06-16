using Objects.Global.Direction;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Helper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace Objects.World.Helper
{
    public class MoveToOtherZoneInfo : IMoveToOtherZoneInfo
    {
        public IMobileObject Performer { get; set; }
        public IRoom ProposedRoom { get; set; }
        public Directions.Direction Direction { get; set; }

        public MoveToOtherZoneInfo(IMobileObject performer, IRoom proposedRoom, Direction direction)
        {
            Performer = performer;
            ProposedRoom = proposedRoom;
            Direction = direction;
        }



    }
}
