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


            ConnectRooms();
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
