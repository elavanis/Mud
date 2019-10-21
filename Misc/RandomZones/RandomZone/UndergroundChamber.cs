using RandomZone.Internal;
using RandomZone.UndergroundChamberObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomZone
{
    public class UndergroundChamber : Internal.RandomZone
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
            else if (lChambers.Count > 1)
            {
                chamber = lChambers[random.Next(lChambers.Count)];
            }
            else
            {
                //there is only 1 chamber, return it so we can finish
                return lChambers[0];
            }

            if (chamber == conectingChamber)
            {
                return GetRandomChamber(unconnectedChambers, lChambers, conectingChamber, false);
            }

            return chamber;
        }

        private void ConnectRooms()
        {
            for (int x = 0; x < rooms.GetLength(0); x++)
            {
                for (int y = 0; y < rooms.GetLength(1); y++)
                {
                    if (rooms[x, y] != null)
                    {
                        if (x + 1 < rooms.GetLength(0)) //don't go out of bounds
                        {
                            if (rooms[x + 1, y] != null)
                            {
                                rooms[x, y].Open(Directions.Direction.East);
                                rooms[x + 1, y].Open(Directions.Direction.West);
                            }
                        }

                        if (y + 1 < rooms.GetLength(1)) //don't go out of bounds
                        {
                            if (rooms[x, y + 1] != null)
                            {
                                rooms[x, y].Open(Directions.Direction.South);
                                rooms[x, y + 1].Open(Directions.Direction.North);
                            }
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
                //int chamberWidth = random.Next(1) * 2 + 3; //3, 5, 7
                //int chamberHeight = random.Next(1) * 2 + 3; //3, 5, 7
                int chamberWidth = 3;
                int chamberHeight = 3;

                Chamber chamber = new Chamber(random.Next(x), random.Next(y), x, y, chamberWidth, chamberHeight);

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
