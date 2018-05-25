using Objects.Mob;
using System.Collections.Generic;

namespace Objects.Race.Races
{
    public class Angel : BaseRace
    {
        public override List<MobileObject.MobileAttribute> RaceAttributes
        {
            get
            {
                if (raceAttributes == null)
                {
                    raceAttributes = new List<MobileObject.MobileAttribute>();
                    raceAttributes.Add(MobileObject.MobileAttribute.Fly);
                }
                return raceAttributes;
            }
        }
    }
}
