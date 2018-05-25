using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Items;
using Objects.Item.Interface;
using Objects.Room.Interface;
using Objects.Room;
using static Objects.Room.Room;
using Objects.Global;
using Objects.Item.Items.Interface;

namespace Objects.Command.PC
{
    public class Recall : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Recall {Set}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Recall" };

        private IResult SuccesfulRecallResult { get; } = new Result(true, "Your body begins to shimmer and become translucent.\r\nThe surroundings begin to fade to black and then new scenery appears before you.\r\nSlowly your body becomes solid again and you can see the recall crystal in front of you.");

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
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
                        return new Result(false, "There is no recall beacon here.");
                    }
                    else
                    {
                        performer.RecallPoint = new RoomId(performer.Room);

                        return new Result(true, "Recall point set.");
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
                    return new Result(false, "You try to recall but your body is held in place.");
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
                    IPlayerCharacter pc = performer as IPlayerCharacter;
                    if (pc != null)
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
                    return new Result(false, "Invalid recall point defined.");
                }
            }
            else
            {
                return new Result(false, "No recall point defined.");
            }
        }
    }
}
