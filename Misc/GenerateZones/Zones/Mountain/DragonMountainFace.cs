using System.Linq;
using System.Reflection;
using MiscShared;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Room.Room;

namespace GenerateZones.Zones.Mountain
{
    public class DragonMountainFace : BaseZone, IZoneCode
    {
        public DragonMountainFace() : base(19)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(DragonMountainFace);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Mountain Face
        #region Level 1
        private IRoom GenerateRoom1()
        {
            string examineDescription = "A rock face looks craggy and has plenty of hand holds.";
            string lookDescription = "You stand at the base of the mountain side.  The ground is rocky here with little vegetation growing.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The path up appears to be clear and able to be climbed.";
            string lookDescription = "You stand at the base of the mountain side.  A few small clumps of grass grow in cracks in the rocky soil.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "Several hand holds have been marked on the rock face showing a way up the mountain side.";
            string lookDescription = "You stand at the base of the mountain side.  A small pool of water sits in a small divot in an otherwise smooth rock ground.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The rock face seems to smooth out the higher up you go.";
            string lookDescription = "You stand at the base of the mountain side.  The ground is made of soil and has several sets of goat prints here.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "A few feet above you the rock face move back into the mountain making it hard to see a path up.";
            string lookDescription = "You stand at the base of the mountain side.  There is a long path leading up the mountain to the east and a short solid rock wall going straight up.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }
        #endregion Level 1

        #region Level 2
        private IRoom GenerateRoom6()
        {
            string examineDescription = "There are scratches on the rock face indicating people have climbed on this rock face before you.";
            string lookDescription = "The wall rock face is craggy and easy to climb.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "A small green bush protrudes from the rock ledge.";
            string lookDescription = "A small ledge makes standing here much easier.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "The hand holds continue on a path straight up.";
            string lookDescription = "A tree branch brushes up against the rock face as if saying hello there.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "The rock face is getting very hard to find hand holds.";
            string lookDescription = "The rock face is harder to climb with less hand holds.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "A small birds nest sits on a small ledge to a few feet to your right.";
            string lookDescription = "The rock wall is smooth like water has flowed over it but is still craggy with plenty of holds.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }
        #endregion Level 2

        #region Level 3
        private IRoom GenerateRoom11()
        {
            string examineDescription = "A small crack quickly grows into a crack large enough to crawl into.";
            string lookDescription = "The rock face is starting to get craggy and brittle.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "The soft stone has been worn away in spots and polished smooth.";
            string lookDescription = "The rock face has become loose crumbly stone that breaks when weight it put on it.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "The rock face contains a vein of sparkling stones while pretty would be to hard to get while dangling from the rock face.";
            string lookDescription = "The rock face looks to have broken off here reveling the innards of the mountain.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "A small birds nest gently sits on a branch growing out of the rock face.  Be careful if you use the branch as a hand hold.";
            string lookDescription = "The hand holds are getting thin except a few branches growing out of the rock face.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "The stone resembles a eye of a dragon.  This is what gives this mountain its name. Dragon Mountain.";
            string lookDescription = "There is a large round stone that some how is carved out of the mountain side.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }
        #endregion Level 3

        #region Level 4
        private IRoom GenerateRoom16()
        {
            string examineDescription = "The rock face is spotted with little hole as if a great volcanic blast blew holes in the mountain side.";
            string lookDescription = "The wall here is a golden brown color.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "A great divot has been worn away by the wind in the face of the mountain.";
            string lookDescription = "The soft rock has not stood the test of time and worn away.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom18()
        {
            string examineDescription = "The stone has sparkles slightly as if its made of granite.";
            string lookDescription = "The rock face looks like it a cool gray color.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "The rock face looks like it has a grid pattern almost as if it was made of scales.";
            string lookDescription = "The rock here has a grid pattern as if it was once made of something else.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom20()
        {
            string examineDescription = "The stone has given way to a white chalky substance that crumbles in your hand.";
            string lookDescription = "The rock has become a white chalky substance making it easy to climb but dangerous to stay.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }
        #endregion Level 4

        #region Level 5
        private IRoom GenerateRoom21()
        {
            string examineDescription = "The top of the rock mountain ledge is covered in green grass.";
            string lookDescription = "The sheer rock face is looks difficult to climb.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "Unfortunately the ropes seem quite frail and do not look to be able to support weight.";
            string lookDescription = "Several ropes have been hung over the side of the ledge allowing one to climb down the cliff.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "It is hard to tell if the depression is natural or man made but the stones were put there by someone for sure.";
            string lookDescription = "The earth has been shaped into a large bowl area with rocks forming a perimeter.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "The remnants of what appear to be torches can be seen scattered around.";
            string lookDescription = "Here the ground has been burned by fire.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom25()
        {
            string examineDescription = "Large rocks five feet in diameter are scattered around for what appears like no good reason.";
            string lookDescription = "The terrain and interesting mix of large rocks and small patches of grass.";
            IRoom room = RockFace(examineDescription, lookDescription);

            return room;
        }
        #endregion Level 5
        #endregion Mountain Face

        #region Path
        private IRoom GenerateRoom26()
        {
            string examineDescription = "A small path leads up the side of the mountain.";
            string lookDescription = "Several small flowers grow along side of the path here.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom27()
        {
            string examineDescription = "A small path leads up the side of the mountain.";
            string lookDescription = "A small set of stones is piled on the side of the path.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom28()
        {
            string examineDescription = "The stone is worn smooth and looks like it would be very slick when wet.";
            string lookDescription = "The path disappears as the path transitions between grass and solid stone.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom29()
        {
            string examineDescription = "The stone is hard and smooth and make the path impossible to see.";
            string lookDescription = "Solid stone makes the path impossible to see the path continues straight on along the side of the mountain.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom30()
        {
            string examineDescription = "Where the stone has turned to grass a well defined path is visible.";
            string lookDescription = "The path transitions between grass and solid stone.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom31()
        {
            string examineDescription = "The white daisies spatter the green grass.";
            string lookDescription = "Daisies line the path like a people watching a parade.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom32()
        {
            string examineDescription = "Each bead is about half an inch long and oval in shape.";
            string lookDescription = "The path is covered in shiny red beads.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom33()
        {
            string examineDescription = "The trees massive green leaves give so shelter from the elements for travelers wishing to stop along the path.";
            string lookDescription = "A small tree grows defiantly upwards as the soil it has grown in slowly slips downward.";
            IRoom room = RockPath(examineDescription, lookDescription);

            return room;
        }

        private IRoom GenerateRoom34()
        {
            return GenerateRoom32();
        }

        private IRoom GenerateRoom35()
        {
            return GenerateRoom31();
        }

        private IRoom GenerateRoom36()
        {
            return GenerateRoom30();
        }

        private IRoom GenerateRoom37()
        {
            return GenerateRoom29();
        }

        private IRoom GenerateRoom38()
        {
            return GenerateRoom28();
        }

        private IRoom GenerateRoom39()
        {
            return GenerateRoom27();
        }

        private IRoom GenerateRoom40()
        {
            return GenerateRoom26();
        }
        #endregion Path

        private IRoom RockFace(string examineDescription, string lookDescription)
        {
            string shortDescription = "Mountain Face";
            IRoom room = new Room(examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom RockPath(string examineDescription, string lookDescription)
        {
            string shortDescription = "Path up the side of the mountain";
            IRoom room = new Room(examineDescription, lookDescription, shortDescription);
            return room;
        }

        private void ConnectRooms()
        {
            #region Rock Face
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);

            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[10]);

            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.East, Zone.Rooms[15]);

            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.East, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.East, Zone.Rooms[20]);

            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.East, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.East, Zone.Rooms[40]);


            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.Up, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.Up, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.Up, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.Up, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.Up, Zone.Rooms[10]);

            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.Up, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.Up, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.Up, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.Up, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.Up, Zone.Rooms[15]);

            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.Up, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.Up, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.Up, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.Up, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.Up, Zone.Rooms[20]);

            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.Up, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.Up, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.Up, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.Up, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.Up, Zone.Rooms[25]);


            ZoneHelper.ConnectZone(Zone.Rooms[21], Direction.North, 22, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[22], Direction.North, 22, 16);
            ZoneHelper.ConnectZone(Zone.Rooms[23], Direction.North, 22, 15);
            ZoneHelper.ConnectZone(Zone.Rooms[24], Direction.North, 22, 14);
            ZoneHelper.ConnectZone(Zone.Rooms[25], Direction.North, 22, 13);

            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 16, 15);
            #endregion Rock Face

            #region Path
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.Up, Zone.Rooms[29]);

            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.West, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.West, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.West, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.Up, Zone.Rooms[33]);

            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.East, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.East, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.East, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.Up, Zone.Rooms[37]);

            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.West, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.West, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.West, Zone.Rooms[40]);
            #endregion Path
        }
    }
}