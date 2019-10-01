using System;
using System.Collections.Generic;
using System.Text;
using Objects.Room.Interface;
using Objects.Zone.Interface;

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

            room.ExamineDescription = "Dirt from above still occasionally falls down and one day may fill the tunnel.";
            room.LookDescription = "The tunnel was originally part of an underground sink hole network.  ";
            room.ShortDescription = "Underground cavern";

            return room;
        }

        #endregion Rooms
    }
}
