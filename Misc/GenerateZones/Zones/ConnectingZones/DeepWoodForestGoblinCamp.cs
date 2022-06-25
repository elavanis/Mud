using MiscShared;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System.Linq;
using System.Reflection;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.ConnectingZones
{
    public class DeepWoodForestGoblinCamp : BaseZone, IZoneCode
    {

        public DeepWoodForestGoblinCamp() : base(17)
        {
        }


        #region Rooms
        IZone IZoneCode.Generate()
        {
            Zone.InGameDaysTillReset = 5;
            Zone.Name = nameof(DeepWoodForestGoblinCamp);

            BuildRoomsViaReflection(this.GetType());

            //AddMobs();

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.North, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.West, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.West, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.North, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);

            ZoneHelper.ConnectZone(Zone.Rooms[11], Direction.East, 16, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 8, 80);
        }


        private IRoom GenerateRoom1()
        {
            string examineDescription = "The ravine is narrow enough to block out a lot of light making it seem darker than it really is.";
            string lookDescription = "Steep rock walls tower above you on both sides.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The pass narrows and makes a series of zig zags making it impossible to see forward or back.";
            string lookDescription = "The walls are jagged enough to climb twenty or thirty feet but then the next sixty or seventy is smooth making it impossible to climb.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The path through the ravine seems surprisingly barren of plant life.";
            string lookDescription = "The ravine narrows enough that you have to turn sideways to continue on.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The path to the East rises slightly before turning out of sight.  The path to the West descends slightly.  The path to the South also rises slightly.";
            string lookDescription = "Here the path splits forming a T.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "The markings are several round Os made in some kind of red paint.";
            string lookDescription = "There are several markings on the ravine wall.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "The cave seems to emanate a foul earthy smell.";
            string lookDescription = "Several bones lie scattered on the ground around the entrance to a cave.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "There is a half eaten goat in one corner along with several other unidentifiable animals.";
            string lookDescription = "The cave has a large pile of straw in one corner.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "The bottom of the ravine is covered in dirt from above.";
            string lookDescription = "Here the path looks less traveled.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "The bottom of the ravine is covered in dirt from above.";
            string lookDescription = "Here the path looks less traveled.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "More dirt has fallen down from above.  As if something is causing the dirt to fall.";
            string lookDescription = "A small shrub grows out of a small rock cropping.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "A few small foot prints can be seen in the dirt but it hard to tell what made them or how long ago.";
            string lookDescription = "A mound of dirt has been built up to the point it causes a natural stairs out of the ravine.";
            string shortDescription = "In a ravine";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 100);

            return room;
        }
        #endregion Rooms
    }
}
