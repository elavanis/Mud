using Objects.Global.MultiClassBonus.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Global.MultiClassBonus
{
    public class MultiClassBonus : IMultiClassBonus
    {
        private List<double> _levelPercent = null;
        private List<double> LevelPercent
        {
            get
            {
                if (_levelPercent == null)
                {
                    _levelPercent = CalculateLevelPercent();
                }
                return _levelPercent;
            }
        }


        private List<double> CalculateLevelPercent()
        {
            List<double> bonusPercent = new List<double>();

            double perecent = 1;
            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxLevel; i++)
            {
                bonusPercent.Add(perecent);
                perecent = perecent / GlobalReference.GlobalValues.Settings.Multiplier;
            }

            bonusPercent.Add(0);  //to offset the 0 index
            bonusPercent.Sort();

            return bonusPercent;
        }

        public int CalculateBonus(int level, int bonusValue)
        {
            return (int)Math.Round(LevelPercent[level] * bonusValue, 0);
        }
    }
}
