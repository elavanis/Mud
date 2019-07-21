﻿using Objects.Command.Interface;
using System.Collections.Generic;
using System.Text;
using Objects.Mob.Interface;
using Objects.Global;
using static Objects.Global.Direction.Directions;
using Objects.Room.Interface;
using Objects.Language;

namespace Objects.Command.PC
{
    public class Move : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(InstructionText(), true);

        private static string InstructionText()
        {
            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendLine("(N)orth");
            strBldr.AppendLine("(E)ast");
            strBldr.AppendLine("(S)outh");
            strBldr.AppendLine("(W)est");
            strBldr.AppendLine("(U)p");
            strBldr.Append("(D)own");
            return strBldr.ToString();
        }

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "N", "North", "E", "East", "S", "South", "W", "West", "U", "Up", "D", "Down" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            //check to see if they are mounted on their mount
            if (performer.Mount != null
                && performer.Mount.Room == performer.Room
                && performer.Mount.Riders.Contains(performer))
            {
                //they are mounted, send the move to command to their mount
                performer.Mount.EnqueueCommand(command.Parameters[0].ParameterValue);

                return null;
            }
            else
            {
                IResult result = GlobalReference.GlobalValues.CanMobDoSomething.Move(performer);
                if (result != null)
                {
                    return result;
                }

                IRoom room = performer.Room;

                Direction direction = FindDirection(command);
                IExit exit = FindExit(direction, room);

                result = ValidateExit(exit);
                if (result != null)
                {
                    return result;
                }

                result = room.CheckLeave(performer);
                if (result != null)
                {
                    return result;
                }

                result = room.CheckLeaveDirection(performer, direction);
                if (result != null)
                {
                    return result;
                }

                IRoom proposedRoom = GlobalReference.GlobalValues.World.Zones[exit.Zone].Rooms[exit.Room];

                //we are safe to cross zones because we make a copy of each pc/npc list before processing it in a thread safe manor
                return MoveToRoom(performer, room, direction, proposedRoom);
            }
        }

        private IResult MoveToRoom(IMobileObject performer, IRoom room, Direction direction, IRoom proposedRoom)
        {
            IResult result = TryToMoveToRoom(performer, room, direction, proposedRoom);

            IMount mount = performer as IMount;
            if (mount != null)
            {
                if (mount.Riders.Count > 0)
                {
                    mount.Riders[0].EnqueueMessage(result.ResultMessage);
                }
            }

            return result;
        }

        private static IResult TryToMoveToRoom(IMobileObject performer, IRoom room, Direction direction, IRoom proposedRoom)
        {
            IResult result = proposedRoom.CheckEnter(performer);
            if (result != null)
            {
                return result;
            }

            if (room.Leave(performer, direction))
            {
                performer.Room = proposedRoom;
                proposedRoom.Enter(performer);

                //Announce the mob leaving
                GlobalReference.GlobalValues.Notify.Room(performer, null, room, new TranslationMessage($"{performer.SentenceDescription} leaves {direction}."), null, true);

                //Announce the mob entering
                GlobalReference.GlobalValues.Notify.Room(performer, null, proposedRoom, new TranslationMessage($"{performer.SentenceDescription} enters the room."), null, true);

                //take the result of the look, change it so they can't move again and return it.  
                result = GlobalReference.GlobalValues.CommandList.PcCommandsLookup["LOOK"].PerformCommand(performer, new Command());
                result.AllowAnotherCommand = false;
                return result;
            }

            //the character was moved before they could move to the desired room.
            return new Result("", false);
        }

        private Direction FindDirection(ICommand command)
        {
            string firstCharacter = command.CommandName.Substring(0, 1);

            switch (firstCharacter)
            {
                case "N":
                    return Direction.North;
                case "E":
                    return Direction.East;
                case "S":
                    return Direction.South;
                case "W":
                    return Direction.West;
                case "U":
                    return Direction.Up;
                case "D":
                default:
                    return Direction.Down;
            }
        }

        private static IExit FindExit(Direction direction, IRoom room)
        {
            IExit exit = null;
            switch (direction)
            {
                case Direction.North:
                    exit = room.North;
                    break;
                case Direction.East:
                    exit = room.East;
                    break;
                case Direction.South:
                    exit = room.South;
                    break;
                case Direction.West:
                    exit = room.West;
                    break;
                case Direction.Up:
                    exit = room.Up;
                    break;
                case Direction.Down:
                    exit = room.Down;
                    break;
            }

            return exit;
        }

        private IResult ValidateExit(IExit exit)
        {
            if (exit == null)
            {
                return new Result("There is no obvious way to leave that way.", true);
            }

            if (exit.Door != null && exit.Door.Opened == false)
            {
                string message = string.Format("You will need to open the {0} first.", exit.Door.SentenceDescription);
                return new Result(message, true);
            }

            return null;
        }
    }
}
