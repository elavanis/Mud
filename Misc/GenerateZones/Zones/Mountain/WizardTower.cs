using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.Mountain
{
    public class WizardTower : BaseZone, IZoneCode
    {
        public WizardTower() : base(23)
        {

        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(WizardTower);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }


        private IRoom GenerateRoom1()
        {
            IRoom room = GroundFloor();

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = GroundFloor();

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GroundFloor();

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GroundFloor();

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = GroundFloor();

            return room;
        }

        private IRoom GroundFloor()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ShortDescription = "Ground Floor";
            room.ExamineDescription = "The hallway is lit with a pair of torches ever 12 feet.";
            room.LookDescription = "A stone hallway leading outside and toward a stairwell.";
            return room;
        }


        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.West, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);

        }

    }
}
