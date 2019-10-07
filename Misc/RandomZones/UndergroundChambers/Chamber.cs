using System;
using System.Collections.Generic;
using System.Text;

namespace UndergroundChambers
{
    public class Chamber
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public bool Valid { get; set; } = true;

        public Chamber(int x, int y, int maxX, int maxY, int sizeX = 3, int sizeY = 3)
        {
            X = x;
            Y = y;
            SizeX = sizeX;
            SizeY = sizeY;

            Rooms = new HashSet<string>();

            int xStart = x - (sizeX - 1) / 2;
            int yStart = y - (sizeY - 1) / 2;

            for (int xPos = xStart; xPos < xStart + sizeX; xPos++)
            {
                for (int yPos = yStart; yPos < yStart + yStart; yPos++)
                {
                    Rooms.Add($"{xPos},{yPos}");
                    if (xPos < 0 || xPos > maxX
                        || yPos < 0 || yPos > maxY)
                    {
                        Valid = false;
                        break;
                    }
                }
            }
        }

        public HashSet<string> Rooms { get; set; }
    }
}
