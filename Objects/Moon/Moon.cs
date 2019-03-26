using Objects.Global;
using Objects.Moon.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Moon
{
    public class Moon : IMoon
    {
        [ExcludeFromCodeCoverage]
        public string Name { get; set; }

        private decimal magicModifier;
        //public decimal MagicModifier
        //{
        //    get
        //    {
        //        return magicModifier;
        //    }
        //    set
        //    {
        //        magicModifier = value;

        //        if (magicModifier != 1)
        //        {
        //            MagicModifierMultiplier = 50 / (magicModifier - 1);
        //        }
        //    }
        //}

        private int moonPhaseCycleDays;
        public int MoonPhaseCycleDays
        {
            get
            {
                return moonPhaseCycleDays;
            }
            set
            {
                moonPhaseCycleDays = value;
                DailyRateOfChange = 200M / moonPhaseCycleDays;
            }
        }


        //public decimal CurrentMagicModifier
        //{
        //    get
        //    {
        //        if (magicModifier == 1)
        //        {
        //            return 1;  //moon does not affect magic
        //        }

        //        decimal percentFull = Math.Abs(MoonPhasePositionThroughCycle);

        //        decimal moonMathCalc = percentFull - 50;

        //        if (moonMathCalc >= 0)
        //        {
        //            return moonMathCalc / MagicModifierMultiplier + 1;
        //        }
        //        else
        //        {
        //            moonMathCalc = Math.Abs(moonMathCalc);
        //            decimal divisor = moonMathCalc / MagicModifierMultiplier + 1;
        //            return 1 / divisor;
        //        }
        //    }
        //}

        private decimal DailyRateOfChange;
        private decimal MagicModifierMultiplier;

        private decimal MoonPhasePositionThroughCycle
        {
            get
            {
                decimal daysPast = GlobalReference.GlobalValues.GameDateTime.GameDateTime.TotalDaysPastBegining;
                decimal daysPastPhase = daysPast % MoonPhaseCycleDays;
                decimal phase = 100 - daysPastPhase * DailyRateOfChange;

                return phase;
            }
        }
    }
}
