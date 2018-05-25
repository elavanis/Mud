using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Magic;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Zone;
using Objects.Room;
using Objects.Personality.Personalities;
using static Objects.Guild.Guild;
using Objects.LevelRange;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Personality.Personalities.Interface;

namespace GenerateZones.Zones
{
    public class GrandViewColiseum : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        //int itemId = 1;
        int npcId = 1;
        public IZone Generate()
        {
            zone.Id = 3;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(GrandViewColiseum);
            zone.ZoneObjectSyncOptions = 2;

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            ConnectRooms();

            return zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 5;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "It is hard to judge the full size of the coliseum from the ground.  The coliseum extends as far as the eye can see to the north and south and towers above you to the east.";
            room.LongDescription = "The coliseum towers far dizzily above you.  Different color pennant are affixed to poles going around the top of the coliseum.  From down here it is hard to see what is on them.";
            room.ShortDescription = "Before the coliseum.";

            INonPlayerCharacter ticketVendor = TicketVendor();
            room.AddMobileObjectToRoom(ticketVendor);
            ticketVendor.Room = room;

            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            return room;
        }

        private INonPlayerCharacter TicketVendor()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();

            npc.Id = npcId++;
            npc.Level = 3;
            npc.ExamineDescription = "The vendor is dressed in a white shirt with gray pants.  He is wearing a red straw hat that looks to be two sizes to big slightly off to one side.";
            npc.LongDescription = "The ticket vendor holds 3 tickets in his hand and is trying to sell it to anyone willing to listen.";
            npc.ShortDescription = "A ticket vendor selling tickets.";
            npc.SentenceDescription = "ticket vendor";
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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "The coliseum is built out of massive limestones. Some of the stones are staggering and baffle the imagination causing you to wonder how this was built in the first place.";
            room.LongDescription = "You stand in the entrance to the coliseum.  To the east you can see the arena floor.  Down you can see stairs leading beneath the arena where the gladiators prepare.";
            room.ShortDescription = "Entrance to the coliseum";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "While the smell of smoke is strong you can start to make out the smells of animals used in the gladiatorial fights.  You can hear the sounds of lions, tigers and bears off in the distance.";
            room.LongDescription = "The air quickly turns smoky as torches are the sole source of light the area under the coliseum.";
            room.ShortDescription = "Beneath the coliseum";

            INonPlayerCharacter guildMaster = WarriorGuildMaster();

            room.AddMobileObjectToRoom(guildMaster);

            return room;
        }

        private INonPlayerCharacter WarriorGuildMaster()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 20;
            npc.ExamineDescription = "The Guildmaster is dressed gray studded leather armor.  A blue eagle is emblazoned across his chest but has begun to fade with use.  His arms are squeezed into gauntlets but bulging biceps tell you that you should think twice before picking a fight.";
            npc.LongDescription = "He looks at you with an annoyed look on his face before returning to his previous activities.";
            npc.ShortDescription = "The warrior Guildmaster.";
            npc.SentenceDescription = "Guildmaster";
            npc.KeyWords.Add("GuildMaster");
            npc.KeyWords.Add("Warrior");
            npc.Personalities.Add(new GuildMaster(Guilds.Warrior));
            return npc;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "The ladder is made of brass rings attached to a single pole that runs the height of the room.";
            room.LongDescription = "This part of the coliseum basement only has two exits. A ladder leading up or a path leading back to the rest of the basement.";
            room.ShortDescription = "Beneath the coliseum";

            return room;
        }
        #endregion Basement

        #region Coliseum Arena
        private IRoom GenerateRoom5()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far north west corner of the arena.  To the south east you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";


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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far north of the arena.  To the south you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far north east of the arena.  To the south west you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

            INonPlayerCharacter arenaMob = ArenaMob();
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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

            INonPlayerCharacter arenaMob = ArenaMob();
            room.AddMobileObjectToRoom(arenaMob);
            arenaMob.Room = room;

            arenaMob = ArenaMob();
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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

            INonPlayerCharacter arenaMob = ArenaMob();
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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "From down here the arena floor looks much bigger but also more dangerous.";
            room.LongDescription = "You stand at the center of the arena floor with the arena extending in a all directions.  Beyond the arena floor the stadium seats extend upward almost out of sight.";
            room.ShortDescription = "Center of the coliseum arena";

            return room;
        }

        private IRoom GenerateRoom18()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far west of the arena.  To the east you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand in the middle of the arena.  All around you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

            INonPlayerCharacter arenaMob = ArenaMob();
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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far east of the arena.  To the west you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far south west of the arena.  To the north east you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far south of the arena.  To the north you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

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
            IRoom room = GenerateRoom();
            room.MovementCost = 2;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "{ArenaExamine}";
            room.LongDescription = "You stand at the far south east of the arena.  To the north west you can see {ArenaLong}.";
            room.ShortDescription = "Inside of the coliseum arena";

            List<string> arenaExamine = new List<string>() { "Broken swords and spears are strewn here and there amongst the bodies.",
                                                                "Waist high grass covers the area here making it hard to see if anything might be hiding in wait." };
            List<string> arenaLong = new List<string>() { "the green grass from the Battle of Nulst reenactment"
                                                           , "the savannah stretch out before you hiding untold wild animals that would love to eat a tasty adventurer like yourself" };

            room.ZoneSyncOptions.Add("{ArenaExamine}", arenaExamine);
            room.ZoneSyncOptions.Add("{ArenaLong}", arenaLong);

            return room;
        }

        private INonPlayerCharacter ArenaMob()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());

            //npc.Level = Shared.Random.random.Next(3) + 1;
            npc.Id = npcId++;
            npc.LevelRange = new LevelRange() { LowerLevel = 1, UpperLevel = 5 };
            npc.ExamineDescription = "{ArenaExamine}";
            npc.LongDescription = "{ArenaLong}";
            npc.ShortDescription = "{ArenaShort}";
            npc.SentenceDescription = "{ArenaSentence}";

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
            INonPlayerCharacter npc = ArenaSpecator();
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
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The arena seats are made of white marble that shine in the sun and glow in the moonlight.";
            room.LongDescription = "The height of the seating in this area is about perfect.  You can see the entire arena while making out the participants.";
            room.ShortDescription = "Arena seating";
            return room;
        }

        private IRoom GenerateRoom38()
        {
            IRoom room = AreanaNoseBleed();
            INonPlayerCharacter npc = ArenaSpecator();
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
            INonPlayerCharacter npc = ArenaSpecator();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom46()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "You can see why the King and Queen have seats here.  This is by far the best seats in the house.";
            room.LongDescription = "The King and Queens thrown's are here overlooking the arena floor below.";
            room.ShortDescription = "Box seating";
            return room;
        }

        private IRoom AreanaNoseBleed()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The nose bleed arena seats are made of wood.  At least they don't have splinters.";
            room.LongDescription = "The nose bleed section of the arena is a bit high to see anything but when the arena is full the energy of the crowd makes this a good seat.";
            room.ShortDescription = "Nose bleed seating";
            return room;
        }

        private INonPlayerCharacter ArenaSpecator()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Wanderer());

            npc.Id = npcId++;
            npc.Level = 3;
            npc.ExamineDescription = "The spectator looks onward towards the arena floor hoping to catch a glimpse of their favorite gladiator in action.";
            npc.LongDescription = "Unable to decided if they should sit or stand this fan is contently jumping up and down and changing seats trying to get a better view of the action blow.";
            npc.ShortDescription = "A spectator sits nervously while watching the games below.";
            npc.SentenceDescription = "A spectator";
            npc.KeyWords.Add("spectator");

            return npc;
        }
        #endregion Coliseum Stands

        #endregion End Rooms

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();


            ZoneHelper.ConnectZone(zone.Rooms[1], Direction.West, 6, 27);
            ZoneHelper.ConnectZone(zone.Rooms[1], Direction.South, 4, 1);

            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.Down, zone.Rooms[2]);
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.Up, zone.Rooms[30]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.East, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.Up, zone.Rooms[16]);

            #region Basement/Arena
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.East, zone.Rooms[6]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.South, zone.Rooms[10]);
            ZoneHelper.ConnectRoom(zone.Rooms[6], Direction.East, zone.Rooms[7]);
            ZoneHelper.ConnectRoom(zone.Rooms[6], Direction.South, zone.Rooms[11]);
            ZoneHelper.ConnectRoom(zone.Rooms[7], Direction.East, zone.Rooms[8]);
            ZoneHelper.ConnectRoom(zone.Rooms[7], Direction.South, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.East, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.South, zone.Rooms[13]);
            ZoneHelper.ConnectRoom(zone.Rooms[9], Direction.South, zone.Rooms[14]);

            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.East, zone.Rooms[11]);
            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.South, zone.Rooms[15]);
            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.East, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.South, zone.Rooms[16]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.East, zone.Rooms[13]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.South, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.East, zone.Rooms[14]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.South, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.South, zone.Rooms[19]);

            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.East, zone.Rooms[16]);
            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.South, zone.Rooms[20]);
            ZoneHelper.ConnectRoom(zone.Rooms[16], Direction.East, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[16], Direction.South, zone.Rooms[21]);
            ZoneHelper.ConnectRoom(zone.Rooms[17], Direction.East, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[17], Direction.South, zone.Rooms[22]);
            ZoneHelper.ConnectRoom(zone.Rooms[18], Direction.East, zone.Rooms[19]);
            ZoneHelper.ConnectRoom(zone.Rooms[18], Direction.South, zone.Rooms[23]);
            ZoneHelper.ConnectRoom(zone.Rooms[19], Direction.South, zone.Rooms[24]);

            ZoneHelper.ConnectRoom(zone.Rooms[20], Direction.East, zone.Rooms[21]);
            ZoneHelper.ConnectRoom(zone.Rooms[20], Direction.South, zone.Rooms[25]);
            ZoneHelper.ConnectRoom(zone.Rooms[21], Direction.East, zone.Rooms[22]);
            ZoneHelper.ConnectRoom(zone.Rooms[21], Direction.South, zone.Rooms[26]);
            ZoneHelper.ConnectRoom(zone.Rooms[22], Direction.East, zone.Rooms[23]);
            ZoneHelper.ConnectRoom(zone.Rooms[22], Direction.South, zone.Rooms[27]);
            ZoneHelper.ConnectRoom(zone.Rooms[23], Direction.East, zone.Rooms[24]);
            ZoneHelper.ConnectRoom(zone.Rooms[23], Direction.South, zone.Rooms[28]);
            ZoneHelper.ConnectRoom(zone.Rooms[24], Direction.South, zone.Rooms[29]);

            ZoneHelper.ConnectRoom(zone.Rooms[25], Direction.East, zone.Rooms[26]);
            ZoneHelper.ConnectRoom(zone.Rooms[26], Direction.East, zone.Rooms[27]);
            ZoneHelper.ConnectRoom(zone.Rooms[27], Direction.East, zone.Rooms[28]);
            ZoneHelper.ConnectRoom(zone.Rooms[28], Direction.East, zone.Rooms[29]);
            #endregion Basement/Arena

            #region Seating
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.Up, zone.Rooms[38]);
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.East, zone.Rooms[46]);
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.South, zone.Rooms[31]);

            ZoneHelper.ConnectRoom(zone.Rooms[31], Direction.Up, zone.Rooms[39]);
            ZoneHelper.ConnectRoom(zone.Rooms[31], Direction.East, zone.Rooms[32]);

            ZoneHelper.ConnectRoom(zone.Rooms[32], Direction.Up, zone.Rooms[40]);
            ZoneHelper.ConnectRoom(zone.Rooms[32], Direction.East, zone.Rooms[33]);

            ZoneHelper.ConnectRoom(zone.Rooms[33], Direction.Up, zone.Rooms[41]);
            ZoneHelper.ConnectRoom(zone.Rooms[33], Direction.North, zone.Rooms[34]);

            ZoneHelper.ConnectRoom(zone.Rooms[34], Direction.Up, zone.Rooms[42]);
            ZoneHelper.ConnectRoom(zone.Rooms[34], Direction.North, zone.Rooms[35]);

            ZoneHelper.ConnectRoom(zone.Rooms[35], Direction.Up, zone.Rooms[43]);
            ZoneHelper.ConnectRoom(zone.Rooms[35], Direction.West, zone.Rooms[36]);

            ZoneHelper.ConnectRoom(zone.Rooms[36], Direction.Up, zone.Rooms[44]);
            ZoneHelper.ConnectRoom(zone.Rooms[36], Direction.West, zone.Rooms[37]);

            ZoneHelper.ConnectRoom(zone.Rooms[37], Direction.Up, zone.Rooms[45]);
            ZoneHelper.ConnectRoom(zone.Rooms[37], Direction.South, zone.Rooms[30]);

            ZoneHelper.ConnectRoom(zone.Rooms[45], Direction.South, zone.Rooms[38]);
            ZoneHelper.ConnectRoom(zone.Rooms[38], Direction.South, zone.Rooms[39]);

            ZoneHelper.ConnectRoom(zone.Rooms[39], Direction.East, zone.Rooms[40]);
            ZoneHelper.ConnectRoom(zone.Rooms[40], Direction.East, zone.Rooms[41]);

            ZoneHelper.ConnectRoom(zone.Rooms[41], Direction.North, zone.Rooms[42]);
            ZoneHelper.ConnectRoom(zone.Rooms[42], Direction.North, zone.Rooms[43]);

            ZoneHelper.ConnectRoom(zone.Rooms[43], Direction.West, zone.Rooms[44]);
            ZoneHelper.ConnectRoom(zone.Rooms[44], Direction.West, zone.Rooms[45]);
            #endregion Seating
        }
    }
}