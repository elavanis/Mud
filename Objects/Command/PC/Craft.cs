using Objects.Command.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;

namespace Objects.Command.PC
{
    public class Craft : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Craft [Position] [Level] [Keyword] [\"SentenceDescription\"] [\"ShortDescription\"] [\"LookDescription\"] [\"ExamineDescription\"] {DamageType}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Craft" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (!(performer is IPlayerCharacter pc))
            {
                return new Result("Only player characters can have craftsman craft items.", true);
            }

            INonPlayerCharacter craftsman = null;
            ICraftsman craftsmanPersonality = null;
            FindCraftsman(performer, ref craftsman, ref craftsmanPersonality);

            if (craftsman == null)
            {
                return new Result("There is no craftsman to make anything for you.", true);
            }

            if (command.Parameters.Count < 7)
            {
                return new Result("Please provide all the parameters needed for the craftsman to make your item.", true);
            }

            try
            {
                AvalableItemPosition position = GetPosition(command.Parameters[0].ParameterValue);
                if (position == AvalableItemPosition.Wield && command.Parameters.Count < 8)
                {
                    return new Result("Damage type is required for weapons.", true);
                }

                int level = int.Parse(command.Parameters[1].ParameterValue);
                string keyword = command.Parameters[2].ParameterValue;
                string sentenceDescription = command.Parameters[3].ParameterValue;
                string shortDescription = command.Parameters[4].ParameterValue;
                string lookDescription = command.Parameters[5].ParameterValue;
                string examineDescription = command.Parameters[6].ParameterValue;
                DamageType damageType = DamageType.Acid;
                if (position == AvalableItemPosition.Wield)
                {
                    damageType = GetDamageType(command.Parameters[7].ParameterValue);
                }

                if (level > craftsman.Level)
                {
                    craftsman.EnqueueCommand($"Tell {performer.KeyWords[0]} That is above my skill level.  You will need to find someone a higher level to craft such an item.");
                    return new Result(null, true);
                }

                return craftsmanPersonality.Build(craftsman, pc, position, level, keyword, sentenceDescription, shortDescription, lookDescription, examineDescription, damageType);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return new Result(ex.Message, true);
                }
                else
                {
                    return new Result("Please verify all parameters and try again.", true);
                }
            }
        }

        private DamageType GetDamageType(string damageType)
        {
            string damageTypeLower = damageType.ToLower();

            switch (damageTypeLower)
            {
                case "bludgeon":
                    return DamageType.Bludgeon;
                case "pierce":
                    return DamageType.Pierce;
                case "slash":
                    return DamageType.Slash;
                default:
                    throw new ArgumentException(
@"Available damage types are Bludgeon,
                             Pierce,
                             Slash");
            }
        }

        private static void FindCraftsman(IMobileObject performer, ref INonPlayerCharacter craftsman, ref ICraftsman craftsmanPersonality)
        {
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    if (personality is ICraftsman)
                    {
                        craftsmanPersonality = personality as ICraftsman;
                        craftsman = npc;
                        break;
                    }
                }

                if (craftsman != null)
                {
                    break;
                }
            }
        }

        private AvalableItemPosition GetPosition(string position)
        {
            string positionLowerCase = position.ToLower();

            switch (positionLowerCase)
            {
                case "arms":
                    return AvalableItemPosition.Arms;
                case "body":
                    return AvalableItemPosition.Body;
                case "feet":
                    return AvalableItemPosition.Feet;
                case "finger":
                    return AvalableItemPosition.Finger;
                case "hand":
                    return AvalableItemPosition.Hand;
                case "head":
                    return AvalableItemPosition.Head;
                case "held":
                    return AvalableItemPosition.Held;
                case "legs":
                    return AvalableItemPosition.Legs;
                case "neck":
                    return AvalableItemPosition.Neck;
                case "waist":
                    return AvalableItemPosition.Waist;
                case "wield":
                    return AvalableItemPosition.Wield;
                default:
                    throw new ArgumentException(
@"Available positions are Wield,
                          Head,
                          Neck,
                          Arms,
                          Hand,
                          Finger,
                          Body,
                          Waist,
                          Legs,
                          Feet,
                          Held ");
            }
        }
    }
}
