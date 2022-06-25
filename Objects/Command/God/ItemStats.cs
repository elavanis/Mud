using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Item;

namespace Objects.Command.God
{
    public class ItemStats : BaseMobileObjectComand, IMobileObjectCommand
    {
        public ItemStats() : base(nameof(ItemStats), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("ItemStats [Item Keyword]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "ItemStats" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count >= 1)
            {
                IParameter parm = command.Parameters[0];
                IItem item = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, parm.ParameterValue, parm.ParameterNumber);

                if (item != null)
                {
                    return Identify(item);
                }
                else
                {
                    string message = string.Format("You do not seem to be holding {0}.", command.Parameters[0].ParameterValue);
                    if (parm.ParameterNumber > 1)
                    {
                        message = message.Substring(0, message.Length - 1);
                        message += "[" + parm.ParameterNumber + "]";
                        message += ".";
                    }
                    return new Result(message, true);
                }
            }
            else
            {
                return new Result("What item would you like to get stats on?", true);
            }
        }

        private IResult Identify(IItem item)
        {
            StringBuilder strBldr = new StringBuilder();

            strBldr.AppendLine(item.SentenceDescription);
            strBldr.AppendLine("Item Type: " + item.GetType().Name);
            strBldr.AppendLine("Zone: " + item.ZoneId);
            strBldr.AppendLine("Id: " + item.Id);
            strBldr.AppendLine("Level: " + item.Level);

            #region Keywords
            StringBuilder shortStringBuilder = new StringBuilder();
            shortStringBuilder.Append("Keywords: ");
            foreach (string keyword in item.KeyWords)
            {
                shortStringBuilder.AppendLine(keyword);
            }

            strBldr.AppendLine(shortStringBuilder.ToString().Trim());
            #endregion Keywords

            strBldr.AppendLine("SentenceDescription: " + item.SentenceDescription);
            strBldr.AppendLine("ShortDescription: " + item.ShortDescription);
            strBldr.AppendLine("LookDescription: " + item.LookDescription);
            strBldr.AppendLine("ExamineDescription: " + item.ExamineDescription);

            strBldr.AppendLine("Weight :" + item.Weight);

            strBldr.Append(GetArmorInfo(item));
            strBldr.Append(GetWeaponInfo(item));

            return new Result(strBldr.ToString().Trim(), false);
        }

        private string GetArmorInfo(IItem item)
        {
            if (item is IArmor armor)
            {
                StringBuilder strBldr = new StringBuilder();

                if (item is IShield shield)
                {
                    strBldr.AppendLine("ShieldNegateDamagePercent: " + shield.NegateDamagePercent);
                }

                strBldr.AppendLine(DiceInfo(armor.Dice));

                foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
                {
                    strBldr.AppendLine(damageType.ToString() + ": " + armor.GetTypeModifier(damageType));
                }

                strBldr.Append(EquipmentInfo(armor));


                return strBldr.ToString();
            }
            return "";
        }

        private string GetWeaponInfo(IItem item)
        {
            if (item is IWeapon weapon)
            {
                StringBuilder strBldr = new StringBuilder();
                foreach (IDamage damage in weapon.DamageList)
                {
                    strBldr.AppendLine("DamageType: " + damage.Type);
                    strBldr.AppendLine(DiceInfo(damage.Dice));
                    if (damage.BonusDamageStat != null)
                    {
                        strBldr.AppendLine("BonusDamageStat: " + damage.BonusDamageStat);
                    }

                    if (damage.BonusDefenseStat != null)
                    {
                        strBldr.AppendLine("BonusDefenseStat: " + damage.BonusDefenseStat);
                    }
                }

                strBldr.Append(EquipmentInfo(weapon));

                return strBldr.ToString();
            }
            return "";
        }

        private string EquipmentInfo(IEquipment equipment)
        {
            StringBuilder strBldr = new StringBuilder();
            if (equipment.Strength != 0)
            {
                strBldr.AppendLine("Strength: " + equipment.Strength);
            }

            if (equipment.Dexterity != 0)
            {
                strBldr.AppendLine("Dexterity: " + equipment.Dexterity);
            }

            if (equipment.Constitution != 0)
            {
                strBldr.AppendLine("Constitution: " + equipment.Constitution);
            }

            if (equipment.Intelligence != 0)
            {
                strBldr.AppendLine("Intelligence: " + equipment.Intelligence);
            }

            if (equipment.Wisdom != 0)
            {
                strBldr.AppendLine("Wisdom: " + equipment.Wisdom);
            }

            if (equipment.Charisma != 0)
            {
                strBldr.AppendLine("Charisma: " + equipment.Charisma);
            }

            if (equipment.MaxHealth != 0)
            {
                strBldr.AppendLine("MaxHealth: " + equipment.MaxHealth);
            }

            if (equipment.MaxMana != 0)
            {
                strBldr.AppendLine("MaxMana: " + equipment.MaxMana);
            }

            if (equipment.MaxStamina != 0)
            {
                strBldr.AppendLine("MaxStamina: " + equipment.MaxStamina);
            }

            strBldr.AppendLine("ItemPosition: " + equipment.ItemPosition);

            StringBuilder localBuilder = new StringBuilder();
            localBuilder.Append("Attributes: ");
            foreach (ItemAttribute attribute in equipment.Attributes)
            {
                localBuilder.AppendLine(attribute.ToString());
            }
            if (localBuilder.ToString() != "Attributes: ")
            {
                strBldr.AppendLine(localBuilder.ToString().Trim());
            }


            localBuilder = new StringBuilder();
            localBuilder.Append("Enchantments: ");
            foreach (IEnchantment enchantment in equipment.Enchantments)
            {
                localBuilder.AppendLine(enchantment.ToString());
            }
            if (localBuilder.ToString() != "Enchantments: ")
            {
                strBldr.AppendLine(localBuilder.ToString().Trim());
            }

            return strBldr.ToString();
        }

        private string DiceInfo(IDice dice)
        {
            return "Die: " + dice.Die + Environment.NewLine
                    + "Sides: " + dice.Sides;
        }
    }
}
