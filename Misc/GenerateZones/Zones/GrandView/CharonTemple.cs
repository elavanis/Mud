using System;
using System.Collections.Generic;
using System.Text;
using Objects.Zone.Interface;

namespace GenerateZones.Zones.GrandView
{
    public class CharonTemple : BaseZone, IZoneCode
    {
        public CharonTemple() : base(20)
        {
        }

        public IZone Generate()
        {
            throw new NotImplementedException();
        }
    }
}
