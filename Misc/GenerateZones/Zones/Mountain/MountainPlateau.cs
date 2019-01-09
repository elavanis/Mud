using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;

namespace GenerateZones.Zones.Mountain
{
    public class MountainPlateau : BaseZone, IZoneCode
    {
        public MountainPlateau() : base(22)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(MountainPlateau);

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
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = Plateau();
            Container chest = Chest();
            chest.ExamineDescription = "The carvings show a mighty lion roaring into the distant.";
            room.AddItemToRoom(chest);
            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = Plateau();
            Container chest = Chest();
            chest.ExamineDescription = "The carvings show a majestic eagle soaring in the clouds.";
            room.AddItemToRoom(chest);
            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = Plateau();
            Container chest = Chest();
            chest.ExamineDescription = "The carvings show a bear climbing the tree of life.";
            room.AddItemToRoom(chest);
            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom GenerateRoom15()
        {
            IRoom room = Plateau();
            Container chest = Chest();
            chest.ExamineDescription = "The carvings show a large fish swallowing the world.";
            room.AddItemToRoom(chest);
            return room;
        }

        private IRoom GenerateRoom16()
        {
            IRoom room = Plateau();

            return room;
        }

        private IRoom Plateau()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ShortDescription = "Plateau";
            room.ExamineDescription = "The plateau is dominated by the tower in the center.  The rest is covered in small clumps of grass and lichen.";
            room.LookDescription = "A large tower rises up before you.";
            return room;
        }
        #endregion Rooms

        #region Items
        public Container Chest()
        {
            Container chest = CreateItem<Container>();
            //chest.ExamineDescription = "";
            chest.LookDescription = "The chest is made carefully chiseled stone.";
            chest.ShortDescription = "A small stone chest with intricate carvings.";
            chest.SentenceDescription = "chest";
            chest.KeyWords.Add("chest");
            chest.Attributes.Add(ItemAttribute.NoGet);

            return chest;
        }

        #endregion Items

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.North, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.West, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.West, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.West, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.West, Zone.Rooms[1]);

        }
    }
}
