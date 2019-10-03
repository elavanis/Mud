using System;
using System.Collections.Generic;
using System.Text;
using MiscShared;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.UnderGrandView
{
    public class UnderGrandViewCastle : BaseZone, IZoneCode
    {
        public UnderGrandViewCastle() : base(25)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(UnderGrandViewCastle);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }



        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "Dirt from above still occasionally falls down and one day may fill the tunnel.";
            room.LookDescription = "The tunnel was originally part of an underground sink hole network.";
            room.ShortDescription = "Underground cavern";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "The dirt from above has filled most of this cavern.  Several small tunnels lead off but most are impassable beyond a few feet.";
            room.LookDescription = "A large mound of dirt fills the cavern.";
            room.ShortDescription = "Underground cavern";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "The tunnel to the west appears to be natural or not finished while the tunnel to the east is lined with bricks";
            room.LookDescription = "This point in the tunnel transitions between a rough natural tunnel and a brick lined tunnel with torches.";
            room.ShortDescription = "Underground cavern";

            return room;
        }

        #endregion Rooms

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
        }
    }
}
