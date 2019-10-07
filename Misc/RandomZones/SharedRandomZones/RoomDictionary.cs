using System;
using System.Collections.Generic;
using System.Text;

namespace SharedRandomZones
{
    public abstract class RoomDictionary
    {
        private static Dictionary<string, string> sortedDictionary = new Dictionary<string, string>();
        private static Dictionary<string, Room> roomDict = new Dictionary<string, Room>();

        public static Room GetRoom(string roomKey)
        {
            Room room = null;
            roomKey = SortRoomKey(roomKey);
            roomDict.TryGetValue(roomKey, out room);
            if (room == null)
            {
                room = BuildRoom(roomKey);
            }

            return room;
        }

        private static string SortRoomKey(string roomKey)
        {
            string key = null;
            sortedDictionary.TryGetValue(roomKey, out key);
            if (key != null)
            {
                return key;
            }

            //string newKey = "";
            StringBuilder newKey = new StringBuilder();
            if (roomKey.Contains("N"))
            {
                //newKey += "N";
                newKey.Append("N");
            }
            if (roomKey.Contains("S"))
            {
                //newKey += "S";
                newKey.Append("S");
            }
            if (roomKey.Contains("E"))
            {
                //newKey += "E";
                newKey.Append("E");
            }
            if (roomKey.Contains("W"))
            {
                //newKey += "W";
                newKey.Append("W");
            }

            sortedDictionary.Add(roomKey, newKey.ToString());
            return newKey.ToString();
        }

        private static Room BuildRoom(string roomKey)
        {
            Room room = new Room();
            if (roomKey.Contains("N"))
            {
                room.Open(Directions.Direction.North);
            }
            if (roomKey.Contains("S"))
            {
                room.Open(Directions.Direction.South);
            }
            if (roomKey.Contains("E"))
            {
                room.Open(Directions.Direction.East);
            }
            if (roomKey.Contains("W"))
            {
                room.Open(Directions.Direction.West);
            }
            roomDict.Add(room.ToString(), room);
            return room;
        }

    }
}