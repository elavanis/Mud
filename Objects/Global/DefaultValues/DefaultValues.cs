using Objects.Die;
using Objects.Die.Interface;
using Objects.Global.DefaultValues.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Global.DefaultValues
{
    public class DefaultValues : IDefaultValues
    {
        private object padLock = new object();

        #region Armor
        private List<IDice> armorValues = null;

        public IDice DiceForArmorLevel(int level)
        {
            if (armorValues == null)
            {
                lock (padLock)
                {
                    if (armorValues == null)
                    {
                        armorValues = GenerateValuesArmor();
                    }
                }
            }

            return armorValues[level - 1];
        }

        private List<IDice> GenerateValuesArmor()
        {
            List<IDice> armorValues = new List<IDice>();
            double target = 1;
            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxCalculationLevel; i++)
            {
                //calculate the number of sides the armor die should have based upon the target damage stopping power
                int sides = (int)Math.Round(target * 2, 0, MidpointRounding.AwayFromZero) - 1;

                //add that entry into the values list
                armorValues.Add(ReduceValues(1, sides));

                //increment the target by 10%
                target *= GlobalReference.GlobalValues.Settings.Multiplier;
            }
            return armorValues;
        }
        #endregion Armor

        #region Weapon
        private List<IDice> weaponValues = null;

        public IDice DiceForWeaponLevel(int level)
        {
            if (weaponValues == null)
            {
                lock (padLock)
                {
                    if (weaponValues == null)
                    {
                        weaponValues = GenerateValuesWeapon();
                    }
                }
            }

            return weaponValues[level - 1];
        }

        private List<IDice> GenerateValuesWeapon()
        {
            List<IDice> weaponValues = new List<IDice>();
            //we multiply by 9 because that is what a fully equipped player can wear. 
            //we multiply by 3 because it is 50% more damage and we need the sides to be 2x higher to make the average equal our target 
            IDice dice = DiceForArmorLevel(1);
            int target = dice.Die * dice.Sides * 9 * 3;

            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxCalculationLevel; i++)
            {
                weaponValues.Add(ReduceValues(1, target));

                target = (int)Math.Round(target * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            }
            return weaponValues;
        }
        #endregion Weapon

        #region Trap
        private List<int> trapValues = null;

        public IDice DiceForTrapLevel(int level, int percentOfAveragePcHealth = 100)
        {
            if (trapValues == null)
            {
                lock (padLock)
                {
                    if (trapValues == null)
                    {
                        trapValues = GenerateValuesTrap();
                    }
                }
            }

            int damageValue = (int)(trapValues[level - 1] * (percentOfAveragePcHealth / 100.0));
            return ReduceValues(1, damageValue);
        }

        private List<int> GenerateValuesTrap()
        {
            List<int> trapValues = new List<int>();
            int damageAmount = (GlobalReference.GlobalValues.Settings.BaseStatValue + GlobalReference.GlobalValues.Settings.AssignableStatPoints / 6) * 10;
            double incrementingValue = (GlobalReference.GlobalValues.Settings.Multiplier - 1) / 6 + GlobalReference.GlobalValues.Settings.Multiplier;
            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxCalculationLevel; i++)
            {
                //we multiply the damage by 2 to get the average damage to be the full health of a player
                trapValues.Add(2 * damageAmount);

                damageAmount = (int)Math.Round(damageAmount * incrementingValue, 0);
            }
            return trapValues;
        }
        #endregion Trap

        #region Spell/Skill
        private List<IDice> abilityValues = null;

        public IDice DiceForSpellLevel(int level)
        {
            if (abilityValues == null)
            {
                lock (padLock)
                {
                    if (abilityValues == null)
                    {
                        abilityValues = GenerateValuesAbility();
                    }
                }
            }

            return abilityValues[level - 1];
        }

        public IDice DiceForSkillLevel(int level)
        {
            return DiceForSpellLevel(level);
        }

        private List<IDice> GenerateValuesAbility()
        {
            List<IDice> weaponValues = new List<IDice>();
            //we multiply by 9 because that is what a fully equipped player can wear. 
            //we multiply by 3 because it is 50% more damage and we need the sides to be 2x higher to make the average equal our target 
            //we multiply by 1.5 because it is skill and is only usable with mana/stamina and thus will run out
            IDice dice = DiceForArmorLevel(1);
            int target = (int)(dice.Die * dice.Sides * 9 * 3 * 1.5);

            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxCalculationLevel; i++)
            {
                weaponValues.Add(ReduceValues(1, target));

                target = (int)Math.Round(target * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            }
            return weaponValues;
        }
        #endregion Spell/Skill

        public IDice ReduceValues(int die, int sides)
        {
            int reducer = 2;
            int sqrtSides = (int)Math.Sqrt(sides);
            while (reducer <= sqrtSides)
            {
                if (sides % reducer == 0)
                {
                    die *= reducer;
                    sides /= reducer;
                    sqrtSides = (int)Math.Sqrt(sides);
                    reducer = 2;
                }
                else
                {
                    reducer++;
                }
            }

            return new Dice(die, sides);
        }

        #region Money
        private List<uint> moneyValues = null;

        public DefaultValues()
        {
        }

        public uint MoneyForNpcLevel(int level)
        {
            if (moneyValues == null)
            {
                lock (padLock)
                {
                    if (moneyValues == null)
                    {
                        moneyValues = GenerateMoneyValues();
                    }
                }
            }

            if (level <= 0)
            {
                return 0;
            }

            return moneyValues[level - 1];
        }

        private List<uint> GenerateMoneyValues()
        {
            List<uint> moneyValues = new List<uint>();
            uint target = 100;
            for (int i = 0; i < GlobalReference.GlobalValues.Settings.MaxCalculationLevel; i++)
            {
                moneyValues.Add(target);
                target = (uint)Math.Round(target * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            }
            return moneyValues;
        }
        #endregion Money
    }
}
