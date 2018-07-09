using Objects.Global.Exp.Interface;
using System;

namespace Objects.Global.Exp
{
    public class Experience : IExperience
    {
        private int _npcMultiplier = 33;

        private int[] _npcExpValues = null;

        /// <summary>
        /// Experence for killing an npc for level x
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDefaultNpcExpForLevel(int level)
        {
            if (_npcExpValues == null)
            {
                _npcExpValues = GenerateNpcExpValues();
            }

            //fix for default level 0 mobs
            if (level == 0)
            {
                return 0;
            }

            return _npcExpValues[level - 1];
        }

        private int[] GenerateNpcExpValues()
        {
            int[] npcExpValue = new int[GlobalReference.GlobalValues.Settings.MaxCalculationLevel];
            int currentExp = 100;
            for (int i = 0; i < npcExpValue.Length; i++)
            {
                npcExpValue[i] = currentExp;
                currentExp = (int)Math.Round(currentExp * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            }
            return npcExpValue;
        }


        private int[] _expValues = null;

        /// <summary>
        /// Experience you need to get to level x
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetExpForLevel(int level)
        {
            if (level <= GlobalReference.GlobalValues.Settings.MaxLevel)
            {
                if (_expValues == null)
                {
                    _expValues = GenerateExpValues();
                }

                return _expValues[level - 1];
            }
            else
            {
                return int.MaxValue;
            }
        }

        private int[] GenerateExpValues()
        {
            int[] expValue = new int[GlobalReference.GlobalValues.Settings.MaxLevel];
            for (int i = 0; i < expValue.Length; i++)
            {
                expValue[i] = (int)(Math.Round(Math.Log(i + 2, 10) * _npcMultiplier, 0, MidpointRounding.AwayFromZero) * GetDefaultNpcExpForLevel(i + 1));
            }

            int lastExpValue = 0;
            int[] runningExpValue = new int[GlobalReference.GlobalValues.Settings.MaxLevel];
            for (int i = 0; i < expValue.Length; i++)
            {
                runningExpValue[i] = lastExpValue + expValue[i];
                lastExpValue = runningExpValue[i];
            }

            return runningExpValue;
        }
    }
}
