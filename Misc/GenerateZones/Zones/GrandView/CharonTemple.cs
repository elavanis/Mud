﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Item.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Mob.NonPlayerCharacter;
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
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(CharonTemple);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }

        #region Rooms
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

        private IRoom GenerateRoom5()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "A small shrine has been setup next to the dock.";
            room.LookDescription = "There is dock extending twenty or thirty feet into a river that extends into the darkness.";
            room.ShortDescription = "Charon Temple";

            room.AddItemToRoom(Shrine());

            return room;
        }

        private IItem Shrine()
        {
            IItem shrine = CreateItem<IItem>();
            shrine.Zone = Zone.Id;
            shrine.ExamineDescription = "The shrine is full of small coins carefully placed there.";
            shrine.LookDescription = "The shrine is make of old wooden boat planks.";
            shrine.ShortDescription = "The shrine has a small lantern lit in the center of the wooden shrine.";
            shrine.SentenceDescription = "shrine";
            shrine.KeyWords.Add("shrine");

            return shrine;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "In the center of the circle is a statue of Charon guiding his boat down the river with his mighty oar.";
            room.LookDescription = "A small circle has been drawn in chalk on the cave floor.";
            room.ShortDescription = "Charon Temple";

            for (int i = 0; i < 8; i++)
            {
                room.AddMobileObjectToRoom(Priest());
            }
            return room;
        }

        private INonPlayerCharacter Priest()
        {
            INonPlayerCharacter nonPlayerCharacter = CreateNonplayerCharacter(MobType.Humanoid);
            nonPlayerCharacter.Personalities.Add(new Guardian());
            nonPlayerCharacter.Level = 20;
            nonPlayerCharacter.ExamineDescription = "Clothed in a gray tunic the priest stands in meditation while emitting a humming sound.";
            nonPlayerCharacter.LookDescription = "The priest mostly ignores you as all are welcome to the temple of Charon.";
            nonPlayerCharacter.ShortDescription = "A mediative priest stands in his place the circle.";
            nonPlayerCharacter.SentenceDescription = "priest";
            nonPlayerCharacter.KeyWords.Add("Priest");

            return nonPlayerCharacter;
        }

        #endregion Rooms

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.Up, 21, 13);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.Down, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.West, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.West, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.West, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.North, Zone.Rooms[6]);
        }
    }
}
