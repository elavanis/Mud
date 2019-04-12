using Objects.Moon.Interface;
using System.Collections.Generic;

namespace Objects.Moon
{
    public class Moons
    {
        private uint lastCalculation = uint.MaxValue;
        private object padLock = new object();

        public List<IMoon> MoonList { get; set; } = new List<IMoon>();

        //private decimal totalMagicalAdjustment = 0;
        //public decimal TotalMagicAdjustment
        //{
        //    get
        //    {
        //        if (lastCalculation == GlobalReference.GlobalValues.TickCounter)
        //        {
        //            return totalMagicalAdjustment;
        //        }
        //        else
        //        {
        //            lock (padLock)
        //            {
        //                if (lastCalculation == GlobalReference.GlobalValues.TickCounter)
        //                {
        //                    return totalMagicalAdjustment;
        //                }
        //                else
        //                {
        //                    totalMagicalAdjustment = 0;

        //                    foreach (IMoon moon in MoonList)
        //                    {
        //                        totalMagicalAdjustment += moon.CurrentMagicModifier;
        //                    }

        //                    lastCalculation = GlobalReference.GlobalValues.TickCounter;
        //                    return totalMagicalAdjustment;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
