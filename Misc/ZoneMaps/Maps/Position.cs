using System;
using System.Collections.Generic;
using System.Text;

namespace Maps
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Position(int x, int y, int z = 1)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
    }
}
