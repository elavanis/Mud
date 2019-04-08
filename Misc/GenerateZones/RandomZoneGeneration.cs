using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Zone.Interface;
using Objects.Global;
using System.Reflection;
using Objects.Room.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Zone;
using Objects.Room;

namespace GenerateZones
{
    public class RandomZoneGeneration
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int ZoneId { get; set; }
        public Random Rand { get; set; }
        public int PercentFlavorText { get; set; } = 5;

        public List<RoomDescription> RoomDescriptions = new List<RoomDescription>();
        public List<FlavorOption> RoomFlavorText = new List<FlavorOption>();
        public RoomDescription RoadDescription;

        private List<int> _exits = new List<int>();

        public RandomZoneGeneration(int width, int height, int zoneId, int seed = 1)
        {
            Width = width;
            Height = height;
            ZoneId = zoneId;
            Rand = new Random(zoneId + seed);

            //we want to add the rooms 1st thing so they do not move if we make changes to the rest of the zone
            _exits.Add(Rand.Next(Width)); //north
            _exits.Add(Rand.Next(Height)); //east
            _exits.Add(Rand.Next(Width)); //south
            _exits.Add(Rand.Next(Height)); //west
        }

        public IZone Generate()
        {
            PropertyInfo info = GlobalReference.GlobalValues.Random.GetType().GetProperty("_random", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(GlobalReference.GlobalValues.Random, Rand);

            IZone zone = new Zone();
            zone.Id = ZoneId;
            int roomNumber = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    roomNumber = AddRoom(zone, roomNumber);

                    if (j != 0)
                    {
                        ConnectRoomLeft(zone, roomNumber, roomNumber - 1);
                    }

                    if (i != 0)
                    {
                        ConnectRoomUp(zone, roomNumber, roomNumber - Height);
                    }
                }
            }

            return zone;
        }

        private void ConnectRoomUp(IZone zone, int roomNumber, int otherRoomNumber)
        {
            ZoneHelper.ConnectRoom(zone.Rooms[roomNumber], Direction.North, zone.Rooms[otherRoomNumber]);
        }

        private void ConnectRoomLeft(IZone zone, int roomNumber, int otherRoomNumber)
        {
            ZoneHelper.ConnectRoom(zone.Rooms[roomNumber], Direction.West, zone.Rooms[otherRoomNumber]);
        }

        private int AddRoom(IZone zone, int roomNumber)
        {
            IRoom room = new Room();
            roomNumber++;

            room.Zone = ZoneId;
            room.Id = roomNumber;

            int key = GlobalReference.GlobalValues.Random.Next(RoomDescriptions.Count);
            room.LookDescription = RoomDescriptions[key].LookDescription;
            room.ExamineDescription = RoomDescriptions[key].ExamineDescription;
            room.ShortDescription = RoomDescriptions[key].ShortDescription;

            if (RoomFlavorText.Count > 0)
            {
                List<int> selectedRoomFlavorText = new List<int>();
                while (CheckIfAddRoomFlavor(PercentFlavorText))
                {
                    int selectedFlavorText = Rand.Next(RoomFlavorText.Count);
                    if (selectedRoomFlavorText.Contains(selectedFlavorText))
                    {
                        break;
                    }
                    else
                    {
                        selectedRoomFlavorText.Add(selectedFlavorText);
                        room.LookDescription += "  " + RoomFlavorText[selectedFlavorText].ToString();
                    }
                }
            }

            zone.Rooms.Add(roomNumber, room);
            return roomNumber;
        }

        private bool CheckIfAddRoomFlavor(int percentFlavorText)
        {
            return GlobalReference.GlobalValues.Random.PercentDiceRoll(percentFlavorText);
        }

        public class RoomDescription
        {
            private Dictionary<string, string> replacedValues = new Dictionary<string, string>();

            private string _lookDescription;
            public string LookDescription
            {
                get
                {
                    return ProcessFlavorText(_lookDescription);
                }
                set
                {
                    _lookDescription = value;
                }
            }

            private string ProcessFlavorText(string description)
            {
                string workingDescription = description;
                foreach (FlavorOption option in FlavorOption)
                {
                    foreach (string key in option.FlavorValues.Keys)
                    {
                        List<string> values = option.FlavorValues[key];
                        string replacementValue = values[GlobalReference.GlobalValues.Random.Next(values.Count)];
                        workingDescription = workingDescription.Replace(key, replacementValue);
                    }
                }

                return workingDescription;
            }

            private string _examineDescription;
            public string ExamineDescription
            {
                get
                {
                    return ProcessFlavorText(_examineDescription);
                }
                set
                {
                    _examineDescription = value;
                }
            }

            private string _shortDescription;
            public string ShortDescription
            {
                get
                {
                    return ProcessFlavorText(_shortDescription);
                }
                set
                {
                    _shortDescription = value;
                }
            }

            public List<FlavorOption> FlavorOption { get; set; } = new List<RandomZoneGeneration.FlavorOption>();
        }

        public class FlavorOption
        {
            public string FlavorText { get; set; }
            public Dictionary<string, List<string>> FlavorValues { get; set; } = new Dictionary<string, List<string>>();

            public override string ToString()
            {
                string updatedFlavorText = FlavorText;

                foreach (string key in FlavorValues.Keys)
                {
                    List<string> flavorOptions = FlavorValues[key];
                    string replacementValue = flavorOptions[GlobalReference.GlobalValues.Random.Next(flavorOptions.Count)];
                    updatedFlavorText = updatedFlavorText.Replace(key, replacementValue);
                }

                return updatedFlavorText;
            }
        }


        public IZone AddRoad(IZone zone, ZoneConnection northZoneId, ZoneConnection eastZoneId, ZoneConnection southZoneId, ZoneConnection westZoneId)
        {
            RoomPos northPos = null;
            RoomPos eastPos = null;
            RoomPos southPos = null;
            RoomPos westPos = null;
            bool northContinue = false;
            bool eastContinue = false;
            bool southContinue = false;
            bool westContinue = false;

            List<RoomPos> northRoad = new List<RoomPos>();
            List<RoomPos> eastRoad = new List<RoomPos>();
            List<RoomPos> southRoad = new List<RoomPos>();
            List<RoomPos> westRoad = new List<RoomPos>();

            List<List<RoomPos>> allRoads = new List<List<RoomPos>>() { northRoad, eastRoad, southRoad, westRoad };

            #region Setup
            if (northZoneId != null)
            {
                northPos = new RoomPos(_exits[0], 0);
                northContinue = true;
                northRoad.Add(northPos);
                IRoom room = GetRoom(northPos, zone);
                ZoneHelper.ConnectZone(room, Direction.North, northZoneId.ZoneId, northZoneId.RoomId);
            }

            if (eastZoneId != null)
            {
                eastPos = new RoomPos(Width - 1, _exits[1]);
                eastContinue = true;
                eastRoad.Add(eastPos);
                IRoom room = GetRoom(eastPos, zone);
                ZoneHelper.ConnectZone(room, Direction.East, eastZoneId.ZoneId, eastZoneId.RoomId);
            }

            if (southZoneId != null)
            {
                southPos = new RoomPos(_exits[2], Height - 1);
                southContinue = true;
                southRoad.Add(southPos);
                IRoom room = GetRoom(southPos, zone);
                ZoneHelper.ConnectZone(room, Direction.South, southZoneId.ZoneId, southZoneId.RoomId);
            }

            if (westZoneId != null)
            {
                westPos = new RoomPos(0, _exits[3]);
                westContinue = true;
                westRoad.Add(westPos);
                IRoom room = GetRoom(westPos, zone);
                ZoneHelper.ConnectZone(room, Direction.West, westZoneId.ZoneId, westZoneId.RoomId);
            }
            #endregion Setup

            while (MultipleRoadsContinue(northContinue, eastContinue, southContinue, westContinue))
            {
                if (northContinue)
                {
                    bool matchPos = SearchForRoadMatch(northPos, northRoad, allRoads);

                    if (matchPos)
                    {
                        northContinue = false;
                    }
                    else
                    {
                        RoomPos newPos = new RoomPos(northPos.X, northPos.Y - 1);
                        northRoad.Add(newPos);
                        northPos = newPos;
                    }
                }

                if (eastContinue)
                {
                    bool matchPos = SearchForRoadMatch(eastPos, eastRoad, allRoads);

                    if (matchPos)
                    {
                        eastContinue = false;
                    }
                    else
                    {
                        RoomPos newPos = new RoomPos(eastPos.X - 1, eastPos.Y);
                        eastRoad.Add(newPos);
                        eastPos = newPos;
                    }
                }

                if (southContinue)
                {
                    bool matchPos = SearchForRoadMatch(southPos, southRoad, allRoads);

                    if (matchPos)
                    {
                        southContinue = false;
                    }
                    else
                    {
                        RoomPos newPos = new RoomPos(southPos.X, southPos.Y - 1);
                        southRoad.Add(newPos);
                        southPos = newPos;
                    }
                }

                if (westContinue)
                {
                    bool matchPos = SearchForRoadMatch(westPos, westRoad, allRoads);

                    if (matchPos)
                    {
                        westContinue = false;
                    }
                    else
                    {
                        RoomPos newPos = new RoomPos(westPos.X + 1, westPos.Y);
                        westRoad.Add(newPos);
                        westPos = newPos;
                    }
                }
            }

            foreach (List<RoomPos> road in allRoads)
            {
                foreach (RoomPos roomPos in road)
                {
                    IRoom room = GetRoom(roomPos, zone);
                    room.LookDescription = RoadDescription.LookDescription;
                    room.ShortDescription = RoadDescription.ShortDescription;
                    room.ExamineDescription = RoadDescription.ExamineDescription;
                }
            }

            return zone;
        }

        private static bool SearchForRoadMatch(RoomPos roomPos, List<RoomPos> listRoad, List<List<RoomPos>> allRoads)
        {
            bool matchPos = false;
            foreach (List<RoomPos> road in allRoads)
            {
                if (road == listRoad)
                {
                    continue;
                }
                else
                {
                    RoomPos foundPos = road.FirstOrDefault(i => i.X == roomPos.X || i.Y == roomPos.Y);
                    if (foundPos != null)
                    {
                        ConnectRoom(roomPos, foundPos, listRoad, road);
                        return true;
                    }
                }
            }

            return matchPos;
        }

        private static void ConnectRoom(RoomPos roomPos, RoomPos foundPos, List<RoomPos> currentRoad, List<RoomPos> foundRoad)
        {
            if (roomPos.X == foundPos.X)
            {
                int direction = foundPos.Y - roomPos.Y;
                direction = direction / Math.Abs(direction);

                while (true)
                {
                    RoomPos newRoomPos = new RoomPos(roomPos.X, roomPos.Y + direction);
                    {
                        currentRoad.Add(newRoomPos);
                        roomPos = newRoomPos;
                        if (foundRoad.FirstOrDefault(i => i.X == roomPos.X && i.Y == roomPos.Y) != null)
                        {
                            break;
                        }
                    }
                }

            }
            else
            {
                int direction = foundPos.X - roomPos.X;
                direction = direction / Math.Abs(direction);

                while (true)
                {
                    RoomPos newRoomPos = new RoomPos(roomPos.X + direction, roomPos.Y);
                    {
                        currentRoad.Add(newRoomPos);
                        roomPos = newRoomPos;
                        if (foundRoad.FirstOrDefault(i => i.X == roomPos.X && i.Y == roomPos.Y) != null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool MultipleRoadsContinue(bool northContinue, bool eastContinue, bool southContinue, bool westContinue)
        {
            List<bool> all = new List<bool>();
            all.Add(northContinue);
            all.Add(eastContinue);
            all.Add(southContinue);
            all.Add(westContinue);

            return all.Where(c => c).Count() > 1;
        }

        private void SetRoadDescription(IRoom room)
        {
            room.ExamineDescription = RoadDescription.ExamineDescription;
            room.LookDescription = RoadDescription.LookDescription;
            room.ShortDescription = RoadDescription.ShortDescription;
        }

        private IRoom GetRoom(RoomPos roomPosition, IZone zone)
        {
            int roomId = roomPosition.Y * Width + roomPosition.X + 1;

            return zone.Rooms[roomId];
        }
    }
    internal class RoomPos
    {
        internal int X { get; set; }
        internal int Y { get; set; }

        internal RoomPos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class ZoneConnection
    {
        public int ZoneId { get; set; }
        public int RoomId { get; set; }
    }
}
