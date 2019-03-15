using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using static Objects.Mob.NonPlayerCharacter;

namespace Objects.Global.Random
{
    public class Random : IRandom
    {
        [ExcludeFromCodeCoverage]
        private static System.Random _random { get; set; } = new System.Random();

        //No reason to test randoms next methods
        [ExcludeFromCodeCoverage]
        public int Next(int maxValue)
        {
            lock (_random)
            {
                return _random.Next(maxValue);
            }
        }

        [ExcludeFromCodeCoverage]
        public int Next(int minValue, int maxValue)
        {
            lock (_random)
            {
                return _random.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// Takes a percentage and rolls a dice to see
        /// if the roll is less than/equal to the percent.  
        /// If so it returns true.
        /// IE 20 returns true 20% of the time and
        /// 100 returns true 100% of the time.
        /// </summary>
        /// <param name="percentSuccessful"></param>
        /// <returns></returns>
        public bool PercentDiceRoll(double percentSuccessful)
        {
            lock (_random)
            {
                if (_random.NextDouble() <= percentSuccessful / 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
