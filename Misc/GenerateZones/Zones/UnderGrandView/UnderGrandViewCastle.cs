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

            room.ExamineDescription = "Tapestries of every color adore the bazaar here.  The one on the right might be look good in your house.  Further in the back you can see the seamstresses work.";
            room.LookDescription = "Colorful tapestries and other cloth materials hang on display all around you.";
            room.ShortDescription = "Bazaar";

            return room;
        }
        #endregion Rooms
    }
}
