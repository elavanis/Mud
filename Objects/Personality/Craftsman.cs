using Objects.Command;
using Objects.Command.Interface;
using Objects.Crafting;
using Objects.Crafting.Interface;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.GameDateTime.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;

namespace Objects.Personality
{
    public class Craftsman : ICraftsman
    {
        [ExcludeFromCodeCoverage]
        public double SellToPcIncrease { get; set; } = 10;

        public IResult Build(INonPlayerCharacter craftsman, IPlayerCharacter performer, AvalableItemPosition position, int level, string keyword, string sentenceDescription, string shortDescription, string lookDescription, string examineDescription, DamageType damageType = DamageType.Slash)
        {
            IResult result;
            IItem item = null;

            switch (position)
            {
                case Equipment.AvalableItemPosition.Held:
                    IEquipment equipment = new Equipment(position, examineDescription, lookDescription, sentenceDescription, shortDescription);
                    result = BuildItem(craftsman, performer, position, level, keyword, equipment);
                    item = equipment;
                    break;
                case Equipment.AvalableItemPosition.Wield:
                    IWeapon weapon = new Weapon();
                    result = BuildItem(craftsman, performer, position, level, keyword, weapon);
                    IDamage damage = new Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level));
                    damage.Type = damageType;
                    weapon.DamageList.Add(damage);
                    item = weapon;
                    break;
                case Equipment.AvalableItemPosition.NotWorn:
                    craftsman.EnqueueCommand($"Tell {performer.KeyWords[0]} I can not build that.");
                    result = new Result("", true);
                    break;
                default:
                    IDice dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level);
                    IArmor armor = new Armor(dice, position, examineDescription, lookDescription, sentenceDescription, shortDescription);
                    result = BuildItem(craftsman, performer, position, level, keyword, armor);
                    item = armor;
                    break;
            }

            if (!result.AllowAnotherCommand)
            {
                ICraftsmanObject craftsmanObject = new CraftsmanObject();
                craftsmanObject.CraftsmanId = new BaseObjectId(craftsman);
                craftsmanObject.CraftmanDescripition = craftsman.ShortDescription;
                craftsmanObject.Completion = DateTime.Now.AddMinutes(item.Level);  //make it take 1 hour game for each level
                craftsmanObject.Item = item;
                performer.CraftsmanObjects.Add(craftsmanObject);
            }

            return result;
        }

        private IResult BuildItem(INonPlayerCharacter craftsman, IMobileObject performer, Equipment.AvalableItemPosition position, int level, string keyword, IEquipment equipment)
        {
            IResult result = null;
            result = CheckMoney(craftsman, performer, level, equipment);
            if (result != null)
            {
                return result;
            }

            equipment.ItemPosition = position;
            equipment.KeyWords.Add(keyword);
            equipment.FinishLoad();

            DateTime completionDate = DateTime.UtcNow.AddMinutes(equipment.Level);  //make it take 1 hour game for each level
            IGameDateTime gameDateTime = GlobalReference.GlobalValues.GameDateTime.GetDateTime(completionDate);
            craftsman.EnqueueCommand($"Tell {performer.KeyWords[0]} I will have this finished for you on {gameDateTime}.");


            result = new Result("", false);

            return result;
        }

        private IResult CheckMoney(INonPlayerCharacter craftsman, IMobileObject performer, int level, IItem item)
        {
            item.Level = level;
            item.FinishLoad();
            ulong sellPrice = SellPrice(craftsman, performer, item);
            if (sellPrice > performer.Money)
            {
                string money = GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(sellPrice);
                return new Result($"You need {money} to have the item made for you.", true);
            }
            else
            {
                performer.Money -= sellPrice;
            }

            return null;
        }

        private ulong SellPrice(INonPlayerCharacter craftsman, IMobileObject performer, IItem item)
        {
            return (ulong)(item.Value * SellToPcIncrease * CharismaEffect(craftsman, performer));
        }

        private double CharismaEffect(INonPlayerCharacter merchant, IMobileObject performer)
        {
            return ((double)merchant.CharismaEffective) / performer.CharismaEffective;
        }

        public string Process(INonPlayerCharacter npc, string command)
        {
            return command;
        }
    }
}
