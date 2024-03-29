﻿using System;
using Objects.Zone.Interface;
using Objects.Room.Interface;
using Objects.Mob.Interface;
using Objects.Item.Items.Interface;
using Objects.Personality;
using Objects.Damage.Interface;
using Objects.Global;
using static Objects.Damage.Damage;
using Objects.Room;
using System.Reflection;
using static Objects.Global.Direction.Directions;
using Objects.Magic.Interface;
using Objects.Magic.Enchantment;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Language;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using MiscShared;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class AbandonedDwarvenMine : BaseZone, IZoneCode
    {
        public AbandonedDwarvenMine() : base(13)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 5;
            Zone.Name = nameof(AbandonedDwarvenMine);

            //int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= 113; i++)
            {
                IRoom? room = null;
                if (i >= 1 && i <= 6)
                {
                    room = OreCartStorage();
                }
                else if (i >= 12 && i <= 46)
                {
                    room = GoldMine();
                }
                else if (i >= 52 && i <= 57)
                {
                    room = GoldMineFloorRoom1();
                }
                else if (i >= 58 && i <= 63)
                {
                    room = GoldMineFloorRoom2();
                }
                else if (i >= 64 && i <= 72)
                {
                    room = GoldMineFloorRoom3();
                }
                else if (i >= 81 && i <= 86)
                {
                    room = GoldMineFloorRoom5();
                }
                else if (i >= 87 && i <= 88)
                {
                    room = GoldMineFloorRoom6();
                }
                else if (i >= 89 && i <= 92)
                {
                    room = GoldMineFloorRoom7();
                }
                else if (i >= 93 && i <= 105)
                {
                    room = GoldMineFloorConnectingTunnel();
                }
                else
                {
                    string methodName = "GenerateRoom" + i;
                    MethodInfo? method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (method != null)
                    {
                        room = (IRoom?)method.Invoke(this, null);
                    }
                }

                if (room != null)
                {
                    room.ZoneId = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            AddMobs();

            ConnectRooms();

            return Zone;
        }

        private void AddMobs()
        {
            for (int i = 0; i < 40; i++)
            {
                int roomId = GlobalReference.GlobalValues.Random.Next(Zone.Rooms.Count) + 1;
                IRoom room = Zone.Rooms[roomId];
                INonPlayerCharacter npc = Shadow(room);
                room.AddMobileObjectToRoom(npc);

                if (i % 2 == 0)
                {
                    npc.Personalities.Add(new Wanderer());
                }

            }
        }

        private void ConnectRooms()
        {
            #region Ore Cart Storage
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.South, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            #endregion Ore Cart Storage

            #region Path Ore Cart Storage -- Gold Mine
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.West, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.West, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.West, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.West, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.West, Zone.Rooms[11]);

            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[106]);
            ZoneHelper.ConnectRoom(Zone.Rooms[106], Direction.East, Zone.Rooms[107]);
            ZoneHelper.ConnectRoom(Zone.Rooms[107], Direction.East, Zone.Rooms[108]);
            ZoneHelper.ConnectRoom(Zone.Rooms[108], Direction.North, Zone.Rooms[109]);
            ZoneHelper.ConnectRoom(Zone.Rooms[109], Direction.North, Zone.Rooms[110]);
            ZoneHelper.ConnectRoom(Zone.Rooms[110], Direction.North, Zone.Rooms[111]);
            ZoneHelper.ConnectRoom(Zone.Rooms[111], Direction.North, Zone.Rooms[112]);
            ZoneHelper.ConnectRoom(Zone.Rooms[112], Direction.North, Zone.Rooms[8]);
            #endregion  Path Ore Cart Storage -- Gold Mine
            ZoneHelper.ConnectZone(Zone.Rooms[108], Direction.East, 15, 1);


            #region Gold Mine
            #region Gold Pit
            for (int i = 11; i < 16; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.West, Zone.Rooms[i + 1]);
            }

            for (int i = 16; i < 21; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.South, Zone.Rooms[i + 1]);
            }

            for (int i = 21; i < 26; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.East, Zone.Rooms[i + 1]);
            }

            for (int i = 26; i < 30; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.North, Zone.Rooms[i + 1]);
            }

            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.Down, Zone.Rooms[31]);

            for (int i = 31; i < 35; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.West, Zone.Rooms[i + 1]);
            }

            for (int i = 35; i < 38; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.South, Zone.Rooms[i + 1]);
            }

            for (int i = 38; i < 41; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.East, Zone.Rooms[i + 1]);
            }

            for (int i = 41; i < 43; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.North, Zone.Rooms[i + 1]);
            }

            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.Down, Zone.Rooms[44]);

            for (int i = 44; i < 45; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.West, Zone.Rooms[i + 1]);
            }

            for (int i = 45; i < 46; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.South, Zone.Rooms[i + 1]);
            }

            for (int i = 46; i < 47; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.East, Zone.Rooms[i + 1]);
            }
            #endregion Gold Pit

            #region Mine Shaft
            for (int i = 47; i <= 51; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.Down, Zone.Rooms[i + 1]);
            }

            #endregion Mine Shaft

            #region Gold Mine Floor
            //Room 1
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.East, Zone.Rooms[53]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.South, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.North, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.South, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.North, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[54], Direction.East, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.East, Zone.Rooms[57]);

            //Room 2
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.East, Zone.Rooms[59]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.East, Zone.Rooms[60]);
            ZoneHelper.ConnectRoom(Zone.Rooms[61], Direction.East, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.East, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.South, Zone.Rooms[61]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.South, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[60], Direction.South, Zone.Rooms[63]);

            //Room 3
            ZoneHelper.ConnectRoom(Zone.Rooms[64], Direction.East, Zone.Rooms[65]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.East, Zone.Rooms[66]);
            ZoneHelper.ConnectRoom(Zone.Rooms[67], Direction.East, Zone.Rooms[68]);
            ZoneHelper.ConnectRoom(Zone.Rooms[68], Direction.East, Zone.Rooms[69]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.East, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.East, Zone.Rooms[72]);

            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.North, Zone.Rooms[67]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.North, Zone.Rooms[68]);
            ZoneHelper.ConnectRoom(Zone.Rooms[72], Direction.North, Zone.Rooms[69]);
            ZoneHelper.ConnectRoom(Zone.Rooms[68], Direction.North, Zone.Rooms[65]);

            //Room4
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.East, Zone.Rooms[74]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.North, Zone.Rooms[75]);
            ZoneHelper.ConnectRoom(Zone.Rooms[75], Direction.North, Zone.Rooms[76]);
            ZoneHelper.ConnectRoom(Zone.Rooms[76], Direction.West, Zone.Rooms[77]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.West, Zone.Rooms[78]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.South, Zone.Rooms[79]);
            ZoneHelper.ConnectRoom(Zone.Rooms[79], Direction.South, Zone.Rooms[80]);
            ZoneHelper.ConnectRoom(Zone.Rooms[80], Direction.East, Zone.Rooms[73]);

            //Room5
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.West, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.South, Zone.Rooms[84]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.South, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.West, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.South, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[83], Direction.South, Zone.Rooms[81]);
            ZoneHelper.ConnectRoom(Zone.Rooms[82], Direction.West, Zone.Rooms[81]);

            //Room6
            ZoneHelper.ConnectRoom(Zone.Rooms[87], Direction.West, Zone.Rooms[88]);

            //Room7
            ZoneHelper.ConnectRoom(Zone.Rooms[89], Direction.East, Zone.Rooms[90]);
            ZoneHelper.ConnectRoom(Zone.Rooms[89], Direction.South, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.East, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[90], Direction.South, Zone.Rooms[92]);

            //Room 1 to Room 2
            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.South, Zone.Rooms[94]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.South, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.South, Zone.Rooms[58]);

            //Room 2 to Room 3
            ZoneHelper.ConnectRoom(Zone.Rooms[60], Direction.North, Zone.Rooms[96]);
            ZoneHelper.ConnectRoom(Zone.Rooms[96], Direction.East, Zone.Rooms[97]);
            ZoneHelper.ConnectRoom(Zone.Rooms[97], Direction.East, Zone.Rooms[70]);

            //Room 3 to Room 4
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.East, Zone.Rooms[98]);
            ZoneHelper.ConnectRoom(Zone.Rooms[98], Direction.North, Zone.Rooms[99]);
            ZoneHelper.ConnectRoom(Zone.Rooms[99], Direction.North, Zone.Rooms[74]);

            //Room 1 to Room 5
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.West, Zone.Rooms[100]);
            ZoneHelper.ConnectRoom(Zone.Rooms[100], Direction.West, Zone.Rooms[101]);
            ZoneHelper.ConnectRoom(Zone.Rooms[101], Direction.South, Zone.Rooms[86]);

            //Room 5 to Room 6
            ZoneHelper.ConnectRoom(Zone.Rooms[81], Direction.South, Zone.Rooms[102]);
            ZoneHelper.ConnectRoom(Zone.Rooms[102], Direction.West, Zone.Rooms[103]);
            ZoneHelper.ConnectRoom(Zone.Rooms[103], Direction.South, Zone.Rooms[87]);

            //Room 5 to Room 7
            ZoneHelper.ConnectRoom(Zone.Rooms[82], Direction.East, Zone.Rooms[104]);
            ZoneHelper.ConnectRoom(Zone.Rooms[104], Direction.South, Zone.Rooms[105]);
            ZoneHelper.ConnectRoom(Zone.Rooms[105], Direction.South, Zone.Rooms[89]);

            #endregion Gold Mine Floor
            #endregion Gold Mine
        }

        #region Rooms
        #region Ore Cart Storage
        private IRoom OreCartStorage()
        {

            string examineDescription = "Rows of mining cart tracks all converge and continue north and outside the room.";
            string lookDescription = "Rows and rows of tracks can be seen indicating this is some type of mining cart storage area.";
            string shortDescription = "Ore Cart Storage.";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Ore Cart Storage

        #region Path Ore Cart Storage -- Gold Mine
        private IRoom GenerateRoom7()
        {

            string examineDescription = "The ore track is beginning to rust from years of neglect.";
            string lookDescription = "An ore track runs to the west off into the darkness and to the south.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom8()
        {

            string examineDescription = "Some type of fire happened here but it happened so long ago it would be hard to tell what it was.";
            string lookDescription = "Some ashes lie on the mine floor here.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom9()
        {

            string examineDescription = "The metal track has warped and bent in the intense heat of a fire.";
            string lookDescription = "The walls of the mine have been covered in soot and the rail ties for the ore cars have been burned away.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom10()
        {

            string examineDescription = "The sound of the water echo off the cave walls and reverberates down the tunnel.  The pool of water over flows a little with each drop making the floor wet before flowing into a crack in the wall to the north.";
            string lookDescription = "A slow but steady drip falls into a shallow pool of water off to the side of the track.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom11()
        {

            string examineDescription = "The walls still bares the scares of the pick axes used to carve out this tunnel.";
            string lookDescription = "A small column of stone reaches the ceiling seeming to indicate the seem that the miners followed spit in two and then rejoined a dozen feet later.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom106()
        {

            string examineDescription = "The mine does not appear to have collapsed but looks like it could.";
            string lookDescription = "The ceiling has been reinforced several times here.  Possibly indicating a weak spot in the mine.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom107()
        {

            string examineDescription = "The cave in appears like has been here a while but was never cleared.";
            string lookDescription = "A tunnel to the south goes about five feet before a cave in seals the way.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom108()
        {

            string examineDescription = "The mushrooms glow with a pale blue light that is to dim to be any more than a novelty.";
            string lookDescription = "Small iridescent mushrooms glow faintly in the dark.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom109()
        {

            string examineDescription = "The mushrooms glow with a pale blue light that is to dim to be any more than a novelty.";
            string lookDescription = "Small iridescent mushrooms glow faintly in the dark.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom110()
        {

            string examineDescription = "The steel track appears to have been cut out and dragged away.";
            string lookDescription = "One side of the steel track is missing.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom111()
        {

            string examineDescription = "The steel track appears to have been cut out and dragged away.";
            string lookDescription = "The steel track has been removed from the area here.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom112()
        {

            string examineDescription = "The barricade failed though and something got through.";
            string lookDescription = "A make shift barricade was built here to hold back something.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom113()
        {

            string examineDescription = "The paintings show a fiery monster coming up out of the ground and attacking miners.";
            string lookDescription = "Several paintings are painted on the walls here.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Path Ore Cart Storage -- Gold Mine

        #region Gold Mine
        private IRoom GoldMine()
        {

            string examineDescription = "The fact that the earth does not collapse in filling the pit is a testament to the original dwarf miners ingenuity.";
            string lookDescription = "You at the edge of a great big open pit mine.";
            string shortDescription = "Ore Track";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        #region Mine Shaft
        private IRoom GenerateRoom47()
        {

            string examineDescription = "The small tunnel leading down seems to squeeze in on you from all around.";
            string lookDescription = "The roughly hewn mine shaft descends into the darkness below.";
            string shortDescription = "Dark Mine Shaft";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom48()
        {

            string examineDescription = "Water can be dribbling down the shaft wall on the east.  Perhaps the miners hit a natural underground stream in their quest for gold.";
            string lookDescription = "The roughly hewn mine shaft descends into the darkness below.";
            string shortDescription = "Dark Mine Shaft";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom49()
        {

            string examineDescription = "You briefly hear a the sound of a far off cry of help as if someone is falling off a ladder and then silence.";
            string lookDescription = "The roughly hewn mine shaft descends into the darkness below.";
            string shortDescription = "Dark Mine Shaft";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom50()
        {

            string examineDescription = "Stopping momentarily on the ladder you feel a icy brush as if something feel past you.";
            string lookDescription = "The roughly hewn mine shaft descends into the darkness below.";
            string shortDescription = "Dark Mine Shaft";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom51()
        {

            string examineDescription = "The walls still bear the marks of the pick axes that carved the shaft in search for more gold.";
            string lookDescription = "The roughly hewn mine shaft descends into the darkness below.";
            string shortDescription = "Dark Mine Shaft";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Mine Shaft

        #region Gold Mine Floor
        private IRoom GoldMineFloorRoom1()
        {

            string examineDescription = "There cavern walls occasionally sparkle here hinting that there may still be gold in these cave walls.";
            string lookDescription = "The room opens up into a large area hinting at a natural cavern of sorts.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GoldMineFloorConnectingTunnel()
        {

            string examineDescription = "The tunnel walls are covered in soot as is something has been burned in it.";
            string lookDescription = "The tunnel twists slightly slowly rising and falling as you continue to make your way through.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GoldMineFloorRoom2()
        {

            string examineDescription = "The room is about twenty five feet in height and forty feet in diameter.  The column is about two feet wide at the base and two inches in the middle.";
            string lookDescription = "The room opens up again to a natural dome with a single pillar in the center where a stalactite and stalagmite have met.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GoldMineFloorRoom3()
        {

            string examineDescription = "The magma is slow and poses no immediate danger other than falling into the crevice.  The temperature of the room though has risen to a slightly warmish temperature.";
            string lookDescription = "The cavern glows with a dull red as magma slowly flows from a hole in the wall to the east down into a deep crevice and to the west.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        #region GoldMineFloorRoom4
        private IRoom GenerateRoom73()
        {

            string examineDescription = "The stone throne to the north is huge standing sixteen feet to the seat and faces to the north.";
            string lookDescription = "A large stone throne dominates the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom74()
        {

            string examineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            string lookDescription = "A large pedestal supports a massive brazier giving light to the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom75()
        {

            string examineDescription = "The stone throne to the west is huge standing sixteen feet to the seat and faces to the north.";
            string lookDescription = "A large stone throne dominates the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom76()
        {

            string examineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            string lookDescription = "A large pedestal supports a massive brazier giving light to the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom77()
        {

            string examineDescription = "The stone throne to the south is huge standing sixteen feet to the seat and faces to the north.";
            string lookDescription = "A large stone throne dominates the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Balrog(room));

            return room;
        }

        private IRoom GenerateRoom78()
        {

            string examineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            string lookDescription = "A large pedestal supports a massive brazier giving light to the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom79()
        {

            string examineDescription = "The stone throne to the east is huge standing sixteen feet to the seat and faces to the north.";
            string lookDescription = "A large stone throne dominates the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom80()
        {

            string examineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            string lookDescription = "A large pedestal supports a massive brazier giving light to the room.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion GoldMineFloorRoom4

        private IRoom GoldMineFloorRoom5()
        {

            string examineDescription = "Dozen of reflections of yourself can be seen in the cavern walls.  Short and fat as well as tall and thin versions of yourself.";
            string lookDescription = "The walls of the room are made of black obsidian glass creating a fun house effect with your reflection.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GoldMineFloorRoom6()
        {

            string examineDescription = "Hints of rich gold veins still sparkles through parts of the cavern walls.";
            string lookDescription = "This small room seems to be mostly untouched by the dwarven miners.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GoldMineFloorRoom7()
        {

            string examineDescription = "It would seem to be that the dwarves stumbled upon a underground section of rock salt.";
            string lookDescription = "Small salt crystals protrude from the cavern walls.";
            string shortDescription = "Dark Mine Floor";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        #endregion Gold Mine Floor
        #endregion Gold Mine

        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Balrog(IRoom room)
        {
            string examineDescription = "The demon is ablaze with fire will smoke hides its true form from view.";
            string lookDescription = "A large demon of fire and smoke standing twenty feet tall.";
            string shortDescription = "A large flaming Balrog.";
            string sentenceDescription = "Balrog";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 87);
            npc.KeyWords.Add("Balrog");
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());

            npc.Enchantments.Add(FireAura());
            npc.AddEquipment(BalrogSword());

            npc.Race.Fire = decimal.MaxValue;
            npc.Race.Poison = decimal.MaxValue;
            npc.Race.Cold = 1.5M;
            npc.Race.Lightning = 1.5M;
            npc.Race.Bludgeon = 1.5M;
            npc.Race.Pierce = 1.5M;
            npc.Race.Slash = 1.5M;

            return npc;
        }

        private IEnchantment FireAura()
        {
            IEnchantment enchantment = new DamageDealtAfterDefenseEnchantment();
            IEffect effect = new Damage();
            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.TargetMessage = new TranslationMessage("The fire from the Balrog burns you.");
            effectParameter.Description = "fire from the Balrog";

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect;
            enchantment.Parameter = effectParameter;

            effectParameter.Damage = new Objects.Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(84), DamageType.Fire);

            return enchantment;
        }

        private IWeapon BalrogSword()
        {
            string examineDescription = "As you get closer to the sword flames flair up engulfing the sword in fire and smoke choking the air and making it hard to determine its true size and shape.";
            string lookDescription = "When the Balrog wielded the sword it had flames leaping from it, now it just smolders.";
            string shortDescription = "A large flaming sword.";
            string sentenceDescription = "sword";

            IWeapon weapon = CreateWeapon(WeaponType.Sword, 87, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.RequiredHands = 2;
            weapon.KeyWords.Add("Balrog");

            IDamage damage = new Objects.Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level), DamageType.Fire);
            weapon.DamageList.Add(damage);

            damage = new Objects.Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level), DamageType.Poison);
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private INonPlayerCharacter Shadow(IRoom room)
        {
            string examineDescription = "The dark figure is hard to see and blends into the shadows.";
            string lookDescription = "The figure seems to fade in and out of existence as it moves among the shadows.";
            string shortDescription = "A shadowy figure.";
            string sentenceDescription = "shadow";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 14);
            npc.KeyWords.Add("Shadow");
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription =
            npc.LookDescription =
            npc.ShortDescription =
            npc.SentenceDescription = "shadow";

            npc.Race.Acid = 1.5M;
            npc.Race.Cold = 1.5M;
            npc.Race.Fire = 1.5M;
            npc.Race.Lightning = 1.5M;
            npc.Race.Thunder = 1.5M;
            npc.Race.Bludgeon = 1.5M;
            npc.Race.Pierce = 1.5M;
            npc.Race.Slash = 1.5M;

            npc.Race.Necrotic = Decimal.MaxValue;
            npc.Race.Poison = Decimal.MaxValue;

            npc.Race.Radiant = .5M;

            return npc;
        }
        #endregion NPC
    }
}
