﻿using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Item.Interface;

namespace Objects.Command.PC
{
    public class Sell : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Sell() : base(nameof(Sell), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Sell {Item Keyword}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sell" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    if (personality is IMerchant merchantMasterPersonality)
                    {
                        if (command.Parameters.Count > 0)
                        {
                            IItem item = null;
                            foreach (IItem i in performer.Items)
                            {
                                if (i.KeyWords.Contains(command.Parameters[0].ParameterValue, StringComparer.CurrentCultureIgnoreCase))
                                {
                                    item = i;
                                    break;
                                }
                            }
                            if (item != null)
                            {
                                return merchantMasterPersonality.Sell(npc, performer, item);
                            }
                            else
                            {
                                return merchantMasterPersonality.Offer(npc, performer);
                            }
                        }
                        else
                        {
                            return merchantMasterPersonality.Offer(npc, performer);
                        }
                    }
                }
            }

            return new Result("There is no merchant here to sell to.", true);
        }
    }
}
