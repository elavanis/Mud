using Objects.Room.Interface;
using Objects.Zone.Interface;
using System.Collections.Generic;

namespace Maps
{
    public class MapGrid
    {
        private Stack<RoomPositionInfo> roomStack = new Stack<RoomPositionInfo>();
        private HashSet<string> hashPositions = new HashSet<string>();
        public Dictionary<IRoom, MapRoom> Grid { get; set; } = new Dictionary<IRoom, MapRoom>();
        public Dictionary<IRoom, MapRoom> BuildRooms(IZone zone)
        {
            int firstRoomFound = 0;
            foreach (var item in zone.Rooms.Keys)
            {
                firstRoomFound = item;
                break;
            }

            IRoom room = zone.Rooms[firstRoomFound];
            Grid.Clear();
            AddRoom(zone, room, 1, 1, 1);

            return Grid;
        }


        private void AddRoom(IZone zone, IRoom room, int x, int y, int z)
        {
            roomStack.Push(new RoomPositionInfo(room, x, y, z));
            while (roomStack.Count != 0)
            {
                RoomPositionInfo roomPositionInfo = roomStack.Pop();
                room = roomPositionInfo.Room;
                x = roomPositionInfo.X;
                y = roomPositionInfo.Y;
                z = roomPositionInfo.Z;


                if (Grid.ContainsKey(room))
                {
                    //return;
                }
                else
                {
                    MapRoom mapRoom = new MapRoom(zone, room, new Position(x, y, z));
                    Grid.Add(room, mapRoom);
                    hashPositions.Add(mapRoom.Position.ToString());
                }

                if (room.North != null)
                {
                    IExit exit = room.North;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;
                            while (CheckPosition(currentPosition.X, currentPosition.Y + 1, currentPosition.Z))
                            {
                                MigrateSouth(currentPosition.Y);
                            }

                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X, currentPosition.Y + 1, currentPosition.Z));
                        }
                    }
                }

                if (room.East != null)
                {
                    IExit exit = room.East;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;

                            while (CheckPosition(currentPosition.X + 1, currentPosition.Y, currentPosition.Z))
                            {
                                MigrateWest(currentPosition.X);
                            }
                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X + 1, currentPosition.Y, currentPosition.Z));
                        }
                    }
                }

                if (room.South != null)
                {
                    IExit exit = room.South;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;
                            while (CheckPosition(currentPosition.X, currentPosition.Y - 1, currentPosition.Z))
                            {
                                MigrateNorth(currentPosition.Y);
                            }
                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X, currentPosition.Y - 1, currentPosition.Z));
                        }
                    }
                }

                if (room.West != null)
                {
                    IExit exit = room.West;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;
                            while (CheckPosition(currentPosition.X - 1, currentPosition.Y, currentPosition.Z))
                            {
                                MigrateEast(currentPosition.X);
                            }
                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X - 1, currentPosition.Y, currentPosition.Z));
                        }
                    }
                }

                if (room.Up != null)
                {
                    IExit exit = room.Up;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;
                            while (CheckPosition(currentPosition.X, currentPosition.Y, currentPosition.Z + 1))
                            {
                                MigrateDown(currentPosition.Z);
                            }
                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X, currentPosition.Y, currentPosition.Z + 1));
                        }
                    }
                }

                if (room.Down != null)
                {
                    IExit exit = room.Down;
                    if (exit.Zone == room.ZoneId)
                    {
                        IRoom newRoom = zone.Rooms[exit.Room];
                        if (!Grid.ContainsKey(newRoom))
                        {
                            Position currentPosition = Grid[room].Position;
                            while (CheckPosition(currentPosition.X, currentPosition.Y, currentPosition.Z - 1))
                            {
                                MigrateUp(currentPosition.Z);
                            }
                            roomStack.Push(new RoomPositionInfo(newRoom, currentPosition.X, currentPosition.Y, currentPosition.Z - 1));
                        }

                    }
                }
            }
        }

        private bool CheckPosition(int x, int y, int z)
        {
            if (hashPositions.Contains(new Position(x, y, z).ToString()))
            {
                return true;
            }

            return false;
        }

        private void MigrateNorth(int y)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.Y >= y)
                {
                    room.Position.Y++;
                }
            }

            ResetStack();
        }

        private void MigrateEast(int x)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.X >= x)
                {
                    room.Position.X++;
                }
            }

            ResetStack();
        }

        private void MigrateSouth(int y)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.Y <= y)
                {
                    room.Position.Y--;
                }
            }

            ResetStack();
        }

        private void MigrateWest(int x)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.X <= x)
                {
                    room.Position.X--;
                }
            }

            ResetStack();
        }

        private void MigrateDown(int z)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.Z <= z)
                {
                    room.Position.Z--;
                }
            }

            ResetStack();
        }

        private void MigrateUp(int z)
        {
            foreach (MapRoom room in Grid.Values)
            {
                if (room.Position.Z >= z)
                {
                    room.Position.Z++;
                }
            }

            ResetStack();
        }

        private void ResetStack()
        {
            roomStack.Clear();
            hashPositions.Clear();
            foreach (IRoom room in Grid.Keys)
            {
                MapRoom mapRoom = Grid[room];
                roomStack.Push(new RoomPositionInfo(room, mapRoom.Position.X, mapRoom.Position.Y, mapRoom.Position.Z));
                hashPositions.Add(mapRoom.Position.ToString());
            }
        }

        private class RoomPositionInfo
        {
            public IRoom Room { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public RoomPositionInfo(IRoom room, int x, int y, int z)
            {
                Room = room;
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}