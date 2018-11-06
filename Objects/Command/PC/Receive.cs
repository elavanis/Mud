using Objects.Command.Interface;
using Objects.Crafting.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Receive : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Receive", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Receive" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IPlayerCharacter pc = performer as IPlayerCharacter;

            if (pc == null)
            {
                return new Result("Only player characters can receive.", true);
            }

            Tuple<INonPlayerCharacter, ICraftsman> foundCraftsman = null;
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    ICraftsman craftsman = personality as ICraftsman;
                    if (craftsman != null)
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

                if (craftsmanObject.CraftsmanId.Zone == foundCraftsman.Item1.Zone
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
