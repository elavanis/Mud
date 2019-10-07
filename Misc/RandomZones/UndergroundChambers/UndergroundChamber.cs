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
                    foreach (string room in item.Rooms)
                    {
                        if (chamber.Rooms.Contains(room))
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
