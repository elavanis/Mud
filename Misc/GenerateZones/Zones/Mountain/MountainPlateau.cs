using System.Linq;
using System.Reflection;
using Objects;
using Objects.Effect.Zone.MountainPlateau;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
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
            Zone.Name = nameof(MountainPlateau);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
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
            IItem lion = Lion();
            chest.Items.Add(lion);

            IEnchantment get = new GetEnchantment();
            CloseDoor closeDoor = new CloseDoor();
            closeDoor.Chest = new BaseObjectId() { Zone = 22, Id = 1 };
            closeDoor.Statue = new BaseObjectId() { Zone = 22, Id = 6 };
            closeDoor.Door = new BaseObjectId() { Zone = 22, Id = 3 };
            get.Effect = closeDoor;
            get.ActivationPercent = 100;
            chest.Enchantments.Add(get);

            IEnchantment put = new PutEnchantment();
            OpenDoor openDoor = new OpenDoor();
            openDoor.Chest = new BaseObjectId() { Zone = 22, Id = 1 };
            openDoor.Statue = new BaseObjectId() { Zone = 22, Id = 6 };
            openDoor.Door = new BaseObjectId() { Zone = 22, Id = 3 };
            put.Effect = openDoor;
            put.ActivationPercent = 100;
            chest.Enchantments.Add(put);

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
            chest.Items.Add(Eagle());

            //IEnchantment get = new GetEnchantment();
            //CloseDoor closeDoor = new CloseDoor();
            //closeDoor.Chest = new BaseObjectId() { Zone = 22, Id = 4 };
            //closeDoor.Statue = new BaseObjectId() { Zone = 22, Id = 11 };
            //closeDoor.Door = new BaseObjectId() { Zone = 22, Id = 6 };
            //get.Effect = closeDoor;
            //get.ActivationPercent = 100;
            //chest.Enchantments.Add(get);

            //IEnchantment put = new PutEnchantment();
            //OpenDoor openDoor = new OpenDoor();
            //openDoor.Chest = new BaseObjectId() { Zone = 22, Id = 4 };
            //openDoor.Statue = new BaseObjectId() { Zone = 22, Id = 9 };
            //openDoor.Door = new BaseObjectId() { Zone = 22, Id = 6 };
            //put.Effect = openDoor;
            //put.ActivationPercent = 100;
            //chest.Enchantments.Add(put);

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
            chest.Items.Add(Bear());

            //IEnchantment get = new GetEnchantment();
            //CloseDoor closeDoor = new CloseDoor();
            //closeDoor.Chest = new BaseObjectId() { Zone = 22, Id = 3 };
            //closeDoor.Statue = new BaseObjectId() { Zone = 22, Id = 8 };
            //closeDoor.Door = new BaseObjectId() { Zone = 22, Id = 5 };
            //get.Effect = closeDoor;
            //get.ActivationPercent = 100;
            //chest.Enchantments.Add(get);

            //IEnchantment put = new PutEnchantment();
            //OpenDoor openDoor = new OpenDoor();
            //openDoor.Chest = new BaseObjectId() { Zone = 22, Id = 3 };
            //openDoor.Statue = new BaseObjectId() { Zone = 22, Id = 8 };
            //openDoor.Door = new BaseObjectId() { Zone = 22, Id = 5 };
            //put.Effect = openDoor;
            //put.ActivationPercent = 100;
            //chest.Enchantments.Add(put);

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
            chest.Items.Add(Fish());

            //IEnchantment get = new GetEnchantment();
            //CloseDoor closeDoor = new CloseDoor();
            //closeDoor.Chest = new BaseObjectId() { Zone = 22, Id = 4 };
            //closeDoor.Statue = new BaseObjectId() { Zone = 22, Id = 8 };
            //closeDoor.Door = new BaseObjectId() { Zone = 22, Id = 5 };
            //get.Effect = closeDoor;
            //get.ActivationPercent = 100;
            //chest.Enchantments.Add(get);

            //IEnchantment put = new PutEnchantment();
            //OpenDoor openDoor = new OpenDoor();
            //openDoor.Chest = new BaseObjectId() { Zone = 22, Id = 4 };
            //openDoor.Statue = new BaseObjectId() { Zone = 22, Id = 8 };
            //openDoor.Door = new BaseObjectId() { Zone = 22, Id = 5 };
            //put.Effect = openDoor;
            //put.ActivationPercent = 100;
            //chest.Enchantments.Add(put);

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

        public IItem Lion()
        {
            IItem item = CreateItem<Item>();
            item.ExamineDescription = "The statue shows a male lion with a full mane.";
            item.LookDescription = "The statue is make of a sandy colored stone material.";
            item.ShortDescription = "A small statue of a lion.";
            item.SentenceDescription = "lion statue";
            item.KeyWords.Add("statue");
            item.KeyWords.Add("lion");

            return item;
        }

        public IItem Eagle()
        {
            IItem item = CreateItem<Item>();
            item.ExamineDescription = "The eagle in the statue is taking perched on a branch overlooking a nest of its young.";
            item.LookDescription = "The statue is make of a sandy colored stone material.";
            item.ShortDescription = "A small statue of a eagle.";
            item.SentenceDescription = "eagle statue";
            item.KeyWords.Add("statue");
            item.KeyWords.Add("eagle");

            return item;
        }

        public IItem Bear()
        {
            IItem item = CreateItem<Item>();
            item.ExamineDescription = "A statue depicting bear reaching out on a tree branch to reach a bee's hive.";
            item.LookDescription = "The statue is make of a sandy colored stone material.";
            item.ShortDescription = "A small statue of a bear.";
            item.SentenceDescription = "bear statue";
            item.KeyWords.Add("statue");
            item.KeyWords.Add("bear");

            return item;
        }

        public IItem Fish()
        {
            IItem item = CreateItem<Item>();
            item.ExamineDescription = "The statue shows a fish jumping out of a lake to catch a dragon fly.";
            item.LookDescription = "The statue is make of a sandy colored stone material.";
            item.ShortDescription = "A small statue of a fish.";
            item.SentenceDescription = "fish statue";
            item.KeyWords.Add("statue");
            item.KeyWords.Add("fish");

            return item;
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

            ZoneHelper.ConnectZone(Zone.Rooms[3], Direction.East, 23, 1, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", false, "The door depicts a bear standing on its hind legs."));
            IDoor door = Zone.Rooms[3].East.Door;
            door.Locked = true;
            door.Pickable = false;

            ZoneHelper.ConnectZone(Zone.Rooms[7], Direction.South, 23, 2, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", false, "The door depicts a fish swallowing a man."));
            door = Zone.Rooms[7].South.Door;
            door.Locked = true;
            door.Pickable = false;

            ZoneHelper.ConnectZone(Zone.Rooms[11], Direction.West, 23, 3, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", false, "The door depicts a lion killing an antelope."));
            door = Zone.Rooms[11].West.Door;
            door.Locked = true;
            door.Pickable = false;

            ZoneHelper.ConnectZone(Zone.Rooms[15], Direction.North, 23, 4, new DoorInfo("door", "The door slides open with the sound of stone sliding on stone.", false, "The door depicts a eagle catching a fish."));
            door = Zone.Rooms[15].North.Door;
            door.Locked = true;
            door.Pickable = false;


            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.South, 19, 21);
            ZoneHelper.ConnectZone(Zone.Rooms[16], Direction.South, 19, 22);
            ZoneHelper.ConnectZone(Zone.Rooms[15], Direction.South, 19, 23);
            ZoneHelper.ConnectZone(Zone.Rooms[14], Direction.South, 19, 24);
            ZoneHelper.ConnectZone(Zone.Rooms[13], Direction.South, 19, 25);
        }
    }
}
