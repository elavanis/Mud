﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Item.Items;
using Objects.Item.Items.EnchantersTower;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Mob.NonPlayerCharacter;

namespace GenerateZones.Zones.Mountain
{
    public class EnchantersTower : BaseZone, IZoneCode
    {
        public EnchantersTower() : base(23)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = -1; //don't let this zone reset
            Zone.Name = nameof(EnchantersTower);

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

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton());
            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton());
            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton());
            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton());
            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = Stairs();

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = Stairs();

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = Stairs();

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = TowerRoom();

            room.ShortDescription = "Top Floor";
            room.ExamineDescription = "A large collection of mirrors and lenses seem to be setup to collect energy into a single point in the center of the room.";
            room.LookDescription = "Everything in the room seems to be focused on the center pedestal.";

            Container pedestal = CreateItem<Container>();
            pedestal.ExamineDescription = "The pedestal has a socket that looks to be designed to hold a focusing item.";
            pedestal.LookDescription = "The pedestal is made of a creamy white stone.";
            pedestal.ShortDescription = "A white stone pedestal stands in the center of the room with everything else focused on it.";
            pedestal.SentenceDescription = "pedestal";
            pedestal.KeyWords.Add("pedestal");
            pedestal.KeyWords.Add("stone");
            pedestal.Attributes.Add(ItemAttribute.NoGet);

            room.AddItemToRoom(pedestal);
            room.AddMobileObjectToRoom(ButlerAutomaton());

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = TowerRoom();
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ShortDescription = "Top Floor";
            room.ExamineDescription = "The room has a small window over looking the plateau to the east.";
            room.LookDescription = "A pair of five inch holes exist in the ceiling and the floor.";

            room.AddMobileObjectToRoom(ButlerAutomaton());

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = Stairs();

            room.AddMobileObjectToRoom(ButlerAutomaton());

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = TowerRoom();

            room.ShortDescription = "Enchanting Room";
            room.ExamineDescription = "The table glows faintly with the residual energy of the thousands of enchantments performed on it.";
            room.LookDescription = "A small table sits in the center of the room with light filtering down on it form a five inch hole in the ceiling.";

            room.AddItemToRoom(Enchantery());
            room.AddMobileObjectToRoom(ButlerAutomaton());

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = TowerRoom();

            room.ShortDescription = "Holding Cells";
            room.LookDescription = "There are several cages along the walls used for holding prisoners.";
            room.ExamineDescription = "Some of the cages show signs of distress where prisoners have tried to escape.  Whether they were successful or not is anyones guess.";

            room.AddMobileObjectToRoom(ButlerAutomaton());

            return room;
        }
        #endregion Rooms

        #region Npc
        private INonPlayerCharacter DusterAutomaton()
        {
            INonPlayerCharacter npc = GenerateAutomaton();
            npc.ShortDescription = "A duster automaton.";
            npc.LookDescription = "The automaton dust both high and low leaving no dust safe.";
            npc.ExamineDescription = "The automaton wanders aimlessly around dusting things with a feather duster.";
            npc.SentenceDescription = "dusting automaton";

            return npc;
        }

        private INonPlayerCharacter GenerateAutomaton()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 20);
            npc.KeyWords.Add("automaton");
            npc.KeyWords.Add("duster");
            npc.Personalities.Add(new Wanderer());
            return npc;
        }

        private INonPlayerCharacter ButlerAutomaton()
        {
            INonPlayerCharacter npc = GenerateAutomaton();
            npc.ShortDescription = "A butler automaton.";
            npc.LookDescription = "The automaton walks around tiding up the place and waits for instructions form its master.";
            npc.ExamineDescription = "The automaton is always alert for the slightest sign that its master needs it to perform a duty.";
            npc.SentenceDescription = "butler automaton";

            return npc;
        }
        #endregion Npc

        private IEnchantery Enchantery()
        {
            IEnchantery tempEnchantery = CreateItem<IEnchantery>();     //do this just to increment the numbers
            IEnchantery enchantery = new EnchantingTable();     //create the real object we need
            enchantery.Zone = tempEnchantery.Zone;
            enchantery.Id = tempEnchantery.Id;
            enchantery.Attributes.Add(ItemAttribute.NoGet);
            enchantery.KeyWords.Add("table");
            enchantery.SentenceDescription = "table";
            enchantery.ShortDescription = "The table glows with wisps of energy radiating upward.";
            enchantery.LookDescription = "The table glows faintly as wisps of energy radiate up into the air before dissipating.";
            enchantery.ExamineDescription = "The table once was a dark oak but with time and enchantments it has begun to glow a slight blue color casting a blue tint on everything in the room.";

            return enchantery;
        }

        private IRoom Stairs()
        {
            IRoom room = TowerRoom();

            room.ShortDescription = "Spiral Staircase";
            room.ExamineDescription = "The stairs are surprisingly made of wood instead of stone and creak slightly as you walk on them.";
            room.LookDescription = "The spiral stairs ascend up the tower as well as down into a basement area.";
            return room;
        }

        private IRoom GroundFloor()
        {
            IRoom room = TowerRoom();

            room.ShortDescription = "Ground Floor";
            room.ExamineDescription = "The stone hallway is lit with a pair of torches ever twelve feet.";
            room.LookDescription = "A stone hallway leading outside the tower and deeper inside toward a stairwell.";
            return room;
        }

        private IRoom TowerRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);
            return room;
        }

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.West, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.Up, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.Up, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.Down, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);
        }
    }
}