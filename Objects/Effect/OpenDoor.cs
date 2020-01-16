using Objects.Effect.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace Objects.Effect
{
    public class OpenDoor : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }


        [ExcludeFromCodeCoverage]
        public OpenDoor()
        {

        }

        public OpenDoor(ISound sound)
        {
            Sound = sound;
        }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.ObjectRoom != null)
            {
                IRoom room = GlobalReference.GlobalValues.World.Zones[parameter.RoomId.Zone].Rooms[parameter.RoomId.Id];
                IDoor door = null;
                IRoom otherRoom = null;
                IDoor otherDoor = null;

                switch (parameter.Direction)
                {
                    case Direction.North:
                        door = room.North.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.North.Zone].Rooms[room.North.Room];
                        otherDoor = otherRoom.South.Door;
                        break;
                    case Direction.East:
                        door = room.East.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.East.Zone].Rooms[room.East.Room];
                        otherDoor = otherRoom.West.Door;
                        break;
                    case Direction.South:
                        door = room.South.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.South.Zone].Rooms[room.South.Room];
                        otherDoor = otherRoom.North.Door;
                        break;
                    case Direction.West:
                        door = room.West.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.West.Zone].Rooms[room.West.Room];
                        otherDoor = otherRoom.East.Door;
                        break;
                    case Direction.Up:
                        door = room.Up.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.Up.Zone].Rooms[room.Up.Room];
                        otherDoor = otherRoom.Down.Door;
                        break;
                    case Direction.Down:
                        door = room.Down.Door;
                        otherRoom = GlobalReference.GlobalValues.World.Zones[room.Down.Zone].Rooms[room.Down.Room];
                        otherDoor = otherRoom.Up.Door;
                        break;
                }

                if (door != null)
                {
                    door.Opened = true;
                }

                if (otherDoor != null)
                {
                    otherDoor.Opened = true;
                }
            }
        }
    }
}
