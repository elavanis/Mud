using System;
using System.Collections.Generic;
using System.Text;
using Objects.Room.Interface;
using Objects.Zone.Interface;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewFort : BaseZone, IZoneCode
    {
        public GrandViewFort() : base(24)
        {
        }

        public IZone Generate()
        {
            throw new NotImplementedException();
        }

        #region Rooms

        private IRoom GenerateRoom1()
        {
            IRoom room = CreateRoom();

            room.ExamineDescription = "The stone walls were carved in place from the side of the mountain.  This leads to their strength as it is on solid piece of stone. ";
            room.LookDescription = "The original fort's stone gate still stands strong.";
            room.ShortDescription = "Front Gate";
        }

        #endregion Rooms
    }
}
