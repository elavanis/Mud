using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maps
{
    public class MapRoom
    {
        public IRoom North { get; set; }
        public IRoom East { get; set; }
        public IRoom South { get; set; }
        public IRoom West { get; set; }
        public IRoom Up { get; set; }
        public IRoom Down { get; set; }
        public Position Position { get; set; }

        public MapRoom()
        {

        }

        public MapRoom(IZone zone, IRoom room, Position position)
        {
            if (room.North?.Zone == zone.Id)
            {
                North = zone.Rooms[room.North.Room];
            }

            if (room.East?.Zone == zone.Id)
            {
                East = zone.Rooms[room.East.Room];
            }

            if (room.South?.Zone == zone.Id)
            {
                South = zone.Rooms[room.South.Room];
            }

            if (room.West?.Zone == zone.Id)
            {
                West = zone.Rooms[room.West.Room];
            }

            if (room.Up?.Zone == zone.Id)
            {
                Up = zone.Rooms[room.Up.Room];
            }

            if (room.Down?.Zone == zone.Id)
            {
                Down = zone.Rooms[room.Down.Room];
            }

            Position = position;
        }
    }
}
