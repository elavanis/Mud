using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Effect.Interface;
using Objects.Attribute.Effect;
using System.Reflection;

namespace Objects.Item.Items
{
    public class Enchantery : Item, IEnchantery
    {
        public string EnchantmentFail { get; set; } = "The item begins to glow and then flashes a bright light.  The item is gone and only a charred ring remains.";
        public string EnchantmentSuccess { get; set; } = "The item begins to glow and then flashes a bright light.  The item continues to have a faint glow and hum slightly.";
        public int CostToEnchantLevel1Item { get; set; } = 1000;
        public decimal SuccessRate { get; set; } = -1;

        protected static object padlock = new object();
        protected static List<Type> defenseTypes = null;
        protected List<Type> DefenseTypes
        {
            get
            {
                if (defenseTypes == null)
                {
                    lock (padlock)
                    {
                        if (defenseTypes == null)
                        {
                            List<Type> tempList = new List<Type>();
                            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                tempList.AddRange(
                                    assembly.GetTypes()
                                       .Where(t => t.GetCustomAttributes().Any(a => a is DefenseEffect))
                                       .ToList()
                                   );
                            }

                            defenseTypes = tempList;
                        }
                    }
                }

                return defenseTypes;
            }
        }

        public Enchantery() : base()
        {
            KeyWords.Add("Enchantery");
        }

        public virtual IResult Enchant(IItem item)
        {
            IResult result = null;

            if (Success(item))
            {
                IEnchantment randomEnchantment = MakeRandomEnchantment(item);

                if (randomEnchantment != null)
                {
                    item.Enchantments.Add(randomEnchantment);
                    result = new Result(EnchantmentSuccess, false);
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = new Result(EnchantmentFail, false);
            }

            return result;
        }

        public virtual bool Success(IItem item)
        {
            return GlobalReference.GlobalValues.Random.PercentDiceRoll(CaluclateSuccessRate(item));
        }

        protected virtual int CaluclateSuccessRate(IItem item)
        {
            int successRate = (int)(Math.Pow(item.Enchantments.Count + 1, -1) * 100);
            return successRate;
        }

        protected IEnchantment MakeRandomEnchantment(IItem item)
        {
            IEnchantment enchantment = null;

            if (item as IWeapon != null)
            {
                enchantment = EnchantWeapon(item);
            }
            else if (item as IArmor != null)
            {
                enchantment = EnchantArmor(item);
            }

            if (enchantment != null)
            {
                enchantment.ActivationPercent = GlobalReference.GlobalValues.Random.Next(100) + 1;
            }

            return enchantment;
        }

        protected IEnchantment EnchantArmor(IItem item)
        {
            IEnchantment enchantment = null;
            IArmor armor = item as IArmor;
            if (armor != null)
            {
                switch (GlobalReference.GlobalValues.Random.Next(2))
                {
                    case 0:
                        enchantment = new DamageReceivedBeforeDefenseEnchantment();
                        enchantment.Effect = GetRandomDefenseEffect();
                        enchantment.Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(item.Level);
                        break;
                    case 1:
                        enchantment = new DamageReceivedAfterDefenseEnchantment();
                        enchantment.Effect = new Effect.Damage();
                        Damage.Damage damage = new Damage.Damage();
                        damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(item.Level);
                        damage.Type = GetRandomDamageType();
                        break;
                }
            }

            return enchantment;
        }

        protected IEffect GetRandomDefenseEffect()
        {
            Type type = DefenseTypes[GlobalReference.GlobalValues.Random.Next(DefenseTypes.Count)];

            return (IEffect)Activator.CreateInstance(type);
        }

        protected IEnchantment EnchantWeapon(IItem item)
        {
            IEnchantment enchantment = new DamageDealtBeforeDefenseEnchantment();
            enchantment.Effect = new Effect.Damage();
            Damage.Damage damage = new Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(item.Level);
            damage.Type = GetRandomDamageType();

            return enchantment;
        }

        protected static Damage.Damage.DamageType GetRandomDamageType()
        {
            List<Damage.Damage.DamageType> damages = Enum.GetValues(typeof(Damage.Damage.DamageType)).Cast<Damage.Damage.DamageType>().ToList();
            return damages[GlobalReference.GlobalValues.Random.Next(damages.Count)];
        }
    }
}
