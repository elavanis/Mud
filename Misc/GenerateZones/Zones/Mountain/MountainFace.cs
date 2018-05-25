using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;

namespace GenerateZones.Zones.Mountain
{
    public class MountainFace : IZoneCode
    {
        IZone zone;
        private int zoneId = 19;
        private int roomId = 1;
        private int itemId = 1;
        private int npcId = 1;

        public IZone Generate()
        {
            zone = new Zone();
            zone.Id = zoneId;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(MountainFace);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (Room)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            return zone;
        }

        private IRoom GenerateRoom1()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A rock face looks craggy and has plenty of hand holds.";
            room.LongDescription = "You stand at the base of the mountain side.  The ground is rocky here with little vegetation growing.";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The path up appears to be clear and able to be climbed.";
            room.LongDescription = "You stand at the base of the mountain side.  A few small clumps of grass grow in cracks in the rocky soil.";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "Several hand holds have been marked on the rock face showing a way up the mountain side.";
            room.LongDescription = "You stand at the base of the mountain side.  A small pool of water sits in a small divot in an otherwise smooth rock ground.";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face seems to smooth out the higher up you go.";
            room.LongDescription = "You stand at the base of the mountain side.  The ground is made of soil and has several sets of goat prints here.";

            return room;
        }


        private IRoom GenerateRoom5()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A few feet above you the rock face move back into the mountain making it hard to see a path up.";
            room.LongDescription = "You stand at the base of the mountain side.  There is a long path leading up the mountain to the east and a short solid rock wall going straight up.";

            return room;
        }

        private IRoom RockFace()
        {
            IRoom room = OutSide();
            room.MovementCost = 100;
            room.ShortDescription = "Mountain Face";
            return room;
        }

        private IRoom OutSide()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            return room;
        }
    }
}
