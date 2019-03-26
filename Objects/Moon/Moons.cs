using Objects.Global;
using Objects.Moon.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Moon
{
    public class Moons
    {
        public List<IMoon> MoonList { get; set; } = new List<IMoon>();

        public decimal TotalMagicAdjustment
        {
            get
            {
                decimal totalAdjustment = 0;



                foreach (IMoon moon in MoonList)
                {
                    totalAdjustment += moon.CurrentMagicModifier;
                }

                return totalAdjustment;
            }
        }
    }
}
