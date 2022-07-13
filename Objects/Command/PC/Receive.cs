using Objects.Command.Interface;
using Objects.Crafting.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Command.PC
{
    public class Receive : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Receive() : base(nameof(Receive), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Receive", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Receive" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (!(performer is IPlayerCharacter pc))
            {
                return new Result("Only player characters can receive.", true);
            }

            Tuple<INonPlayerCharacter, ICraftsman> foundCraftsman = null;
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    if (personality is ICraftsman craftsman)
                    {
                        foundCraftsman = new Tuple<INonPlayerCharacter, ICraftsman>(npc, craftsman);
                        break;
                    }
                }
                if (foundCraftsman != null)
                {
                    break;
                }
            }

            if (foundCraftsman == null)
            {
                return new Result("No craftsman found at this location.", true);
            }

            bool foundItem = false;
            for (int i = pc.CraftsmanObjects.Count - 1; i >= 0; i--)
            {
                ICraftsmanObject craftsmanObject = pc.CraftsmanObjects[i];

                if (craftsmanObject.CraftsmanId.Zone == foundCraftsman.Item1.ZoneId
                    && craftsmanObject.CraftsmanId.Id == foundCraftsman.Item1.Id
                    && craftsmanObject.Completion < DateTime.Now)
                {
                    pc.Items.Add(craftsmanObject.Item);
                    pc.CraftsmanObjects.RemoveAt(i);
                    foundCraftsman.Item1.EnqueueCommand($"Tell {pc.KeyWords[0]} As promised, here is your {craftsmanObject.Item.KeyWords[0]}.");
                    foundItem = true;
                }
            }

            if (foundItem)
            {
                return new Result("", false);
            }
            else
            {
                foundCraftsman.Item1.EnqueueCommand($"Tell {pc.KeyWords[0]} Sorry I don't have anything for you to pick up at this time.");
                return new Result("", true);
            }
        }
    }
}
