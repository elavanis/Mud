﻿using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Command.PC
{
    public class Put : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Put() : base(nameof(Put), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Put [Item Name] [Container]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Put" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count < 2)
            {
                return Instructions;
            }
            else
            {
                IItem item = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, command.Parameters[0].ParameterValue).FirstOrDefault();

                if (item == null)
                {
                    return new Result($"You do not seem to be carrying {command.Parameters[0].ParameterValue}.", true);
                }

                IItem itemToPutIn = GlobalReference.GlobalValues.FindObjects.FindItemsInRoom(performer.Room, command.Parameters[1].ParameterValue).FirstOrDefault();

                if (itemToPutIn == null)
                {
                    return new Result($"You were unable to find {command.Parameters[1].ParameterValue}.", true);
                }

                IContainer container = itemToPutIn as IContainer;
                if (container != null)
                {
                    GlobalReference.GlobalValues.Engine.Event.Put(performer, item, container);

                    container.Items.Add(item);
                    performer.Items.Remove(item);

                    return new Result($"You put {item.SentenceDescription} in {itemToPutIn.SentenceDescription}.", false);
                }
                else
                {
                    return new Result($"You can not put things in {itemToPutIn.SentenceDescription}.", true);
                }
            }
        }
    }
}
