using Objects.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob.SpecificNPC
{
    public class WaterElemental : NonPlayerCharacter
    {
        public WaterElemental()
        {
            KeyWords.Add("elemental");
            KeyWords.Add("water");
            LookDescription = "A water elemental flows around leaving a trail of water behind.";
            ExamineDescription = "The innards of the water elemental churn and flow distorting the image of what is behind it.";
            ShortDescription = "A turbulent water elemental.";
            SentenceDescription = "water elemental";

            //GlobalReference.GlobalValues.World.WindSpeed
            //GlobalReference.GlobalValues.World.Precipitation

        }
    }
}
