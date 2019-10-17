using RandomZone.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomZone
{
    public class Maze : Internal.RandomZone
    {
        private Position beginPos = null;
        private Position endPos = null;
        public void Generate(int x, int y, int fillPercent = 100, int randomSeed = -1)
        {
            Initilize(x, y, randomSeed);


            List<Position> path = CreateMazePath(rooms);
            beginPos = path[0];
            endPos = path[path.Count - 1];

            BuildRestOfRooms(rooms, fillPercent);
        }

        private List<Position> CreateMazePath(Room[,] rooms)
        {
            List<Position> path = new List<Position>();
            int totalRooms = rooms.GetLength(0) * rooms.GetLength(1);
            int minRooms = (rooms.GetLength(0) + rooms.GetLength(1));

            int beginX = random.Next(rooms.GetLength(0));
            int beginY = 0;
            if (beginX == 0 || beginX == rooms.GetLength(0) - 1)
            {
                beginY = random.Next(rooms.GetLength(1));
            }
            else
            {
                if (random.Next(2) == 0)
                {
                    beginY = 0;
                }
                else
                {
                    beginY = rooms.GetLength(1) - 1;
                }
            }

            //beginX = 0;
            //beginY = 0;

            Room room = new Room();
            rooms[beginX, beginY] = room;
            Position beginPosition = new Position(beginX, beginY);
            path.Add(beginPosition);


            bool done = false;
            //build the maze path out
            while (!done)
            {
                done = AddToPath(rooms, path, minRooms);
            }

            path.Remove(path.Last());
            return path;
        }

        private bool AddToPath(Room[,] rooms, List<Position> path, int minRooms)
        {
            Position currentPos = path[path.Count - 1];
            bool mazeEnds = false;

            //get the directions it can go
            bool CanEndMaze = path.Count >= minRooms;
            List<Directions.Direction> dir = GetRoomDirectionsAvailableToGo(rooms, currentPos, CanEndMaze);

            if (dir.Count > 0)
            {
                //pick one
                Directions.Direction direction = dir[random.Next(dir.Count)];
                mazeEnds = OpenDoor(currentPos.X, currentPos.Y, direction);
                switch (direction)
                {
                    case Directions.Direction.North:
                        currentPos = new Position(currentPos.X, currentPos.Y - 1);
                        break;
                    case Directions.Direction.South:
                        currentPos = new Position(currentPos.X, currentPos.Y + 1);
                        break;
                    case Directions.Direction.East:
                        currentPos = new Position(currentPos.X + 1, currentPos.Y);
                        break;
                    case Directions.Direction.West:
                        currentPos = new Position(currentPos.X - 1, currentPos.Y);
                        break;
                }
                path.Add(currentPos);
            }
            else
            {
                path.RemoveAt(path.Count - 1);
            }
            return mazeEnds;
        }

        private List<Directions.Direction> GetRoomDirectionsAvailableToGo(Room[,] rooms, Position currentPos, bool CanEndMaze)
        {
            List<Directions.Direction> dir = new List<Directions.Direction>();
            if (CanGoNorth(rooms, currentPos, CanEndMaze))
            {
                dir.Add(Directions.Direction.North);
            }

            if (CanGoSouth(rooms, currentPos, CanEndMaze))
            {
                dir.Add(Directions.Direction.South);
            }

            if (CanGoEast(rooms, currentPos, CanEndMaze))
            {
                dir.Add(Directions.Direction.East);
            }

            if (CanGoWest(rooms, currentPos, CanEndMaze))
            {
                dir.Add(Directions.Direction.West);
            }
            return dir;
        }

        private bool CanGoNorth(Room[,] rooms, Position currentPos, bool canEndMaze)
        {
            bool result = false;
            int newPos = currentPos.Y - 1;
            //make sure we don't go outside the maze boundry
            if (newPos >= 0)
            {
                if (rooms[currentPos.X, newPos] == null)
                {
                    result = true;
                }
            }
            else
            {
                //we are going to be outside the maze boundry so we check to see if we can end the maze
                if (canEndMaze)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool CanGoSouth(Room[,] rooms, Position currentPos, bool canEndMaze)
        {
            bool result = false;
            int newPos = currentPos.Y + 1;
            //make sure we don't go outside the maze boundry
            if (newPos < rooms.GetLength(1))
            {
                if (rooms[currentPos.X, newPos] == null)
                {
                    result = true;
                }
            }
            else
            {
                //we are going to be outside the maze boundry so we check to see if we can end the maze
                if (canEndMaze)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool CanGoEast(Room[,] rooms, Position currentPos, bool canEndMaze)
        {
            bool result = false;
            int newPos = currentPos.X + 1;
            //make sure we don't go outside the maze boundry
            if (newPos < rooms.GetLength(0))
            {
                if (rooms[newPos, currentPos.Y] == null)
                {
                    result = true;
                }
            }
            else
            {
                //we are going to be outside the maze boundry so we check to see if we can end the maze
                if (canEndMaze)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool CanGoWest(Room[,] rooms, Position currentPos, bool canEndMaze)
        {
            bool result = false;
            int newPos = currentPos.X - 1;
            //make sure we don't go outside the maze boundry
            if (newPos >= 0)
            {
                if (rooms[newPos, currentPos.Y] == null)
                {
                    result = true;
                }
            }
            else
            {
                //we are going to be outside the maze boundry so we check to see if we can end the maze
                if (canEndMaze)
                {
                    result = true;
                }
            }
            return result;
        }

        protected void BuildRestOfRooms(Room[,] rooms, int fillPercent)
        {
            List<Position> listOfAvaibleRoomsToOpenDoors = new List<Position>();
            for (int x = 0; x < rooms.GetLength(0); x++)
            {
                for (int y = 0; y < rooms.GetLength(1); y++)
                {
                    if (rooms[x, y] != null)
                    {
                        listOfAvaibleRoomsToOpenDoors.Add(new Position(x, y));
                    }
                }
            }

            List<Position> listOfPopulatedRooms = new List<Position>(listOfAvaibleRoomsToOpenDoors);


            int doneRooms = 0;
            int totalRooms = rooms.GetLength(0) * rooms.GetLength(1);

            int roomsToFill = totalRooms;
            if (fillPercent < 100)
            {
                roomsToFill = totalRooms * fillPercent / 100;
            }

            while (listOfAvaibleRoomsToOpenDoors.Count > 0)
            {
                if (listOfPopulatedRooms.Count >= roomsToFill)
                {
                    break;
                }

                int randomRoom = random.Next(listOfAvaibleRoomsToOpenDoors.Count);
                Position randomPos = listOfAvaibleRoomsToOpenDoors[randomRoom];
                List<Directions.Direction> dir = GetRoomDirectionsAvailableToGo(rooms, randomPos, false);
                if (dir.Count > 0)
                {
                    Directions.Direction randomDir = dir[random.Next(dir.Count)];
                    OpenDoor(randomPos.X, randomPos.Y, randomDir);

                    Position position = null;

                    switch (randomDir)
                    {
                        case Directions.Direction.North:
                            position = new Position(randomPos.X, randomPos.Y--);
                            break;
                        case Directions.Direction.South:
                            position = new Position(randomPos.X, randomPos.Y++);
                            break;
                        case Directions.Direction.East:
                            position = new Position(randomPos.X++, randomPos.Y);
                            break;
                        case Directions.Direction.West:
                            position = new Position(randomPos.X--, randomPos.Y);
                            break;
                    }
                    listOfAvaibleRoomsToOpenDoors.Add(position);
                    listOfPopulatedRooms.Add(position);
                }
                else
                {
                    listOfAvaibleRoomsToOpenDoors.RemoveAt(randomRoom);
                    doneRooms++;
                }

            }
        }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            internal Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return string.Format("{0},{1}", X, Y);
            }
        }
    }
}
