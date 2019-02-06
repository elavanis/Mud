using System;
using System.Collections.Generic;
using System.Text;
using Objects.Zone.Interface;

namespace GenerateZones.Zones.Mountain
{
    public class WizardTower : BaseZone, IZoneCode
    {
        public WizardTower() : base(23)
        {

        }

        public IZone Generate()
        {
            throw new NotImplementedException();
        }
    }
}
