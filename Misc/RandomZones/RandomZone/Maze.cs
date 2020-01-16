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
    }
}
