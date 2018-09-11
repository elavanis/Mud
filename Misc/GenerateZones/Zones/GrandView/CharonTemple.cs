using System;
using System.Collections.Generic;
using System.Text;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Room.Room;

namespace GenerateZones.Zones.GrandView
{
    public class CharonTemple : BaseZone, IZoneCode
    {
        public CharonTemple() : base(20)
        {
        }

        public IZone Generate()
        {
            throw new NotImplementedException();
        }

        private IRoom GenerateRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(RoomAttribute.Indoor);
            return room;
        }

        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "Faint humming sounds can be heard from below.";
            room.LookDescription = "Stairs descend downward into the darkness of the of the temple.";
            room.ShortDescription = "Charon Temple";
            room.Attributes.Add(RoomAttribute.NoLight);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            room.LookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            room.ShortDescription = "Charon Temple";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            room.LookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            room.ShortDescription = "Charon Temple";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            room.LookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            room.ShortDescription = "Charon Temple";

            return room;
        }
    }
}
