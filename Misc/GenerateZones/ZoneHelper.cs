using Objects;
using Objects.Global;
using Objects.Item.Items;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Global.Direction.Directions;

namespace GenerateZones
{
    public static class ZoneHelper
    {
        public static void ConnectZone(IRoom room, Direction roomExitDirection, int zoneId, int roomId, DoorInfo doorInfo = null)
        {
            IRoom dummyRoom = new Room();
            dummyRoom.Zone = zoneId;
            dummyRoom.Id = roomId;
            ConnectRoom(room, roomExitDirection, dummyRoom, doorInfo);
        }

        public static void ConnectRoom(IRoom room1, Direction room1ExitDirection, IRoom room2, DoorInfo doorInfo = null)
        {
            AddExitToRoom(room1, room1ExitDirection, room2, doorInfo);
            AddExitToRoom(room2, ReverseDirection(room1ExitDirection), room1, doorInfo);
        }

        public static void AddRoom(IZone zone, IRoom room)
        {
            zone.Rooms.Add(room.Id, room);
        }

        private static Direction ReverseDirection(Direction room1ExitDirection)
        {
            switch (room1ExitDirection)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
            }

            //should never hit
            return Direction.North;
        }

        private static void AddExitToRoom(IRoom room1, Direction room1ExitDirection, IRoom room2, DoorInfo doorInfo)
        {
            IExit exit = new Exit();
            exit.Room = room2.Id;
            exit.Zone = room2.Zone;
            switch (room1ExitDirection)
            {
                case Direction.North:
                    if (room1.North != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.North = exit;
                    break;
                case Direction.East:
                    if (room1.East != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.East = exit;
                    break;
                case Direction.South:
                    if (room1.South != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.South = exit;
                    break;
                case Direction.West:
                    if (room1.West != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.West = exit;
                    break;
                case Direction.Up:
                    if (room1.Up != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.Up = exit;
                    break;
                case Direction.Down:
                    if (room1.Down != null)
                    {
                        throw new Exception("Exit already assigned");
                    }
                    room1.Down = exit;
                    break;
            }

            if (doorInfo != null)
            {
                exit.Door = new Door();
                exit.Door.Zone = room1.Zone;    //needed to pass verification
                exit.Door.Id = room1.Id;        //needed to pass verification
                exit.Door.KeyWords.Add(doorInfo.Name);
                exit.Door.ExamineDescription = doorInfo.Description;
                exit.Door.LongDescription = doorInfo.Description;
                exit.Door.ShortDescription = doorInfo.Name;
                exit.Door.SentenceDescription = doorInfo.Name;
                exit.Door.OpenMessage = doorInfo.OpenMessage;
                exit.Door.Linked = doorInfo.Linked;
                if (doorInfo.Linked)
                {
                    exit.Door.LinkedRoomId = new BaseObjectId() { Id = room2.Id, Zone = room2.Zone };
                    exit.Door.LinkedRoomDirection = ReverseDirection(room1ExitDirection);
                }
            }
        }
    }
}
