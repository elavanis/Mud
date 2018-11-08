using Objects.Global;
using Objects.Global.Stats;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Magic.Enchantment.DefeatbleInfo
{
    public class DefeatInfo : IDefeatInfo
    {
        [ExcludeFromCodeCoverage]
        public int CurrentEnchantmentPoints { get; set; }
        [ExcludeFromCodeCoverage]
        public Stats.Stat MobStat { get; set; }

        /// <summary>
        /// Returns if the enchantment was broken.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool DoesPayerDefeatEnchantment(IMobileObject target)
        {
            int enchantmentRoll = GlobalReference.GlobalValues.Random.Next(CurrentEnchantmentPoints);
            int playerRoll = GlobalReference.GlobalValues.Random.Next(target.GetStatEffective(MobStat));

            if (playerRoll >= enchantmentRoll)
            {
                //player rolled higher and the enchantment is defeated
                return true;
            }
            else
            {
                //player did not defeat the enchantment

                //percent of the max player roll
                double percentRoll = playerRoll * 1.0 / (target.GetStatEffective(MobStat));

                //reduce roll value by percent roll
                int effectiveDeduction = (int)(playerRoll * percentRoll);

                //subtract the effective deduction 
                CurrentEnchantmentPoints -= Math.Max(1, effectiveDeduction); //always reduce by at least 1

                CurrentEnchantmentPoints = Math.Max(0, CurrentEnchantmentPoints); //don't let the value be less than 0 or else errors
                return false;
            }
        }
    }
}