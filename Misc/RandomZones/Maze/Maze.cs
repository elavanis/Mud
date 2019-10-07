using MiscShared;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;
using SharedRandomZones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maze
{
    public class Maze : RandomZone
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
