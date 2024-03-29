﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Room;
using Objects.Personality;
using static Objects.Guild.Guild;
using Objects.LevelRange;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using static Objects.Mob.NonPlayerCharacter;
using MiscShared;

namespace GenerateZones.Zones
{
    public class GrandViewColiseum : BaseZone, IZoneCode
    {
        public GrandViewColiseum() : base(3)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewColiseum);
            Zone.ZoneObjectSyncOptions = 2;

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 5, 19);
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.South, 4, 1);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.Down, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.Up, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.Up, Zone.Rooms[16]);

            #region Basement/Arena
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.South, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.South, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[14]);

            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.South, Zone.Rooms[19]);

            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.East, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.South, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.South, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.South, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.East, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.South, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.South, Zone.Rooms[24]);

            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.East, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.South, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.East, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.South, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.South, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.South, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.South, Zone.Rooms[29]);

            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.East, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.East, Zone.Rooms[29]);
            #endregion Basement/Arena

            #region Seating
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.Up, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.East, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.South, Zone.Rooms[31]);

            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.Up, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.East, Zone.Rooms[32]);

            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.Up, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.East, Zone.Rooms[33]);

            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.Up, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.North, Zone.Rooms[34]);

            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.Up, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.North, Zone.Rooms[35]);

            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.Up, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.West, Zone.Rooms[36]);

            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.Up, Zone.Rooms[44]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.West, Zone.Rooms[37]);

            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.Up, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.South, Zone.Rooms[30]);

            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.South, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.South, Zone.Rooms[39]);

            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.East, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.East, Zone.Rooms[41]);

            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.North, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.North, Zone.Rooms[43]);

            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.West, Zone.Rooms[44]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.West, Zone.Rooms[45]);
            #endregion Seating
        }


        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "It is hard to judge the full size of the coliseum from the ground.  The coliseum extends as far as the eye can see to the north and south and towers above you to the east.";
            string lookDescription = "The coliseum towers far dizzily above you.  Different color pennant are affixed to poles going around the top of the coliseum.  From down here it is hard to see what is on them.";
            string shortDescription = "Before the coliseum.";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            INonPlayerCharacter ticketVendor = TicketVendor(room);
            room.AddMobileObjectToRoom(ticketVendor);
            ticketVendor.Room = room;

            return room;
        }

        private INonPlayerCharacter TicketVendor(IRoom room)
        {
            string examineDescription = "The vendor is dressed in a white shirt with gray pants.  He is wearing a red straw hat that looks to be two sizes to big slightly off to one side.";
            string lookDescription = "The ticket vendor holds 3 tickets in his hand and is trying to sell it to anyone willing to listen.";
            string sentenceDescription = "ticket vendor";
            string shortDescription = "A ticket vendor selling tickets.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 3);
            npc.KeyWords.Add("vendor");

            Speaker speaker = new Speaker();
            speaker.ThingsToSay.Add("Get your tickets to the gladiator games.");
            speaker.ThingsToSay.Add("I got three tickets to the games left.  Get them while they last.");

            npc.Personalities.Add(speaker);
            return npc;
        }

        #region Basement
        private IRoom GenerateRoom2()
        {
            string examineDescription = "The coliseum is built out of massive limestones. Some of the stones are staggering and baffle the imagination causing you to wonder how this was built in the first place.";
            string lookDescription = "You stand in the entrance to the coliseum.  To the east you can see the arena floor.  Down you can see stairs leading beneath the arena where the gladiators prepare.";
            string shortDescription = "Entrance to the coliseum";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription, 2);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "While the smell of smoke is strong you can start to make out the smells of animals used in the gladiatorial fights.  You can hear the sounds of lions, tigers and bears off in the distance.";
            string lookDescription = "The air quickly turns smoky as torches are the sole source of light the area under the coliseum.";
            string shortDescription = "Beneath the coliseum";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription, 2);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            INonPlayerCharacter guildMaster = GladiatorGuildMaster(room);

            room.AddMobileObjectToRoom(guildMaster);

            return room;
        }

        private INonPlayerCharacter GladiatorGuildMaster(IRoom room)
        {
            string examineDescription = "The Guildmaster is dressed gray studded leather armor.  A blue eagle is emblazoned across his chest but has begun to fade with use.  His arms are squeezed into gauntlets but bulging biceps tell you that you should think twice before picking a fight.";
            string lookDescription = "He looks at you with an annoyed look on his face before returning to his previous activities.";
            string sentenceDescription = "Guildmaster";
            string shortDescription = "The Gladiator Guildmaster.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);
            npc.KeyWords.Add("GuildMaster");
            npc.KeyWords.Add("Gladiator");
            npc.Personalities.Add(new GuildMaster(Guilds.Gladiator));
            return npc;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The ladder is made of brass rings attached to a single pole that runs the height of the room.";
            string lookDescription = "This part of the coliseum basement only has two exits. A ladder leading up or a path leading back to the rest of the basement.";
            string shortDescription = "Beneath the coliseum";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription, 2);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            return room;
        }
        #endregion Basement

        #region Coliseum Arena
        private IRoom GenerateRoom5()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far north west corner of the arena.  To the south east you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The wall of the arena made to look like a castle is to the north.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);


            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The wall of the arena made to look like a castle is to the north.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);


            List<string> arenaExamine = new List<string>() { "The wall of the arena made to look like a castle gate is to the north.  You think back to \"Battle\" which was more of a slaughter as the king forced the peasants to defend the city.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);
            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The wall of the arena made to look like a castle is to the north.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far north east of the arena.  To the south west you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The wall of the arena made to look like a castle is to the north.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            INonPlayerCharacter arenaMob = ArenaMob(room);
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            INonPlayerCharacter arenaMob = ArenaMob(room);
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            arenaMob = ArenaMob(room);
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom16()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            INonPlayerCharacter arenaMob = ArenaMob(room);
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "From down here the arena floor looks much bigger but also more dangerous.";
            string lookDescription = "You stand at the center of the arena floor with the arena extending in a all directions.  Beyond the arena floor the stadium seats extend upward almost out of sight.";
            string shortDescription = "Center of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            return room;
        }

        private IRoom GenerateRoom18()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom20()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom21()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            INonPlayerCharacter arenaMob = ArenaMob(room);
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom25()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far south west of the arena.  To the north east you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom26()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom27()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom28()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "The battlefield grass is a luscious green.  The arena workers did a good job of recreating the fertile lands of Nulst.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private IRoom GenerateRoom29()
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "You stand at the far south east of the arena.  To the north west you can see {ArenaLong}.";
            string shortDescription = "Inside of the coliseum arena";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 2);

            List<string> arenaExamine = new List<string>() { "Broken swords and spears are strewn here and there amongst the bodies.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        #endregion Coliseum Arena

        #region Coliseum Stands
        private IRoom GenerateRoom30()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom31()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom32()
        {
            IRoom room = AreanaSeating();
            INonPlayerCharacter npc = ArenaSpecator(room);
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom33()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom34()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom35()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom36()
        {
            return AreanaSeating();
        }

        private IRoom GenerateRoom37()
        {
            return AreanaSeating();
        }

        private IRoom AreanaSeating()
        {
            string examineDescription = "The arena seats are made of white marble that shine in the sun and glow in the moonlight.";
            string lookDescription = "The height of the seating in this area is about perfect.  You can see the entire arena while making out the participants.";
            string shortDescription = "Arena seating";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom GenerateRoom38()
        {
            IRoom room = AreanaNoseBleed();
            INonPlayerCharacter npc = ArenaSpecator(room);
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom39()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom40()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom41()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom42()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom43()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom44()
        {
            return AreanaNoseBleed();
        }

        private IRoom GenerateRoom45()
        {
            IRoom room = AreanaNoseBleed();
            INonPlayerCharacter npc = ArenaSpecator(room);
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom46()
        {
            string examineDescription = "You can see why the King and Queen have seats here.  This is by far the best seats in the house.";
            string lookDescription = "The King and Queens thrown's are here overlooking the arena floor below.";
            string shortDescription = "Box seating";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            return room;
        }

        private IRoom AreanaNoseBleed()
        {
            string examineDescription = "The nose bleed arena seats are made of wood.  At least they don't have splinters.";
            string lookDescription = "The nose bleed section of the arena is a bit high to see anything but when the arena is full the energy of the crowd makes this a good seat.";
            string shortDescription = "Nose bleed seating";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        #endregion Coliseum Stands

        #endregion End Rooms

        #region Npcs
        private INonPlayerCharacter ArenaMob(IRoom room)
        {
            string examineDescription = "{ArenaExamine}";
            string lookDescription = "{ArenaLong}";
            string sentenceDescription = "{ArenaSentence}";
            string shortDescription = "{ArenaShort}";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());
            npc.LevelRange = new LevelRange() { LowerLevel = 1, UpperLevel = 5 };

            List<string> arenaExamine = new List<string>() { "The knight is dressed in armor with a {knightColor1} {knightAnimal1} on the chest.  His shield has his family crest painted on it.  The crest is made of {knightAmount} {knightColor2} {knightAnimal2} painted on a shield of {knightColor3} and {knightColor4}.",
                                                                "The lion has a large mane with shades of orange and golden hair that catches the light as it walks." };
            List<string> arenaLong = new List<string>() { "The gladiator is dressed in full armor is made to look like a knight of Struig."
                                                           , "While this is the thinest lion you have seen that just means its the hungriest lion you have seen which makes it the most dangerous." };
            List<string> arenaShort = new List<string>() {"A gladiator dressed as knight stands here waiting for a peasant to wander by."
                                                           , "A hungry lion roams the savannah looking for a meal." };
            List<string> arenaSentence = new List<string>() { "Knight of Struig"
                                                           , "Hungry lion" };

            List<string> keywords = new List<string>() { "Knight,Struig"
                                                           , "lion" };

            npc.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            npc.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);
            npc.ZoneSyncOptions.Add("{ArenaShort}", arenaShort);
            npc.ZoneSyncOptions.Add("{ArenaSentence}", arenaSentence);
            npc.ZoneSyncOptions.Add("ZoneSyncKeywords", keywords);

            #region Knight
            List<string> knightColor1 = new List<string>() { "black", "red", "brown" };
            List<string> knightAnimal1 = new List<string>() { "lion", "dragon", "snake", "horse" };
            List<string> knightAmount = new List<string>() { "a pair of", "three" };
            List<string> knightColor2 = new List<string>() { "white", "gold", "yellow" };
            List<string> knightAnimal2 = new List<string>() { "lions", "gryphons", "eagles", "bears" };
            List<string> knightColor3 = new List<string>() { "white", "gold", "black" };
            List<string> knightColor4 = new List<string>() { "red", "blue", "green" };

            npc.FlavorOptions.Add("{knightColor1}", knightColor1);
            npc.FlavorOptions.Add("{knightAnimal1}", knightAnimal1);

            npc.FlavorOptions.Add("{knightAmount}", knightAmount);
            npc.FlavorOptions.Add("{knightColor2}", knightColor2);
            npc.FlavorOptions.Add("{knightAnimal2}", knightAnimal2);
            npc.FlavorOptions.Add("{knightColor3}", knightColor3);
            npc.FlavorOptions.Add("{knightColor4}", knightColor4);
            #endregion Knight

            return npc;
        }

        private INonPlayerCharacter ArenaSpecator(IRoom room)
        {
            string examineDescription = "The spectator looks onward towards the arena floor hoping to catch a glimpse of their favorite gladiator in action.";
            string lookDescription = "Unable to decided if they should sit or stand this fan is contently jumping up and down and changing seats trying to get a better view of the action blow.";
            string sentenceDescription = "A spectator";
            string shortDescription = "A spectator sits nervously while watching the games below.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 3);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("spectator");

            return npc;
        }
        #endregion Npcs

    }
}