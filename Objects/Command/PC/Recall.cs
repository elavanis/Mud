using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Room;
using Objects.Global;
using Objects.Item.Items.Interface;

namespace Objects.Command.PC
{
    public class Recall : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Recall() : base(nameof(Recall), ShortCutCharPositions.Standing) { }

        public IResult Instructions { get; } = new Result("Recall {Set}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Recall" };

        private IResult SuccesfulRecallResult { get; } = new Result("Your body begins to shimmer and become translucent.\r\nThe surroundings begin to fade to black and then new scenery appears before you.\r\nSlowly your body becomes solid again and you can see the recall crystal in front of you.", false);

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count >= 1)
            {
                string param = command.Parameters[0].ParameterValue;
                if (param.Equals("Set", StringComparison.CurrentCultureIgnoreCase))
                {
                    IRecallBeacon beacon = null;
                    foreach (IItem item in performer.Room.Items)
                    {
                        beacon = item as IRecallBeacon;
                        if (beacon != null)
                        {
                            break;
                        }
                    }

                    if (beacon == null)
                    {
                        return new Result("There is no recall beacon here.", true);
                    }
                    else
                    {
                        performer.RecallPoint = new RoomId(performer.Room);

                        return new Result("Recall point set.", false);
                    }
                }
            }

            return PerformRecall(performer);
        }

        private IResult PerformRecall(IMobileObject performer)
        {
            if (performer.RecallPoint != null)
            {
                IRoom currentRoom = performer.Room;

                if (currentRoom.Attributes.Contains(Room.Room.RoomAttribute.NoRecall))
                {
                    return new Result("You try to recall but your body is held in place.", true);
                }

                IRoom targetRoom = null;
                try
                {
                    targetRoom = GlobalReference.GlobalValues.World.Zones[performer.RecallPoint.Zone].Rooms[performer.RecallPoint.Id];
                }
                catch
                {
                }

                if (targetRoom != null)
                {
                    if (performer is IPlayerCharacter pc)
                    {
                        currentRoom.RemoveMobileObjectFromRoom(pc);
                        targetRoom.AddMobileObjectToRoom(pc);
                        pc.Room = targetRoom;
                        pc.EnqueueCommand("Look");

                        return SuccesfulRecallResult;
                    }
                    else
                    {
                        INonPlayerCharacter npc = performer as INonPlayerCharacter;
                        currentRoom.RemoveMobileObjectFromRoom(npc);
                        targetRoom.AddMobileObjectToRoom(npc);
                        npc.Room = targetRoom;
                        npc.EnqueueCommand("Look");

                        return SuccesfulRecallResult;
                    }
                }
                else
                {
                    return new Result("Invalid recall point defined.", true);
                }
            }
            else
            {
                return new Result("No recall point defined.", true);
            }
        }
    }
}
