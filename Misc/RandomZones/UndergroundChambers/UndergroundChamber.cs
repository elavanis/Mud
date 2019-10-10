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

            Chamber end = GetRandomChamber(unconnectedChambers, lChambers, null, false);

            while (unconnectedChambers.Count > 0)
            {
                Chamber start = GetRandomChamber(unconnectedChambers, lChambers, end, true);

                int diffX = end.X - start.X;
                int diffY = end.Y - start.Y;

                bool northSouthFirst = random.Next(2) == 1;
                if (northSouthFirst)
                {
                    bool connected = GoNorthSouth(start, end, start.X);

                    if (!connected)
                    {
                        GoEastWest(start, end, end.Y);
                    }
                }
                else
                {
                    bool connected = GoEastWest(start, end, start.Y);

                    if (!connected)
                    {
                        GoNorthSouth(start, end, end.X);
                    }
                }
            }
        }

        private bool GoNorthSouth(Chamber start, Chamber end, int xPos)
        {
            int changeValue = start.Y - end.Y > 0 ? -1 : 1;
            int yPos = start.Y;

            while (yPos != end.Y)
            {
                if (rooms[xPos, yPos] == null)
                {
                    rooms[xPos, yPos] = new Room();
                }
                else
                {
                    if (!start.Rooms.Contains($"{xPos},{yPos}"))
                    {
                        return true;
                    }
                }

                yPos += changeValue;
            }

            return false;
        }

        private bool GoEastWest(Chamber start, Chamber end, int yPos)
        {
            int changeValue = start.X - end.X > 0 ? -1 : 1;
            int xPos = start.X;

            while (xPos != end.X)
            {
                if (rooms[xPos, yPos] == null)
                {
                    rooms[xPos, yPos] = new Room();
                }
                else
                {
                    if (!start.Rooms.Contains($"{xPos},{yPos}"))
                    {
                        return true;
                    }
                }

                xPos += changeValue;
            }

            return false;
        }

        private Chamber GetRandomChamber(List<Chamber> unconnectedChambers, List<Chamber> lChambers, Chamber conectingChamber, bool removeFromListOfChambers)
        {
            Chamber chamber;
            if (unconnectedChambers.Count > 0)
            {
                chamber = unconnectedChambers[random.Next(unconnectedChambers.Count)];

                if (removeFromListOfChambers)
                {
                    unconnectedChambers.Remove(chamber);
                }
            }
            else
            {
                chamber = lChambers[random.Next(lChambers.Count)];
            }

            if (chamber == conectingChamber)
            {
                return GetRandomChamber(unconnectedChambers, lChambers, conectingChamber, false);
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
