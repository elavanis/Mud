using System.Linq;
using System.Reflection;
using MiscShared;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality;
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
            Zone.Name = nameof(CharonTemple);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "Faint humming sounds can be heard from below.";
            string lookDescription = "Stairs descend downward into the darkness of the of the temple.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            string lookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            string lookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The blue fire does not radiate heat or produce sound.  Just a cold blue light.";
            string lookDescription = "The temple tunnel is lined on both sides with cauldrons of blue fire.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "A small shrine has been setup next to the dock.";
            string lookDescription = "There is dock extending twenty or thirty feet into a river that extends into the darkness.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Shrine());

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "In the center of the circle is a statue of Charon guiding his boat down the river with his mighty oar.";
            string lookDescription = "A small circle has been drawn in chalk on the cave floor.";
            string shortDescription = "Charon Temple";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            for (int i = 0; i < 8; i++)
            {
                room.AddMobileObjectToRoom(Priest(room));
            }
            return room;
        }
        #endregion Rooms

        #region Npcs
        private INonPlayerCharacter Priest(IRoom room)
        {
            string examineDescription = "Clothed in a gray tunic the priest stands in meditation while emitting a humming sound.";
            string lookDescription = "The priest mostly ignores you as all are welcome to the temple of Charon.";
            string sentenceDescription = "priest";
            string shortDescription = "A mediative priest stands in his place in the circle.";
            INonPlayerCharacter nonPlayerCharacter = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);

            nonPlayerCharacter.Personalities.Add(new Guardian());
            nonPlayerCharacter.KeyWords.Add("Priest");

            return nonPlayerCharacter;
        }
        #endregion Npcs

        #region Items
        private IItem Shrine()
        {
            string examineDescription = "The shrine is full of small coins carefully placed there.";
            string lookDescription = "The shrine is make of old wooden boat planks.";
            string sentenceDescription = "shrine";
            string shortDescription = "The shrine has a small lantern lit in the center of the wooden shrine.";
            IItem shrine = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            shrine.ZoneId = Zone.Id;
            shrine.SentenceDescription = "shrine";
            shrine.KeyWords.Add("shrine");

            return shrine;
        }
        #endregion Items

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

