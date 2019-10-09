using SharedRandomZones;
using System;
using System.Collections.Generic;

namespace UndergroundChambers
{
    public class UndergroundChamber : RandomZone
    {
        public void Generate(int x, int y, int chambers, int randomSeed = -1)
        {
            Initilize(x, y, randomSeed);

            int chamberFailedInitlize = 0;
            List<Chamber> lChambers = new List<Chamber>();

            BuildChambers(x, y, chambers, chamberFailedInitlize, lChambers);

            BuildRooms(lChambers);

            ConnectChambers(lChambers);

            ConnectRooms();
        }

        private void ConnectChambers(List<Chamber> lChambers)
        {
            List<Chamber> unconnectedChambers = new List<Chamber>(lChambers);

            while (unconnectedChambers.Count > 0)
            {
                Chamber start = GetRandomChamber(unconnectedChambers, lChambers);
                Chamber end = GetRandomChamber(unconnectedChambers, lChambers);

                int diffX = end.X - start.X;
                int diffY = end.Y - start.Y;

                bool northSouthFirst = random.Next(2) == 1;
                if (northSouthFirst)
                {
                    GoNorthSouth(start.Y, end.Y, start.X);
                    GoEastWest(start.X, end.X, end.Y);
                }
                else
                {
                    GoEastWest(start.X, end.X, start.Y);
                    GoNorthSouth(start.Y, end.Y, end.X);
                }
            }
        }

        private void GoNorthSouth(int startY, int endY, int xPos)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (rooms[xPos, y] == null)
                {
                    rooms[xPos, y] = new Room();
                }
            }
        }

        private void GoEastWest(int startX, int endX, int yPos)
        {
            for (int x = startX; x <= endX; x++)
            {
                if (rooms[x, yPos] == null)
                {
                    rooms[x, yPos] = new Room();
                }
            }
        }

        private Chamber GetRandomChamber(List<Chamber> unconnectedChambers, List<Chamber> lChambers)
        {
            Chamber chamber;
            if (unconnectedChambers.Count > 0)
            {
                chamber = unconnectedChambers[random.Next(unconnectedChambers.Count)];
                unconnectedChambers.Remove(chamber);
            }
            else
            {
                chamber = lChambers[random.Next(lChambers.Count)];
            }

            return chamber;
        }

        private void ConnectRooms()
        {
            for (int x = 0; x < rooms.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < rooms.GetLength(1) - 1; y++)
                {
                    if (rooms[x, y] != null)
                    {
                        if (rooms[x + 1, y] != null)
                        {
                            rooms[x, y].Open(Directions.Direction.East);
                            rooms[x + 1, y].Open(Directions.Direction.West);
                        }

                        if (rooms[x, y + 1] != null)
                        {
                            rooms[x, y].Open(Directions.Direction.South);
                            rooms[x, y + 1].Open(Directions.Direction.North);
                        }
                    }
                }
            }
        }

        private void BuildRooms(List<Chamber> lChambers)
        {
            foreach (Chamber chamber in lChambers)
            {
                foreach (string item in chamber.Rooms)
                {
                    string[] str = item.Split(',');
                    int xPos = int.Parse(str[0]);
                    int yPos = int.Parse(str[1]);

                    rooms[xPos, yPos] = new Room();
                }
            }
        }

        private void BuildChambers(int x, int y, int chambers, int chamberFailedInitlize, List<Chamber> lChambers)
        {
            while (lChambers.Count < chambers && chamberFailedInitlize < 100)
            {
                Chamber chamber = new Chamber(random.Next(x), random.Next(y), x, y);

                bool valid = MatchRooms(lChambers, chamber);

                if (valid)
                {
                    chamberFailedInitlize = 0;
                    lChambers.Add(chamber);
                }
                else
                {
                    chamberFailedInitlize++;
                }
            }
        }

        private static bool MatchRooms(List<Chamber> lChambers, Chamber chamber)
        {
            if (chamber.Valid)
            {
                foreach (Chamber item in lChambers)
                {
                    foreach (string buffer in item.Buffer)
                    {
                        if (chamber.Rooms.Contains(buffer))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
