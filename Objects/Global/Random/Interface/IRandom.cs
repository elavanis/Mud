namespace Objects.Global.Random.Interface
{
    public interface IRandom
    {
        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        long Next(long maxValue);
        long Next(long minValue, long maxValue);

        ///// <summary>
        ///// Takes a percentage and rolls a dice to see
        ///// if the roll is less than/equal to the percent.  
        ///// If so it returns true.
        ///// IE 20 returns true 20% of the time and
        ///// 100 returns true 100% of the time.
        ///// </summary>
        ///// <param name="percentSuccessful"></param>
        ///// <returns></returns>
        //bool PercentDiceRoll(int percentSuccessful);

        /// <summary>
        /// Takes a percentage and rolls a dice to see
        /// if the roll is less than/equal to the percent.  
        /// If so it returns true.
        /// IE 20 returns true 20% of the time and
        /// 100 returns true 100% of the time.
        /// </summary>
        /// <param name="percentSuccessful"></param>
        /// <returns></returns>
        bool PercentDiceRoll(double percentSuccessful);
    }
}