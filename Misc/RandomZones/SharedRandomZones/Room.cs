using System;
using System.Collections.Generic;
using System.Text;

namespace SharedRandomZones
{
    public class Room
    {
        private bool North = false;
        private bool South = false;
        private bool East = false;
        private bool West = false;

        public void Open(Directions.Direction direction)
        {
            bool openClose = true;
            OpenClose(direction, openClose);
        }

        internal void Close(Directions.Direction direction)
        {
            bool openClose = false;
            OpenClose(direction, openClose);
        }

        public override string ToString()
        {
            string room = "";
            if (North)
            {
                room += "N";
            }
            if (South)
            {
                room += "S";
            }
            if (East)
            {
                room += "E";
            }
            if (West)
            {
                room += "W";
            }
            return room;
        }

        private void OpenClose(Directions.Direction direction, bool openClose)
        {
            switch (direction)
            {
                case Directions.Direction.North:
                    North = openClose;
                    break;
                case Directions.Direction.South:
                    South = openClose;
                    break;
                case Directions.Direction.East:
                    East = openClose;
                    break;
                case Directions.Direction.West:
                    West = openClose;
                    break;
            }
        }
    }
}
