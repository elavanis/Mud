using System.Linq;
using System.Reflection;
using MiscShared;
using Objects.Item.Items;
using Objects.Item.Items.EnchantersTower;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Personality.Custom.EnchantersTower;
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

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
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
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);

            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 22, 3, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", "The door slides closed with the sound of stone sliding on stone.", true, "The door depicts a bear standing on its hind legs."));
            ZoneHelper.ConnectZone(Zone.Rooms[2], Direction.North, 22, 7, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", "The door slides closed with the sound of stone sliding on stone.", true, "The door depicts a fish swallowing a man."));
            ZoneHelper.ConnectZone(Zone.Rooms[3], Direction.East, 22, 11, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", "The door slides closed with the sound of stone sliding on stone.", true, "The door depicts a lion killing an antelope."));
            ZoneHelper.ConnectZone(Zone.Rooms[4], Direction.South, 22, 15, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", "The door slides closed with the sound of stone sliding on stone.", true, "The door depicts a eagle catching a fish."));
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton(room));
            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton(room));
            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton(room));
            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GroundFloor();
            room.AddMobileObjectToRoom(DusterAutomaton(room));
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
            string shortDescription = "Top Floor";
            string examineDescription = "A large collection of mirrors and lenses seem to be setup to collect energy into a single point in the center of the room.";
            string lookDescription = "Everything in the room seems to be focused on the center pedestal.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            examineDescription = "The pedestal has a socket that looks to be designed to hold a focusing item.";
            lookDescription = "The pedestal is made of a creamy white stone.";
            shortDescription = "A white stone pedestal stands in the center of the room with everything else focused on it.";
            string sentenceDescription = "pedestal";
            string openMessage = "";
            string closeMessage = "";
            Container pedestal = CreateContainer(openMessage, closeMessage, examineDescription, lookDescription, sentenceDescription, shortDescription);
            pedestal.KeyWords.Add("pedestal");
            pedestal.KeyWords.Add("stone");
            pedestal.Attributes.Add(ItemAttribute.NoGet);

            room.AddItemToRoom(pedestal);
            room.AddMobileObjectToRoom(ButlerAutomaton(room));

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string shortDescription = "Top Floor";
            string examineDescription = "The room has a small window over looking the plateau to the east.";
            string lookDescription = "A pair of five inch holes exist in the ceiling and the floor.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.AddMobileObjectToRoom(ButlerAutomaton(room));

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = Stairs();

            room.AddMobileObjectToRoom(ButlerAutomaton(room));

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string shortDescription = "Enchanting Room";
            string examineDescription = "The table glows faintly with the residual energy of the thousands of enchantments performed on it.";
            string lookDescription = "A small table sits in the center of the room with light filtering down on it form a five inch hole in the ceiling.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Enchantery());
            room.AddMobileObjectToRoom(ButlerAutomaton(room));

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string shortDescription = "Holding Cells";
            string lookDescription = "There are several cages along the walls used for holding prisoners.";
            string examineDescription = "Some of the cages show signs of distress where prisoners have tried to escape.  Whether they were successful or not is anyones guess.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(ButlerAutomaton(room));
            room.AddMobileObjectToRoom(GoblinDaughter(room));
            room.AddMobileObjectToRoom(GuardAutomaton(room));

            return room;
        }

        private IRoom Stairs()
        {
            string shortDescription = "Spiral Staircase";
            string examineDescription = "The stairs are surprisingly made of wood instead of stone and creak slightly as you walk on them.";
            string lookDescription = "The spiral stairs ascend up the tower as well as down into a basement area.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom GroundFloor()
        {
            string shortDescription = "Ground Floor";
            string examineDescription = "The stone hallway is lit with a pair of torches ever twelve feet.";
            string lookDescription = "A stone hallway leading outside the tower and deeper inside toward a stairwell.";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }
        #endregion Rooms

        #region Npc
        private INonPlayerCharacter DusterAutomaton(IRoom room)
        {
            string examineDescription = "The automaton wanders aimlessly around dusting things with a feather duster.";
            string lookDescription = "The automaton dust both high and low leaving no dust safe.";
            string sentenceDescription = "dusting automaton";
            string shortDescription = "A duster automaton.";

            INonPlayerCharacter npc = GenerateAutomaton(room, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.KeyWords.Add("duster");

            return npc;
        }

        private INonPlayerCharacter GenerateAutomaton(IRoom room, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);
            npc.KeyWords.Add("automaton");
            npc.Personalities.Add(new Wanderer());
            return npc;
        }

        private INonPlayerCharacter ButlerAutomaton(IRoom room)
        {
            string examineDescription = "The automaton is always alert for the slightest sign that its master needs it to perform a duty.";
            string lookDescription = "The automaton walks around tiding up the place and waits for instructions form its master.";
            string shortDescription = "A butler automaton.";
            string sentenceDescription = "butler automaton";

            INonPlayerCharacter npc = GenerateAutomaton(room, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.KeyWords.Add("butler");

            return npc;
        }

        private INonPlayerCharacter GoblinDaughter(IRoom room)
        {
            string examineDescription = "The goblin chiefs daughter has been crying but have been given free rein of the room.";
            string lookDescription = "The goblin chiefs daughter appears to be in good health.";
            string shortDescription = "Goblin chief daughter.";
            string sentenceDescription = "goblin chief daughter";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);

            npc.KeyWords.Add("goblin");
            npc.KeyWords.Add("daughter");
            npc.Personalities.Add(new FollowPlayerAfterRescue());

            return npc;
        }

        private INonPlayerCharacter GuardAutomaton(IRoom room)
        {
            string examineDescription = "The guard stands at the door ready to keep any of the prisoners from escaping.";
            string lookDescription = "The guard seems to allow prisoners to roam around the room.";
            string shortDescription = "A guard automaton.";
            string sentenceDescription = "guard automaton";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);
            npc.KeyWords.Add("automaton");
            npc.KeyWords.Add("guard");

            return npc;
        }
        #endregion Npc

        #region Items
        private IEnchantery Enchantery()
        {
            string examineDescription = "The table once was a dark oak but with time and enchantments it has begun to glow a slight blue color casting a blue tint on everything in the room.";
            string lookDescription = "The table glows faintly as wisps of energy radiate up into the air before dissipating.";
            string shortDescription = "table";
            string sentenceDescription = "The table glows with wisps of energy radiating upward.";

            IEnchantery enchantery = CreateEnchantery(examineDescription, lookDescription, shortDescription, sentenceDescription);

            enchantery.Attributes.Add(ItemAttribute.NoGet);
            enchantery.KeyWords.Add("table");

            return enchantery;
        }

        #endregion Items
    }
}
