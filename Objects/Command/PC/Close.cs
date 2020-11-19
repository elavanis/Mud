using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace Objects.Command.PC
{
    public class Close : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Close() : base(nameof(Close), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Close [Item Name]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Close" };


        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count == 0)
            {
                return new Result("What would you like to close?", true);
            }

            IParameter parameter = command.Parameters[0];
            IBaseObject foundItem = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameter.ParameterValue, parameter.ParameterNumber, true, true, false, false, true);

            if (foundItem != null)
            {
                if (foundItem is IDoor door)
                {
                    if (door.Linked)
                    {
                        IRoom otherRoom = GlobalReference.GlobalValues.World.Zones[door.LinkedRoomId.Zone].Rooms[door.LinkedRoomId.Id];
                        IDoor otherDoor = null;
                        switch (door.LinkedRoomDirection)
                        {
                            case Direction.North:
                                otherDoor = otherRoom.North.Door;
                                break;
                            case Direction.East:
                                otherDoor = otherRoom.East.Door;
                                break;
                            case Direction.South:
                                otherDoor = otherRoom.South.Door;
                                break;
                            case Direction.West:
                                otherDoor = otherRoom.West.Door;
                                break;
                            case Direction.Up:
                                otherDoor = otherRoom.Up.Door;
                                break;
                            case Direction.Down:
                                otherDoor = otherRoom.Down.Door;
                                break;
                        }

                        if (otherDoor != null)
                        {
                            otherDoor.Locked = false;
                            otherDoor.Opened = false;
                        }
                    }

                    return door.Close(performer);
                }
                else
                {
                    if (foundItem is IOpenable openable)
                    {
                        return openable.Close(performer);
                    }
                }

                return new Result("You found what you were looking for but could not figure out how to close it.", true);
            }
            else
            {
                return new Result("You were unable to find that what you were looking for.", true);
            }
        }
    }
}